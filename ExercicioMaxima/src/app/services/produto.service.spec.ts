import { TestBed } from '@angular/core/testing';
import { ProdutoService } from './produto.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ProdutoCreateDto } from '../models/produto.model';

describe('ProdutoService', () => {
  let service: ProdutoService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ProdutoService]
    });

    service = TestBed.inject(ProdutoService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('deve criar produto', () => {
    const produtoDto: ProdutoCreateDto = {
      codigo: 'TEST001',
      descricao: 'Produto Teste',
      departamentoCodigo: 'DEP001',
      preco: 100,
      status: true
    };

    service.criar(produtoDto).subscribe(produto => {
      expect(produto.codigo).toBe('TEST001');
    });

    const req = httpMock.expectOne('https://localhost:7251/api/produto');
    expect(req.request.method).toBe('POST');
    req.flush({ ...produtoDto, id: '1' });
  });
});
