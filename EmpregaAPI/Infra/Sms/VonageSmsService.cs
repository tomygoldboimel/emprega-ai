using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Vonage;
using Vonage.Request;
using Vonage.Messaging;
using System.Text.RegularExpressions;

public class VonageSmsService
{
    private readonly VonageClient _client;
    private readonly VonageSettings _settings;
    private readonly IMemoryCache _cache;

    private const int TEMPO_EXPIRACAO_MINUTOS = 5;

    public VonageSmsService(
        IOptions<VonageSettings> options,
        IMemoryCache cache)
    {
        _settings = options.Value;
        _cache = cache;

        var credentials = Credentials.FromApiKeyAndSecret(
            _settings.ApiKey,
            _settings.ApiSecret
        );

        _client = new VonageClient(credentials);
    }

    public async Task EnviarCodigoAsync(string telefone)
    {
        var telefoneFormatado = FormatarTelefone(telefone);
        var codigo = GerarCodigo(6);

        _cache.Set(
            GerarChaveCache(telefone),
            codigo,
            TimeSpan.FromMinutes(TEMPO_EXPIRACAO_MINUTOS)
        );

        var mensagem = $"Codigo de verificacao EmpregaAI: {codigo}. Expira em {TEMPO_EXPIRACAO_MINUTOS} minutos."; ;

        await _client.SmsClient.SendAnSmsAsync(new SendSmsRequest
        {
            To = telefoneFormatado,
            From = _settings.From,
            Text = mensagem,
            Type = SmsType.Text
        });
    }

    public bool VerificarCodigo(string telefone, string codigoInformado)
    {
        if (!_cache.TryGetValue(GerarChaveCache(telefone), out string? codigoSalvo))
            return false;

        if (codigoSalvo != codigoInformado)
            return false;

        _cache.Remove(GerarChaveCache(telefone));
        return true;
    }

    private string GerarCodigo(int tamanho)
    {
        var random = new Random();
        return string.Concat(
            Enumerable.Range(0, tamanho)
                      .Select(_ => random.Next(0, 10))
        );
    }
    private string FormatarTelefone(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone)) return string.Empty;
        string apenasNumeros = Regex.Replace(telefone, @"\D", "");

        return $"55{apenasNumeros}";
    }
    private string GerarChaveCache(string telefone)
        => $"sms_codigo_{telefone}";
}
