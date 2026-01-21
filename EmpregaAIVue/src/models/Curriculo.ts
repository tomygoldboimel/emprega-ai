export interface Curriculo {
  id: string; 
  nome: string;
  dataNascimento: string;
  email: string;
  telefone: string | null;
  endereco: string | null;
  cidade: string | null;
  estado: string | null;
  objetivo: string | null;
  excluido: boolean;
}