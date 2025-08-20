import { Observable, of } from 'rxjs';
import { Departamento } from '../../models/departamento.model';

const MOCK_DEPARTAMENTOS: Departamento[] = [
  { codigo: 'DEP001', descricao: 'Eletrônicos' },
  { codigo: 'DEP002', descricao: 'Roupas' },
  { codigo: 'DEP003', descricao: 'Livros' }
];

export class DepartamentoServiceMock {
  obterTodos(): Observable<Departamento[]> {
    return of(MOCK_DEPARTAMENTOS);
  }

  buscarPorCodigo(codigo: string): Observable<string> {
    const departamento = MOCK_DEPARTAMENTOS.find(d => d.codigo === codigo);
    return of(departamento?.descricao || 'Departamento não encontrado');
  }
}
