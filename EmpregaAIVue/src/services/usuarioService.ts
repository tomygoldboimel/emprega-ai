import api from '../api';
import type { Usuario } from 'src/models/Usuario';

const ROUTE = '/Usuario';

class UsuarioService {
  async adicionarUsuario(usuario: Omit<Usuario, 'id' | 'ativo' | 'excluido'>): Promise<Usuario> {
    const response = await api.post<Usuario>(ROUTE, usuario);
    return response.data;
  }
  async login(telefone: string): Promise<Usuario | null> {
    const response = await api.get<Usuario | null>(`${ROUTE}/login`, {
      params: {
        telefone
      },
      withCredentials: true
    });
    return response.data;
  }
  async verificarSessao(): Promise<boolean> {
    try {
      const response = await api.get<{ autenticado: boolean }>(
        `${ROUTE}/verificar-sessao`,
        { withCredentials: true }
      );
      return response.data.autenticado;
    } catch {
      return false;
    }
  }
  async logout(): Promise<void> {
    await api.post(`${ROUTE}/logout`, {}, {
      withCredentials: true
    });
  }
  async listarUsuarios(): Promise<Usuario[]> {
    const response = await api.get<Usuario[]>(ROUTE);
    return response.data;
  }

  async listarUsuarioPorId(id: string): Promise<Usuario> {
    const response = await api.get<Usuario>(`${ROUTE}/${id}`);
    return response.data;
  }

  async atualizarUsuario(usuario: Usuario): Promise<Usuario> {
    const response = await api.put<Usuario>(`${ROUTE}/Atualizar`, usuario);
    return response.data;
  }

  async excluirUsuario(usuario: Usuario): Promise<Usuario> {
    const response = await api.put<Usuario>(`${ROUTE}/Deletar`, usuario);
    return response.data;
  }
}

export default new UsuarioService();