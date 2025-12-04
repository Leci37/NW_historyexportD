import { Injectable, computed, effect, signal } from '@angular/core';
import { PointsApi } from '../../../core/api/points.api';
import {
  PointDto,
  UpdatePointsDto,
  UpdatePointsResponseDto
} from '../../../core/models/points.model';

type EditableField = keyof Pick<
  UpdatePointsDto,
  'description' | 'device' | 'historyFastArch' | 'historySlowArch' | 'historyExtdArch'
>;
type PendingEdit = { pointId: number } & Partial<Omit<UpdatePointsDto, 'pointId'>>;

/**
 * Store ligero con Signals:
 * - rows: vista actual (se actualiza al editar en UI)
 * - originalById: referencia cargada desde API para calcular diffs / revertir
 * - edits: cambios pendientes por pointId
 */
@Injectable({ providedIn: 'root' })
export class PointsStore {
  // Estado base
  private readonly rows = signal<PointDto[]>([]);
  private readonly originalById = signal<Map<number, PointDto>>(new Map());
  private readonly edits =  signal<Record<number, PendingEdit>>({});

  // Flags
  readonly isLoading = signal(false);
  readonly isSaving = signal(false);
  readonly lastResponse = signal<UpdatePointsResponseDto | null>(null);
  readonly lastError = signal<string | null>(null);

  // Selectores
  readonly data = computed(() => this.rows());
  readonly hasChanges = computed(() => Object.keys(this.edits()).length > 0);

  /** Devuelve los cambios pendientes para una fila */
  changesFor = (id: number) => computed(() => this.edits()[id] ?? {});

  constructor(private api: PointsApi) {
    // Limpia errores al comenzar un guardado o carga (ayuda en UX)
    effect(() => {
      if (this.isSaving() || this.isLoading()) this.lastError.set(null);
    });
  }

  // --- API ---

  load() {
    this.isLoading.set(true);
    this.api.getAll().subscribe({
      next: (list) => {
        console.log(list);
        this.rows.set(list ?? []);
        // Map para comparación/revert
        const m = new Map<number, PointDto>();
        for (const r of list ?? []) m.set(r.pointId, { ...r });
        this.originalById.set(m);
        this.edits.set({});
        this.lastResponse.set(null);
      },
      error: (err) => this.lastError.set(err?.message ?? 'Error al cargar puntos'),
      complete: () => this.isLoading.set(false)
    });
  }

  /** Edición local de un campo (actualiza la vista y el registro de cambios) */
  editField(id: number, field: EditableField, value: unknown) {
    // 1) Actualiza la fila en la vista
    const copy = [...this.rows()];
    const idx = copy.findIndex(r => r.pointId === id);
    if (idx >= 0) {
      copy[idx] = { ...copy[idx], [field]: value as any };
      this.rows.set(copy);
    }

    // 2) Calcula diff contra original y registra en edits
    const orig = this.originalById().get(id);
    if (!orig) return;

    const nextEdits = { ...this.edits() };
    const partial: PendingEdit = { ...(nextEdits[id] ?? { pointId: id }), pointId: id };

    // Normalización equivalente al backend (trim en strings)
    const normalize = (v: unknown) =>
      typeof v === 'string' ? v.trim() : v;

    const current = (copy[idx] as any)?.[field];
    const original = (orig as any)?.[field];

    if (normalize(current) === normalize(original)) {
      // Si vuelve al valor original, quítalo del partial
      delete (partial as any)[field];
    } else {
      (partial as any)[field] = current;
    }

    // Quita el registro completo si ya no hay cambios
    if (Object.keys(partial).length <= 1) {
      delete nextEdits[id];
    } else {
      nextEdits[id] = partial;
    }
    this.edits.set(nextEdits);
  }

  /** Revierte los cambios de una fila */
  revertRow(id: number) {
    const orig = this.originalById().get(id);
    if (!orig) return;
    const next = [...this.rows()];
    const idx = next.findIndex(r => r.pointId === id);
    if (idx >= 0) next[idx] = { ...orig };
    this.rows.set(next);

    const e = { ...this.edits() };
    delete e[id];
    this.edits.set(e);
  }

  /** Empaqueta solo los cambios y envía al backend */
  saveAll() {
    const edits = this.edits();
    const payload: UpdatePointsDto[] = Object.values(edits);
    if (payload.length === 0) return;

    this.isSaving.set(true);
    this.api.updateBulk(payload).subscribe({
      next: (resp) => {
        this.lastResponse.set(resp);
        // Para cada updated/skipped, sincroniza original y limpia edit
        const updatedOrSkipped = new Set(
          resp.results
            .filter(r => r.status === 'updated' || r.status === 'skipped')
            .map(r => r.pointId)
        );

        if (updatedOrSkipped.size > 0) {
          const m = new Map(this.originalById());
          const list = [...this.rows()];
          for (let i = 0; i < list.length; i++) {
            const row = list[i];
            if (updatedOrSkipped.has(row.pointId)) {
              m.set(row.pointId, { ...row });
              const e = this.edits();
              if (e[row.pointId]) {
                const copy = { ...this.edits() };
                delete copy[row.pointId];
                this.edits.set(copy);
              }
            }
          }
          this.originalById.set(m);
        }
        // Si hubo errores/not_found/conflict, esos edits permanecen
      },
      error: (err) => this.lastError.set(err?.message ?? 'Error al guardar cambios'),
      complete: () => this.isSaving.set(false)
    });
  }
}
