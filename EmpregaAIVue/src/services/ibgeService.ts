import axios from 'axios';

export interface Cidade {
  id: number;
  nome: string;
}

const api = axios.create({
  baseURL: 'https://servicodados.ibge.gov.br/api/v1/localidades'
});

export const ibgeService = {
  async listarCidades(uf: string): Promise<Cidade[]> {
    if (!uf) return [];
    const response = await api.get<Cidade[]>(`/estados/${uf}/municipios?orderBy=nome`);
    return response.data;
  }
};