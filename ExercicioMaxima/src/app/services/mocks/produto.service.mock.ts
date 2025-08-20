import { Observable, of, throwError } from 'rxjs';
import { Produto, ProdutoCreateDto, ProdutoUpdateDto } from '../../models/produto.model';

// Mock data
const MOCK_PRODUTOS: Produto[] = [
  {
    id: '1',
    codigo: 'PROD001',
    descricao: 'Produto Teste 1',
    departamentoCodigo: 'DEP001',
    preco: 100.50,
    status: true,
    departamentoDescricao: 'Departamento 1'
  },
  {
    id: '2',
    codigo: 'PROD002',
    descricao: 'Produto Teste 2',
    departamentoCodigo: 'DEP002',
    preco: 200.75,
    status: false,
    departamentoDescricao: 'Departamento 2'
  }
];

export class ProdutoServiceMock {
  private produtos = [...MOCK_PRODUTOS];

  obterTodos(): Observable<Produto[]> {
    return of(this.produtos);
  }

  obterPorId(id: string): Observable<Produto> {
    const produto = this.produtos.find(p => p.id === id);
    return produto ? of(produto) : throwError(() => new Error('Produto não encontrado'));
  }

  criar(produto: ProdutoCreateDto): Observable<Produto> {
    const novoProduto: Produto = {
      id: (this.produtos.length + 1).toString(),
      ...produto,
      departamentoDescricao: `Departamento ${produto.departamentoCodigo}`
    };
    this.produtos.push(novoProduto);
    return of(novoProduto);
  }

  atualizar(id: string, produto: ProdutoUpdateDto): Observable<Produto> {
    const index = this.produtos.findIndex(p => p.id === id);
    if (index === -1) {
      return throwError(() => new Error('Produto não encontrado'));
    }

    this.produtos[index] = { ...this.produtos[index], ...produto };
    return of(this.produtos[index]);
  }

  excluir(id: string): Observable<void> {
    const index = this.produtos.findIndex(p => p.id === id);
    if (index === -1) {
      return throwError(() => new Error('Produto não encontrado'));
    }

    this.produtos.splice(index, 1);
    return of(void 0);
  }
}
