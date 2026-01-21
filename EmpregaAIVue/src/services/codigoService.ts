import axios from 'axios'

const API_URL = 'https://localhost:7274/api/Auth'

export interface Usuario {
  id: string
  telefone: string
  ativo: boolean
  excluido: boolean
}

export interface EnviarCodigoRequest {
  telefone: string
}

export interface VerificarCodigoRequest {
  telefone: string
  codigo: string
}

export interface ApiMessageResponse {
  message: string
}

/**
 * Envia o código por SMS
 */
export async function enviarCodigo(
  telefone: string
): Promise<ApiMessageResponse> {
  const response = await axios.post<ApiMessageResponse>(
    `${API_URL}/send-code`,
    { telefone } as EnviarCodigoRequest
  )

  return response.data
}

/**
 * Verifica o código e AUTENTICA o usuário
 */
export async function verificarCodigo(
  telefone: string,
  codigo: string
): Promise<Usuario> {
  const response = await axios.post<Usuario>(
    `${API_URL}/verify-code`,
    {
      telefone,
      codigo,
    } as VerificarCodigoRequest
  )

  return response.data
}
