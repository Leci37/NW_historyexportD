import { Routes } from '@angular/router';

export const routes: Routes = [
   {
    path: '',
    pathMatch: 'full',
    loadComponent: () =>
      import('./features/historicos-export/pages/historicos-export/historicos-export')
        .then(m => m.HistoricosExport)
  },
];
