export interface Formacao {
  id: string;
  curriculoId: string; 
  instituicao: string | null;
  curso: string | null;
  status: boolean | null;
  excluido: boolean | null;
}