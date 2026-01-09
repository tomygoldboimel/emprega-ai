using System.Text.Json.Serialization;

namespace EmpregaAI.Models.Resume
{
    public class CurriculoUpload
    {
        [JsonPropertyName("dadosPessoais")]
        public DadosPessoaisProcessados? DadosPessoais { get; set; }

        [JsonPropertyName("experiencias")]
        public List<ExperienciaProcessada>? Experiencias { get; set; }

        [JsonPropertyName("formacoes")]
        public List<FormacaoProcessada>? Formacoes { get; set; }

        [JsonPropertyName("certificacoes")]
        public List<CertificacaoProcessada>? Certificacoes { get; set; }
    }

    public class DadosPessoaisProcessados
    {
        [JsonPropertyName("nomeCompleto")]
        public string? NomeCompleto { get; set; }

        [JsonPropertyName("dataNascimento")]
        public string? DataNascimento { get; set; }

        [JsonPropertyName("telefone")]
        public string? Telefone { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("endereco")]
        public string? Endereco { get; set; }

        [JsonPropertyName("cidade")]
        public string? Cidade { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }

        [JsonPropertyName("linkedIn")]
        public string? LinkedIn { get; set; }

        [JsonPropertyName("gitHub")]
        public string? GitHub { get; set; }
    }

    public class ExperienciaProcessada
    {
        [JsonPropertyName("empresa")]
        public string? Empresa { get; set; }

        [JsonPropertyName("cargo")]
        public string? Cargo { get; set; }

        [JsonPropertyName("dataInicio")]
        public string? DataInicio { get; set; }

        [JsonPropertyName("dataFim")]
        public string? DataFim { get; set; }

        [JsonPropertyName("empregoAtual")]
        public bool EmpregoAtual { get; set; }

        [JsonPropertyName("descricao")]
        public string? Descricao { get; set; }
    }

    public class FormacaoProcessada
    {
        [JsonPropertyName("instituicao")]
        public string? Instituicao { get; set; }

        [JsonPropertyName("curso")]
        public string? Curso { get; set; }

        [JsonPropertyName("nivel")]
        public string? Nivel { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("dataInicio")]
        public string? DataInicio { get; set; }

        [JsonPropertyName("dataConclusao")]
        public string? DataConclusao { get; set; }
    }

    public class CertificacaoProcessada
    {
        [JsonPropertyName("nome")]
        public string? Nome { get; set; }

        [JsonPropertyName("instituicao")]
        public string? Instituicao { get; set; }

        [JsonPropertyName("dataConclusao")]
        public string? DataConclusao { get; set; }

        [JsonPropertyName("cargaHoraria")]
        public int? CargaHoraria { get; set; }

        [JsonPropertyName("urlCertificado")]
        public string? UrlCertificado { get; set; }
    }
}