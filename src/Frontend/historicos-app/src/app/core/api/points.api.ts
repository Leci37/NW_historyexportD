import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../config/app.config';
import { PointDto, UpdatePointsDto, UpdatePointsResponseDto } from '../models/points.model';

@Injectable({ providedIn: 'root' })
export class PointsApi {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getAll() {
    // GET /api/points
    return this.http.get<PointDto[]>(this.baseUrl);
  }

  updateBulk(items: UpdatePointsDto[]) {
    // POST /api/points
    console.log(items);
    return this.http.post<UpdatePointsResponseDto>(this.baseUrl, items, {
      // Opcional: observar respuesta completa si quisieras leer status 207
      observe: 'body'
    });
  }
}
