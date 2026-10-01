import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'catalog',
    pathMatch: 'full',
  },
  {
    path: 'catalog/:id',
    loadComponent: () =>
      import('./features/catalog/title-details-page/title-details-page').then(
        (module) => module.TitleDetailsPage,
      ),
  },
  {
    path: 'catalog',
    loadComponent: () =>
      import('./features/catalog/catalog-page/catalog-page').then(
        (module) => module.CatalogPage,
      ),
  },
  {
    path: 'profile',
    loadComponent: () =>
      import('./features/library/profile-page/profile-page').then(
        (module) => module.ProfilePage,
      ),
  },
  {
    path: '**',
    redirectTo: 'catalog',
  },
];
