export interface Produto {
  id: string;
  codigo: string;
  descricao: string;
  preco: number;
  status: boolean;
  departamentoCodigo: string;
  departamentoDescricao?: string;
}

export interface ProdutoCreateDto {
  codigo: string;
  departamentoCodigo: string;
  descricao: string;
  preco: number;
  status: boolean;
}

export interface ProdutoUpdateDto {
  codigo: string;
  descricao: string;
  preco: number;
  status: boolean;
  departamentoCodigo: string;
}
