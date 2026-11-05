using SimulacaoBanco.Models;

namespace SimulacaoBanco.Services;

public class BancoService
{
    public string Nome { get; private set; }

    private readonly List<Cliente> _clientes = new();
    private readonly List<ContaBancaria> _contas = new();

    private int _proximoIdCliente = 1;
    private int _proximoNumeroConta = 1001;

    public BancoService(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome inválido.");

        Nome = nome;
    }

    // CADASTRO DE CLIENTES

    public Result<Cliente> CadastrarCliente(
        string nome,
        DateOnly dataNascimento,
        string cpf,
        Endereco endereco,
        string telefone)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return Result<Cliente>.Failure("CPF inválido.");

        if (_clientes.Any(c => c.Cpf == cpf))
            return Result<Cliente>.Failure("CPF já cadastrado.");

        try
        {
            Cliente cliente = new(
                _proximoIdCliente++,
                nome,
                dataNascimento,
                cpf,
                endereco,
                telefone
            );

            _clientes.Add(cliente);

            return Result<Cliente>.Success(cliente);
        }
        catch (ArgumentException ex)
        {
            return Result<Cliente>.Failure(ex.Message);
        }
    }

    // BUSCA DE CLIENTES

    public Result<Cliente> BuscarCliente(int id)
    {
        var cliente = _clientes.FirstOrDefault(c => c.ID == id);

        if (cliente is null)
            return Result<Cliente>.Failure($"Cliente com ID {id} não encontrado.");

        return Result<Cliente>.Success(cliente);
    }

    public IReadOnlyList<Cliente> ListarClientes()
    {
        return _clientes.AsReadOnly();
    }

    // ABERTURA DE CONTAS

    public Result<ContaBancaria> AbrirConta(int idCliente)
    {
        var clienteResult = BuscarCliente(idCliente);
        if (clienteResult.IsFailure)
            return Result<ContaBancaria>.Failure(clienteResult.Error);

        try
        {
            ContaBancaria conta = new(_proximoNumeroConta++, clienteResult.Value);
            _contas.Add(conta);
            return Result<ContaBancaria>.Success(conta);
        }
        catch (ArgumentException ex)
        {
            return Result<ContaBancaria>.Failure(ex.Message);
        }
    }

    // BUSCA DE CONTAS

    public Result<ContaBancaria> BuscarConta(int numero)
    {
        var conta = _contas.FirstOrDefault(c => c.Numero == numero);

        if (conta is null)
            return Result<ContaBancaria>.Failure($"Conta com número {numero} não encontrada.");

        return Result<ContaBancaria>.Success(conta);
    }

    public IReadOnlyList<ContaBancaria> ListarContas()
    {
        return _contas.AsReadOnly();
    }

    // TRANSFERÊNCIA

    public Result<bool> Transferir(
        int numeroOrigem,
        int numeroDestino,
        decimal valor)
    {
        var origemResult = BuscarConta(numeroOrigem);
        if (origemResult.IsFailure)
            return Result<bool>.Failure(origemResult.Error);

        var destinoResult = BuscarConta(numeroDestino);
        if (destinoResult.IsFailure)
            return Result<bool>.Failure(destinoResult.Error);

        try
        {
            if (!origemResult.Value.Transferir(destinoResult.Value, valor))
                return Result<bool>.Failure("Transferência não realizada. Verifique o saldo e os dados das contas.");

            return Result<bool>.Success(true);
        }
        catch (ArgumentException ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }
}