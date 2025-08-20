import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Departamento } from '../models/departamento.model';

@Injectable({
  providedIn: 'root'
})
export class DepartamentoService {
  private readonly apiUrl = 'https://localhost:7251/api/departamento';

  constructor(private http: HttpClient) { }

  obterTodos(): Observable<Departamento[]> {
    return this.http.get<Departamento[]>(this.apiUrl)
      .pipe(catchError(this.handleError));
  }

  obterPorId(codigo: string): Observable<string> {
    return this.http.get<string>(`${this.apiUrl}/${codigo}`)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Erro ao carregar departamentos';

    if (error.status === 0) {
      errorMessage = 'Não foi possível conectar ao servidor';
    }

    return throwError(() => errorMessage);
  }
}
