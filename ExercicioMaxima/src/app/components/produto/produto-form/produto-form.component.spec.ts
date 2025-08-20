import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ProdutoFormComponent } from './produto-form.component';
import { ProdutoService } from '../../../services/produto.service';
import { DepartamentoService } from '../../../services/departamento.service';
import { ProdutoServiceMock } from '../../../services/mocks/produto.service.mock';
import { DepartamentoServiceMock } from '../../../services/mocks/departamento.service.mock';
import { ActivatedRoute, Router } from '@angular/router';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatSnackBar } from '@angular/material/snack-bar';

describe('ProdutoFormComponent', () => {
  let component: ProdutoFormComponent;
  let fixture: ComponentFixture<ProdutoFormComponent>;
  let produtoService: ProdutoService;
  let departamentoService: DepartamentoService;

 beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        ProdutoFormComponent,
        HttpClientTestingModule,
        NoopAnimationsModule
      ],
      providers: [
        { provide: ProdutoService, useClass: ProdutoServiceMock },
        { provide: DepartamentoService, useClass: DepartamentoServiceMock },
        { provide: ActivatedRoute, useValue: {
          snapshot: {
            paramMap: {
              get: (key: string) => key === 'id' ? null : null
            }
          }
        }},
        { provide: Router, useValue: { navigate: () => {} } },
        { provide: MatSnackBar, useValue: { open: () => {} } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ProdutoFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('deve criar o componente', () => {
    expect(component).toBeTruthy();
  });

  it('deve carregar departamentos na inicialização', fakeAsync(() => {
    spyOn(departamentoService, 'obterTodos').and.callThrough();

    component.ngOnInit();
    tick();

    expect(departamentoService.obterTodos).toHaveBeenCalled();
    expect(component.departamentos.length).toBe(3);
  }));

  it('deve criar produto quando formulário válido', fakeAsync(() => {

    spyOn(produtoService, 'criar').and.callThrough();
    spyOn(component, 'voltar').and.callThrough();

    // Preenche formulário
    component.produtoForm.patchValue({
      codigo: 'TEST003',
      descricao: 'Novo Produto',
      departamentoCodigo: 'DEP001',
      preco: 50.99,
      status: true
    });

    component.onSubmit();
    tick();

    expect(produtoService.criar).toHaveBeenCalled();
    expect(component.voltar).toHaveBeenCalled();
  }));

  it('não deve submeter quando formulário inválido', () => {

    spyOn(produtoService, 'criar').and.callThrough();

    component.produtoForm.patchValue({
      codigo: '',
      descricao: '',
      departamentoCodigo: '',
      preco: null,
      status: true
    });

    component.onSubmit();

    expect(produtoService.criar).not.toHaveBeenCalled();
    expect(component.produtoForm.invalid).toBeTrue();
  });
});
