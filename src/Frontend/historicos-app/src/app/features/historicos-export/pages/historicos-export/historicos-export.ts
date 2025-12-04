import { Component, OnInit, inject, effect, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PointsStore } from '../../store/points.store';
import { PointDto } from '../../../../core/models/points.model';
import { ExportService } from '../../../../core/utils/export.service';


type RowForm = FormGroup<{
  description: any;
  device: any;
  historyFastArch: any;
  historySlowArch: any;
  historyExtdArch: any;
}>;

@Component({
  selector: 'app-historicos-export',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './historicos-export.html',
  styleUrls: ['./historicos-export.scss']
})
export class HistoricosExport implements OnInit {
  private readonly store = inject(PointsStore);
  private readonly fb = inject(FormBuilder);
  private readonly exporter = inject(ExportService);
  
  data = this.store.data;
  hasChanges = this.store.hasChanges;
  isLoading = this.store.isLoading;
  isSaving = this.store.isSaving;
  lastError = this.store.lastError;
  lastResponse = this.store.lastResponse;

  trackByPointId = (_: number, item: PointDto) => item.pointId;
    // Mapa de forms por fila
  ngOnInit(): void {
     this.store.load();
  }

  onSave() { this.store.saveAll(); }

  onEditText(id: number, field: 'description' | 'device', value: string) {
    this.store.editField(id, field, value);
  }
  onEditBool(
    id: number,
    field: 'historyFastArch' | 'historySlowArch' | 'historyExtdArch',
    checked: boolean
  ) {
    this.store.editField(id, field, checked);
  }
  onRevert(id: number) { this.store.revertRow(id); }

  onExportExcel() {
    this.exporter.exportToExcel(this.data());
  }

  onExportPdf() {
    this.exporter.exportToPdf(this.data());
  }

}
