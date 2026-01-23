import api from '../api'

const ROUTE = '/Auth';

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
  const response = await api.post<ApiMessageResponse>(
    `${ROUTE}/send-code`,
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
  const response = await api.post<Usuario>(
    `${ROUTE}/verify-code`,
    {
      telefone,
      codigo,
    } as VerificarCodigoRequest
  )

  return response.data
}
