import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Router, RouterModule } from '@angular/router';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { Produto } from '../../../models/produto.model';
import { ProdutoService } from '../../../services/produto.service';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-produto-list',
  templateUrl: './produto-list.component.html',
  styleUrls: ['./produto-list.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatSnackBarModule
  ]
})
export class ProdutoListComponent implements OnInit {
  produtos: Produto[] = [];
  dataSource = new MatTableDataSource<Produto>([]);
  displayedColumns: string[] = ['codigo', 'descricao', 'departamento', 'preco', 'status', 'acoes'];
  loading = false;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private produtoService: ProdutoService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.carregarProdutos();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  carregarProdutos(): void {
    this.loading = true;
    this.produtoService.obterTodos().subscribe({
      next: (produtos) => {
        this.produtos = produtos;
        this.dataSource.data = produtos;
        this.loading = false;
      },
      error: (error) => {
        this.showError('Erro ao carregar produtos: ' + error);
        this.loading = false;
      }
    });
  }

  novoProduto(): void {
    this.router.navigate(['/produtos/novo']);
  }

  editarProduto(produto: Produto): void {
    this.router.navigate(['/produtos/editar', produto.id]);
  }

  excluirProduto(produto: Produto): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '350px',
      data: {
        title: 'Confirmar Exclusão',
        message: `Deseja realmente excluir o produto "${produto.descricao}"?`,
        confirmText: 'Excluir',
        cancelText: 'Cancelar'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.produtoService.excluir(produto.id).subscribe({
          next: () => {
            this.showSuccess('Produto excluído com sucesso');
            this.carregarProdutos();
          },
          error: (error) => {
            this.showError('Erro ao excluir produto: ' + error);
          }
        });
      }
    });
  }

  formatarPreco(preco: number): string {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(preco);
  }

  getStatusText(status: boolean): string {
    return status ? 'Ativo' : 'Inativo';
  }

  getDepartamentoDescricao(produto: Produto): string {
    return produto.departamentoDescricao || produto.departamentoCodigo || 'N/A';
  }

  private showSuccess(message: string): void {
    this.snackBar.open(message, 'Fechar', {
      duration: 3000,
      panelClass: ['success-snackbar']
    });
  }

  private showError(message: string): void {
    this.snackBar.open(message, 'Fechar', {
      duration: 5000,
      panelClass: ['error-snackbar']
    });
  }
}
