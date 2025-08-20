import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'produtos',
    pathMatch: 'full'
  },
  {
    path: 'produtos',
    loadComponent: () => import('./components/produto/produto-list/produto-list.component').then(m => m.ProdutoListComponent)
  },
  {
    path: 'produtos/novo',
    loadComponent: () => import('./components/produto/produto-form/produto-form.component').then(m => m.ProdutoFormComponent)
  },
  {
    path: 'produtos/editar/:id',
    loadComponent: () => import('./components/produto/produto-form/produto-form.component').then(m => m.ProdutoFormComponent)
  },
  {
    path: '**',
    redirectTo: 'produtos'
  }
];
