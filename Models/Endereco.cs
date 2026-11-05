using System.Text.RegularExpressions;

namespace SimulacaoBanco.Models;

public class Endereco
{
    public string Logradouro { get; private set; }
    public int Numero { get; private set; }
    public string Complemento { get; private set; }
    public string Bairro { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    public string Cep { get; private set; }
    public string Pais { get; private set; }
    public string Zona { get; private set; }

    private static readonly Regex CepRegex = new(@"^\d{5}-\d{3}$", RegexOptions.Compiled);

    public Endereco(string logradouro,
                    int numero,
                    string complemento,
                    string bairro,
                    string cidade,
                    string estado,
                    string cep,
                    string pais,
                    string zona)
    {
        if (string.IsNullOrWhiteSpace(logradouro))
            throw new ArgumentException("Logradouro é obrigatório.", nameof(logradouro));

        if (string.IsNullOrWhiteSpace(cidade))
            throw new ArgumentException("Cidade é obrigatória.", nameof(cidade));

        if (string.IsNullOrWhiteSpace(estado))
            throw new ArgumentException("Estado é obrigatório.", nameof(estado));

        if (string.IsNullOrWhiteSpace(cep))
            throw new ArgumentException("CEP é obrigatório.", nameof(cep));

        if (!CepRegex.IsMatch(cep))
            throw new ArgumentException($"CEP inválido '{cep}'. Formato esperado: 99999-999.", nameof(cep));

        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Cep = cep;
        Pais = pais;
        Zona = zona;
    }
}