using EmpregaAI.Models.Resume;
using System.Text.Json;
using System.Net.Http.Headers;
using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Models;
using DocumentFormat.OpenXml.Drawing;
using iText.Kernel.Pdf.Canvas.Wmf;

namespace EmpregaAI.Services
{

    public class ProcessadorGroqService : IProcessadorGroqService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _groqApiKey;
        private readonly string _groqModel;

        public ProcessadorGroqService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _groqApiKey = _configuration["Groq:ApiKey"]
                ?? throw new InvalidOperationException("Groq API Key não configurada");
            _groqModel = _configuration["Groq:Model"] ?? "llama-3.3-70b-versatile";
        }

        public async Task<CurriculoUpload> ProcessResumeTextAsync(string resumeText)
        {
            var prompt = BuildPrompt(resumeText);

            var requestBody = new
            {
                model = _groqModel,
                messages = new[]
                {
                    new { role = "system", content = GetSystemPrompt() },
                    new { role = "user", content = prompt }
                },
                temperature = 0.1,
                response_format = new { type = "json_object" }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.groq.com/openai/v1/chat/completions")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    System.Text.Encoding.UTF8,
                    "application/json"
                )
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _groqApiKey);

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Log para debug (opcional, pode remover depois)
            try
            {
                var logPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "groq_debug");
                Directory.CreateDirectory(logPath);
                File.WriteAllText(
                    System.IO.Path.Combine(logPath, $"response_{DateTime.Now:yyyyMMdd_HHmmss}.txt"),
                    responseContent
                );
            }
            catch { /* ignora erro de log */ }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro da Groq API: {response.StatusCode} - {responseContent}");
            }

            var groqResponse = JsonSerializer.Deserialize<GroqResponse>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            var jsonContent = groqResponse?.Choices?.FirstOrDefault()?.Message?.Content;

            if (string.IsNullOrEmpty(jsonContent))
            {
                throw new Exception("Resposta da IA vazia ou inválida");
            }

            // ===== LIMPEZA DO JSON =====
            // O content vem com \n, \r, \t que precisam ser removidos
            var cleanJson = jsonContent
                .Replace("\\n", "")
                .Replace("\\r", "")
                .Replace("\\t", "")
                .Trim();
            // ===========================

            try
            {
                var processedData = JsonSerializer.Deserialize<CurriculoUpload>(
                    cleanJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

                if (processedData == null)
                {
                    throw new Exception("Deserialização retornou null");
                }

                return processedData;
            }
            catch (JsonException ex)
            {
                throw new Exception($"Erro ao deserializar JSON: {ex.Message}");
            }
        }

        private string BuildPrompt(string resumeText)
        {
            return $@"Extraia as informações do seguinte currículo e retorne no formato JSON especificado:

{resumeText}

Lembre-se: retorne APENAS o JSON, sem texto adicional.";
        }


        private string GetSystemPrompt()
        {
            return @"Você é um assistente especializado em extrair informações estruturadas de currículos.
            Seu trabalho é analisar o texto fornecido e retornar APENAS um JSON válido com os dados extraídos.
            NÃO adicione explicações, comentários ou texto adicional. APENAS o JSON.

            O JSON deve seguir EXATAMENTE esta estrutura:
            {
              ""dadosPessoais"": {
                ""nomeCompleto"": ""string ou null"",
                ""telefone"": ""string ou null"",
                ""endereco"": ""string ou null"",
                ""cidade"": ""string ou null"",
                ""estado"": ""string ou null"",
                ""linkedIn"": ""string ou null"",
                ""gitHub"": ""string ou null""
              },
              ""experiencias"": [
                {
                  ""empresa"": ""string ou null"",
                  ""cargo"": ""string ou null"",
                  ""dataInicio"": ""YYYY-MM-DD ou null"",
                  ""dataFim"": ""YYYY-MM-DD ou null"",
                  ""empregoAtual"": true/false,
                  ""descricao"": ""string ou null""
                }
              ],
              ""formacoes"": [
                {
                  ""instituicao"": ""string ou null"",
                  ""curso"": ""string ou null"",
                  ""nivel"": ""Ensino Médio/Graduação/Pós-graduação/Mestrado/Doutorado ou null"",
                  ""status"": ""Concluído/Em andamento/Trancado ou null"",
                  ""dataInicio"": ""YYYY-MM-DD ou null"",
                  ""dataConclusao"": ""YYYY-MM-DD ou null""
                }
              ],
              ""certificacoes"": [
                {
                  ""nome"": ""string ou null"",
                  ""instituicao"": ""string ou null"",
                  ""dataConclusao"": ""YYYY-MM-DD ou null"",
                  ""cargaHoraria"": number ou null,
                  ""urlCertificado"": ""string ou null""
                }
              ]
            }

            REGRAS IMPORTANTES:
            1. Datas devem estar no formato ISO (YYYY-MM-DD)
            2. Se não encontrar uma informação, use null
            3. empregoAtual deve ser true se o texto indicar que ainda trabalha lá
            4. Arrays vazios são permitidos se não houver dados
            5. NUNCA invente dados que não estão no currículo
            6. Para descrições de experiências com múltiplos itens, separe cada item com \n e remova símbolos de bullet";
        }

        private class GroqResponse
        {
            public List<Choice>? Choices { get; set; }
        }

        private class Choice
        {
            public Message? Message { get; set; }
        }

        private class Message
        {
            public string? Content { get; set; }
        }
    }
}