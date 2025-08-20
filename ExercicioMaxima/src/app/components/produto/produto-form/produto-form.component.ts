import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { ProdutoService } from '../../../services/produto.service';
import { DepartamentoService } from '../../../services/departamento.service';
import { Produto, ProdutoCreateDto, ProdutoUpdateDto } from '../../../models/produto.model';
import { Departamento } from '../../../models/departamento.model';

@Component({
  selector: 'app-produto-form',
  templateUrl: './produto-form.component.html',
  styleUrls: ['./produto-form.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatSelectModule,
    MatSlideToggleModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule
  ]
})
export class ProdutoFormComponent implements OnInit {
  produtoForm!: FormGroup;
  departamentos: Departamento[] = [];
  isEditMode = false;
  produtoId: string | null = null;
  loading = false;
  submitting = false;

  constructor(
    private fb: FormBuilder,
    private produtoService: ProdutoService,
    private departamentoService: DepartamentoService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.initForm();
  }

  ngOnInit(): void {
    this.produtoId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.produtoId;

    this.carregarDepartamentos();

    if (this.isEditMode) {
      this.carregarProduto();
    }
  }

  private initForm(): void {
    this.produtoForm = this.fb.group({
      codigo: ['', [Validators.required, Validators.maxLength(50)]],
      descricao: ['', [Validators.required, Validators.maxLength(500)]],
      departamentoCodigo: ['', [Validators.required]],
      preco: ['', [Validators.required, Validators.min(0.01)]],
      status: [true]
    });
  }

  private carregarDepartamentos(): void {
    this.departamentoService.obterTodos().subscribe({
      next: (departamentos) => {
        this.departamentos = departamentos;
      },
      error: (error) => {
        this.showError('Erro ao carregar departamentos: ' + error);
      }
    });
  }

  private carregarProduto(): void {
    if (!this.produtoId) return;

    this.loading = true;
    this.produtoService.obterPorId(this.produtoId).subscribe({
      next: (produto) => {
        this.produtoForm.patchValue({
          codigo: produto.codigo,
          descricao: produto.descricao,
          departamentoCodigo: produto.departamentoCodigo,
          preco: produto.preco,
          status: produto.status
        });
        this.loading = false;
      },
      error: (error) => {
        this.showError('Erro ao carregar produto: ' + error);
        this.loading = false;
        this.voltar();
      }
    });
  }

  onSubmit(): void {
    if (this.produtoForm.invalid) {
      this.markFormGroupTouched();
      return;
    }

    this.submitting = true;
    const formValue = this.produtoForm.value;

    if (this.isEditMode && this.produtoId) {
      const produto: ProdutoUpdateDto = {
        codigo: String(this.produtoForm.get('codigo')?.value),
        descricao: String(this.produtoForm.get('descricao')?.value),
        departamentoCodigo: String(this.produtoForm.get('departamentoCodigo')?.value),
        preco: Number(this.produtoForm.get('preco')?.value),
        status: Boolean(this.produtoForm.get('status')?.value)
      };

      this.produtoService.atualizar(this.produtoId, produto).subscribe({
        next: () => {
          this.showSuccess('Produto atualizado com sucesso');
          this.voltar();
        },
        error: (error) => {
          this.showError('Erro ao atualizar produto: ' + error);
          this.submitting = false;
        }
      });
    } else {
      const produto: ProdutoCreateDto = {
        codigo: String(this.produtoForm.get('codigo')?.value),
        descricao: String(this.produtoForm.get('descricao')?.value),
        departamentoCodigo: String(this.produtoForm.get('departamentoCodigo')?.value),
        preco: Number(this.produtoForm.get('preco')?.value),
        status: Boolean(this.produtoForm.get('status')?.value)
      };

      this.produtoService.criar(produto).subscribe({
        next: (produtoCriado) => {
          console.log('Produto criado:', produtoCriado);
          this.showSuccess('Produto criado com sucesso');
          this.voltar();
        },
        error: (error) => {
          this.showError('Erro ao criar produto: ' + error);
          this.submitting = false;
        }
      });
    }
  }

  voltar(): void {
    this.router.navigate(['/produtos']);
  }

  getErrorMessage(fieldName: string): string {
    const control = this.produtoForm.get(fieldName);

    if (control?.hasError('required')) {
      return `${this.getFieldLabel(fieldName)} é obrigatório`;
    }

    if (control?.hasError('maxlength')) {
      const maxLength = control.errors?.['maxlength'].requiredLength;
      return `${this.getFieldLabel(fieldName)} deve ter no máximo ${maxLength} caracteres`;
    }

    if (control?.hasError('min')) {
      return 'Preço deve ser maior que zero';
    }

    return '';
  }

  private getFieldLabel(fieldName: string): string {
    const labels: { [key: string]: string } = {
      codigo: 'Código',
      descricao: 'Descrição',
      departamentoCodigo: 'Departamento', // ✅ Atualizado
      preco: 'Preço'
    };
    return labels[fieldName] || fieldName;
  }

  private markFormGroupTouched(): void {
    Object.keys(this.produtoForm.controls).forEach(key => {
      const control = this.produtoForm.get(key);
      control?.markAsTouched();
    });
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
