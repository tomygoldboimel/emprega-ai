export interface Formacao {
  id: string;
  curriculoId: string; 
  instituicao: string;
  curso: string;
  nivel: string;
  status: string;
  dataInicio: string;
  dataConclusao: string;
  excluido: boolean;
}