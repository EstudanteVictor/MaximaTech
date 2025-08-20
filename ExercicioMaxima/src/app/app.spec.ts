import { Injectable } from "@angular/core";
import { Produto, ProdutoCreateDto, ProdutoUpdateDto } from "./models/produto.model";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class ProdutoService {
   constructor(private http: HttpClient) {}

  private readonly apiUrl = 'https://localhost:7251/api/produto';
  // ou: private readonly apiUrl = 'https://localhost:7251/api/produtos';

  obterTodos(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this.apiUrl);
  }

  obterPorId(id: string): Observable<Produto> {
    return this.http.get<Produto>(`${this.apiUrl}/${id}`);
  }

  criar(produto: ProdutoCreateDto): Observable<Produto> {
    return this.http.post<Produto>(this.apiUrl, produto);
  }

  atualizar(id: string, produto: ProdutoUpdateDto): Observable<Produto> {
    return this.http.put<Produto>(`${this.apiUrl}/${id}`, produto);
  }

  excluir(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
