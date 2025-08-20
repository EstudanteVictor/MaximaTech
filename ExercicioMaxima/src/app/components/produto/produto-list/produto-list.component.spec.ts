import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ProdutoListComponent } from './produto-list.component';
import { ProdutoService } from '../../../services/produto.service';
import { ProdutoServiceMock } from '../../../services/mocks/produto.service.mock';
import { MatDialog } from '@angular/material/dialog';
import { of } from 'rxjs';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

describe('ProdutoListComponent', () => {
  let component: ProdutoListComponent;
  let fixture: ComponentFixture<ProdutoListComponent>;
  let produtoService: ProdutoService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        ProdutoListComponent,
        NoopAnimationsModule
      ],
      providers: [
        { provide: ProdutoService, useClass: ProdutoServiceMock },
        { provide: MatDialog, useValue: { open: () => ({ afterClosed: () => of(true) }) } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ProdutoListComponent);
    component = fixture.componentInstance;
    produtoService = TestBed.inject(ProdutoService);
    fixture.detectChanges();
  });

  it('deve criar o componente', () => {
    expect(component).toBeTruthy();
  });

  it('deve carregar produtos na inicialização', fakeAsync(() => {
    // Arrange
    spyOn(produtoService, 'obterTodos').and.callThrough();

    // Act
    component.ngOnInit();
    tick(); // Espera async operations

    // Assert
    expect(produtoService.obterTodos).toHaveBeenCalled();
    expect(component.produtos.length).toBe(2);
    expect(component.loading).toBeFalse();
  }));

  it('deve exibir produtos na tabela', fakeAsync(() => {
    // Act
    component.ngOnInit();
    tick();
    fixture.detectChanges();

    // Assert
    const tableRows = fixture.nativeElement.querySelectorAll('mat-row');
    expect(tableRows.length).toBe(2);

    const firstRowCells = tableRows[0].querySelectorAll('mat-cell');
    expect(firstRowCells[0].textContent).toContain('PROD001');
    expect(firstRowCells[1].textContent).toContain('Produto Teste 1');
  }));

  it('deve chamar exclusão quando confirmado', fakeAsync(() => {
    // Arrange
    spyOn(produtoService, 'excluir').and.callThrough();
    const produto = component.produtos[0];

    // Act
    component.excluirProduto(produto);
    tick();

    // Assert
    expect(produtoService.excluir).toHaveBeenCalledWith('1');
  }));
});
