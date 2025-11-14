// src/services/usuarioService.ts
import axios from 'axios';
import type { Usuario } from 'src/models/Usuario';

const API_URL = 'https://localhost:44388/api/Usuario';

class UsuarioService {
  async adicionarUsuario(usuario: Omit<Usuario, 'id' | 'ativo' | 'excluido'>): Promise<Usuario> {
    const response = await axios.post<Usuario>(API_URL, usuario);
    return response.data;
  }
  async login(email: string, senha: string): Promise<Usuario | null> {
    const response = await axios.get<Usuario | null>(`${API_URL}/login`, {
      params: {
        email,
        senha
      }
    });
    return response.data;
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