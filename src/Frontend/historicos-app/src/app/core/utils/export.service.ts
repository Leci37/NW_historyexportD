import { Injectable } from '@angular/core';
import * as XLSX from 'xlsx';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { PointDto } from '../models/points.model';

@Injectable({ providedIn: 'root' })
export class ExportService {

  /** Exporta los datos visibles en pantalla a Excel */
  exportToExcel(points: PointDto[]): void {
    if (!points || points.length === 0) return;

    // Convertir a una estructura plana (solo columnas relevantes)
    const data = points.map(p => ({
      ID: p.pointId,
      Punto: p.pointName ?? '',
      Parámetro: p.paramName ?? '',
      Descripción: p.description ?? '',
      Dispositivo: p.device ?? '',
      'Arch. Rápida': this.bool(p.historyFastArch),
      'Arch. Lenta': this.bool(p.historySlowArch),
      'Arch. Ext.': this.bool(p.historyExtdArch),
    }));

    const worksheet = XLSX.utils.json_to_sheet(data);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Puntos');

    const filename = this.getFileName('ExportFile', 'xlsx');
    XLSX.writeFile(workbook, filename);
  }

  /** Exporta los datos visibles en pantalla a PDF */
  exportToPdf(points: PointDto[]): void {
    if (!points || points.length === 0) return;

    const doc = new jsPDF({ orientation: 'landscape' });
    const dateStr = new Date().toLocaleString();
    const filename = this.getFileName('ExportFile', 'pdf');

    doc.setFontSize(12);
    doc.text(`Listado de Puntos (${dateStr})`, 14, 12);

    const head = [[
      'ID', 'Punto', 'Parámetro', 'Descripción', 'Dispositivo',
      'Arch. Rápida', 'Arch. Lenta', 'Arch. Ext.'
    ]];

    const body = points.map(p => [
      p.pointId,
      p.pointName ?? '',
      p.paramName ?? '',
      p.description ?? '',
      p.device ?? '',
      this.bool(p.historyFastArch),
      this.bool(p.historySlowArch),
      this.bool(p.historyExtdArch),
    ]);

    autoTable(doc, {
      head,
      body,
      startY: 18,
      styles: { fontSize: 8 },
      headStyles: { fillColor: [230, 230, 230] },
    });

    doc.save(filename);
  }

  /** Helpers */
  private bool(v?: boolean | null): string {
    return v ? 'X' : '';
  }

  private getFileName(prefix: string, ext: string): string {
    const now = new Date();
    const pad = (n: number) => n.toString().padStart(2, '0');
    const name = `${prefix}_${now.getFullYear()}${pad(now.getMonth() + 1)}${pad(now.getDate())}_${pad(now.getHours())}${pad(now.getMinutes())}`;
    return `${name}.${ext}`;
  }
}
