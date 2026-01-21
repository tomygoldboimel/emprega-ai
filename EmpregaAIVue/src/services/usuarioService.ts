import axios from 'axios';
import type { Usuario } from 'src/models/Usuario';

const API_URL = 'https://localhost:7274/api/Usuario';

const api = axios.create({
  baseURL: 'http://localhost:7274/api',
  withCredentials: true
});

class UsuarioService {
  async adicionarUsuario(usuario: Omit<Usuario, 'id' | 'ativo' | 'excluido'>): Promise<Usuario> {
    const response = await axios.post<Usuario>(API_URL, usuario);
    return response.data;
  }
  async login(telefone: string): Promise<Usuario | null> {
    const response = await axios.get<Usuario | null>(`${API_URL}/login`, {
      params: {
        telefone
      },
      withCredentials: true
    });
    return response.data;
  }
  async verificarSessao(): Promise<boolean> {
    try {
      const response = await axios.get<{ autenticado: boolean }>(
        `${API_URL}/verificar-sessao`,
        { withCredentials: true }
      );
      return response.data.autenticado;
    } catch {
      return false;
    }
  }
  async logout(): Promise<void> {
    await axios.post(`${API_URL}/logout`, {}, {
      withCredentials: true
    });
  }
  async listarUsuarios(): Promise<Usuario[]> {
    const response = await axios.get<Usuario[]>(API_URL);
    return response.data;
  }

  async listarUsuarioPorId(id: string): Promise<Usuario> {
    const response = await axios.get<Usuario>(`${API_URL}/${id}`);
    return response.data;
  }

  async atualizarUsuario(usuario: Usuario): Promise<Usuario> {
    const response = await axios.put<Usuario>(`${API_URL}/Atualizar`, usuario);
    return response.data;
  }

  async excluirUsuario(usuario: Usuario): Promise<Usuario> {
    const response = await axios.put<Usuario>(`${API_URL}/Deletar`, usuario);
    return response.data;
  }
}

export default new UsuarioService();