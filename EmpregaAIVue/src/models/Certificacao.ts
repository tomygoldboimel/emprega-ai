export interface Certificacao {
  id: string; 
  curriculoId: string;
  nome: string;
  instituicao: string;
  dataConclusao: string;
  urlCertificado: string;
  cargaHoraria: number;
  excluido: boolean;
}