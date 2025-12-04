export interface PointDto {
  pointId: number;
  pointName?: string | null;
  paramName?: string | null;
  description?: string | null;
  device?: string | null;
  historyFast?: boolean | null;
  historySlow?: boolean | null;
  historyExtd?: boolean | null;
  historyFastArch?: boolean | null;
  historySlowArch?: boolean | null;
  historyExtdArch?: boolean | null;
}

export interface UpdatePointsDto {
  pointId: number;
  description?: string | null;
  device?: string | null;
  historyFastArch?: boolean | null;
  historySlowArch?: boolean | null;
  historyExtdArch?: boolean | null;
}

export interface UpdatePointsResponseDto {
  processedCount: number;
  updatedCount: number;
  failedCount: number;
  results: ItemResult[];
}

export interface ItemResult {
  pointId: number;
  status: 'updated' | 'skipped' | 'not_found' | 'conflict' | 'error' | string;
  message?: string | null;
  changedFields?: string[] | null;
}
