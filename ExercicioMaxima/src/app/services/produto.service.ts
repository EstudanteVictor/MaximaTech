import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Produto, ProdutoCreateDto, ProdutoUpdateDto } from '../models/produto.model';

@Injectable({
  providedIn: 'root'
})
export class ProdutoService {
  private readonly apiUrl = 'https://localhost:7251/api/produto';

  constructor(private http: HttpClient) {}

  obterTodos(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this.apiUrl)
      .pipe(catchError(this.handleError));
  }

  obterPorId(id: string): Observable<Produto> {
    return this.http.get<Produto>(`${this.apiUrl}/${id}`)
      .pipe(catchError(this.handleError));
  }

  criar(produto: ProdutoCreateDto): Observable<Produto> {
    console.log('Enviando para API:', produto);

    return this.http.post<Produto>(this.apiUrl, produto)
      .pipe(
        catchError(error => {
          console.error('Erro completo:', error);
          return this.handleError(error);
        })
      );
  }

  atualizar(id: string, produto: ProdutoUpdateDto): Observable<Produto> {
    return this.http.put<Produto>(`${this.apiUrl}/${id}`, produto)
      .pipe(catchError(this.handleError));
  }

  excluir(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Erro desconhecido';

    if (error.error instanceof ErrorEvent) {
      // Erro do cliente
      errorMessage = `Erro: ${error.error.message}`;
    } else {
      // Erro do servidor
      if (error.status === 409) {
        errorMessage = 'Código do produto já existe';
      } else if (error.status === 404) {
        errorMessage = 'Produto não encontrado';
      } else if (error.status === 400) {
        errorMessage = 'Dados inválidos';
      } else if (error.status === 0) {
        errorMessage = 'Não foi possível conectar ao servidor';
      } else {
        errorMessage = error.error?.message || `Erro: ${error.status}`;
      }
    }
    return throwError(() => errorMessage);
  }
}
