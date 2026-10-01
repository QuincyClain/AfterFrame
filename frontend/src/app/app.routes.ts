import { Routes } from '@angular/router';
import { CatalogPage } from './features/catalog/catalog-page/catalog-page';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'catalog',
    pathMatch: 'full',
  },
  {
    path: 'catalog',
    component: CatalogPage,
  },
];
