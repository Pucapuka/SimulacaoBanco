using SimulacaoBanco.Models;
using SimulacaoBanco.Services;

namespace SimulacaoBanco.Tests.Services;

public class BancoServiceTests
{
    private static Endereco CriarEndereco()
    {
        return new Endereco(
            "Rua A",
            100,
            "Casa",
            "Centro",
            "Imperatriz",
            "MA",
            "65900-000",
            "Brasil",
            "Urbana"
        );
    }

    private static BancoService CriarBanco()
    {
        return new BancoService("Banco Teste");
    }

    private static Result<Cliente> CadastrarClienteValido(
        BancoService banco,
        string nome = "João",
        string cpf = "12345678901")
    {
        return banco.CadastrarCliente(
            nome,
            new DateOnly(1990, 5, 10),
            cpf,
            CriarEndereco(),
            "999999999"
        );
    }

    [Fact]
    public void Construtor_NomeVazio_DeveLancarArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new BancoService(""));
        Assert.Throws<ArgumentException>(() => new BancoService("   "));
    }

    [Fact]
    public void CadastrarCliente_DadosValidos_DeveRetornarSucesso()
    {
        var banco = CriarBanco();

        var resultado = CadastrarClienteValido(banco);

        Assert.True(resultado.IsSuccess);
        Assert.Equal("João", resultado.Value.Nome);
        Assert.Equal("12345678901", resultado.Value.Cpf);
        Assert.Equal(1, resultado.Value.ID);
        Assert.Single(banco.ListarClientes());
    }

    [Fact]
    public void CadastrarCliente_CpfVazio_DeveRetornarFalha()
    {
        var banco = CriarBanco();

        var resultado = banco.CadastrarCliente(
            "João",
            new DateOnly(1990, 5, 10),
            "   ",
            CriarEndereco(),
            "999999999"
        );

        Assert.True(resultado.IsFailure);
        Assert.Equal("CPF inválido.", resultado.Error);
        Assert.Empty(banco.ListarClientes());
    }

    [Fact]
    public void CadastrarCliente_CpfDuplicado_DeveRetornarFalha()
    {
        var banco = CriarBanco();
        CadastrarClienteValido(banco, cpf: "12345678901");

        var resultado = CadastrarClienteValido(banco, nome: "Maria", cpf: "12345678901");

        Assert.True(resultado.IsFailure);
        Assert.Equal("CPF já cadastrado.", resultado.Error);
        Assert.Single(banco.ListarClientes());
    }

    [Fact]
    public void CadastrarCliente_NomeInvalido_DeveRetornarFalha()
    {
        var banco = CriarBanco();

        var resultado = banco.CadastrarCliente(
            "",
            new DateOnly(1990, 5, 10),
            "12345678901",
            CriarEndereco(),
            "999999999"
        );

        Assert.True(resultado.IsFailure);
        Assert.Equal("Nome inválido.", resultado.Error);
        Assert.Empty(banco.ListarClientes());
    }

    [Fact]
    public void BuscarCliente_IdExistente_DeveRetornarCliente()
    {
        var banco = CriarBanco();
        var cadastro = CadastrarClienteValido(banco);

        var resultado = banco.BuscarCliente(cadastro.Value.ID);

        Assert.True(resultado.IsSuccess);
        var clienteEncontrado = resultado.Value;
        Assert.Equal(cadastro.Value.ID, clienteEncontrado.ID);
    }

    [Fact]
    public void BuscarCliente_IdInexistente_DeveRetornarFalha()
    {
        var banco = CriarBanco();

        var resultado = banco.BuscarCliente(99);

        Assert.True(resultado.IsFailure);
        Assert.Equal("Cliente com ID 99 não encontrado.", resultado.Error);
    }

    [Fact]
    public void AbrirConta_ClienteExistente_DeveCriarConta()
    {
        var banco = CriarBanco();
        var cliente = CadastrarClienteValido(banco);

        var resultado = banco.AbrirConta(cliente.Value.ID);

        Assert.True(resultado.IsSuccess);
        Assert.Equal(1001, resultado.Value.Numero);
        Assert.Equal(cliente.Value.ID, resultado.Value.Cliente.ID);
        Assert.Equal(0m, resultado.Value.Saldo);
        Assert.Single(banco.ListarContas());
    }

    [Fact]
    public void AbrirConta_ClienteInexistente_DeveRetornarFalha()
    {
        var banco = CriarBanco();

        var resultado = banco.AbrirConta(99);

        Assert.True(resultado.IsFailure);
        Assert.Equal("Cliente com ID 99 não encontrado.", resultado.Error);
        Assert.Empty(banco.ListarContas());
    }

    [Fact]
    public void BuscarConta_NumeroExistente_DeveRetornarConta()
    {
        var banco = CriarBanco();
        var cliente = CadastrarClienteValido(banco);
        var conta = banco.AbrirConta(cliente.Value.ID);

        var resultado = banco.BuscarConta(conta.Value.Numero);

        Assert.True(resultado.IsSuccess);
        Assert.Equal(conta.Value.Numero, resultado.Value.Numero);
    }

    [Fact]
    public void BuscarConta_NumeroInexistente_DeveRetornarFalha()
    {
        var banco = CriarBanco();

        var resultado = banco.BuscarConta(9999);

        Assert.True(resultado.IsFailure);
        Assert.Equal("Conta com número 9999 não encontrada.", resultado.Error);
    }

    [Fact]
    public void Transferir_ValorValido_DeveAtualizarOsDoisSaldos()
    {
        var banco = CriarBanco();
        var origemCliente = CadastrarClienteValido(banco, cpf: "11111111111");
        var destinoCliente = CadastrarClienteValido(banco, nome: "Maria", cpf: "22222222222");

        var origem = banco.AbrirConta(origemCliente.Value.ID);
        var destino = banco.AbrirConta(destinoCliente.Value.ID);
        origem.Value.Depositar(500m);

        var resultado = banco.Transferir(origem.Value.Numero, destino.Value.Numero, 200m);

        Assert.True(resultado.IsSuccess);
        Assert.Equal(300m, origem.Value.Saldo);
        Assert.Equal(200m, destino.Value.Saldo);
    }

    [Fact]
    public void Transferir_ContaOrigemInexistente_DeveRetornarFalha()
    {
        var banco = CriarBanco();
        var cliente = CadastrarClienteValido(banco);
        var destino = banco.AbrirConta(cliente.Value.ID);

        var resultado = banco.Transferir(9999, destino.Value.Numero, 100m);

        Assert.True(resultado.IsFailure);
        Assert.Equal("Conta com número 9999 não encontrada.", resultado.Error);
    }

    [Fact]
    public void Transferir_SaldoInsuficiente_DeveRetornarFalha()
    {
        var banco = CriarBanco();
        var origemCliente = CadastrarClienteValido(banco, cpf: "11111111111");
        var destinoCliente = CadastrarClienteValido(banco, nome: "Maria", cpf: "22222222222");

        var origem = banco.AbrirConta(origemCliente.Value.ID);
        var destino = banco.AbrirConta(destinoCliente.Value.ID);
        origem.Value.Depositar(50m);

        var resultado = banco.Transferir(origem.Value.Numero, destino.Value.Numero, 100m);

        Assert.True(resultado.IsFailure);
        Assert.Equal("Transferência não realizada. Verifique o saldo e os dados das contas.", resultado.Error);
        Assert.Equal(50m, origem.Value.Saldo);
        Assert.Equal(0m, destino.Value.Saldo);
    }
    [Fact]
    public void Transferir_ContaDestinoInexistente_DeveRetornarFalha()
    {
        //Arrange
        var banco = CriarBanco();
        var origemCliente = CadastrarClienteValido(banco, cpf: "11111111111");
        var origem = banco.AbrirConta(origemCliente.Value.ID);

        //Act
        var resultado = banco.Transferir(origem.Value.Numero, 9999, 100m);

        //Assert
        Assert.True(resultado.IsFailure);
        Assert.Equal("Conta com número 9999 não encontrada.", resultado.Error);
    }

    [Fact]
    public void Transferir_ValorNegativo_DeveRetornarFalha()
    {
        //Arrange
        var banco = CriarBanco();
        var origemCliente = CadastrarClienteValido(banco, cpf: "11111111111");
        var destinoCliente = CadastrarClienteValido(banco, cpf: "22222222222");
        
        var origem = banco.AbrirConta(origemCliente.Value.ID);
        var destino = banco.AbrirConta(destinoCliente.Value.ID);

        origem.Value.Depositar(500m);
        
        //Act
        var resultado = banco.Transferir(origem.Value.Numero, destino.Value.Numero, -100m);
        
        //Assert
        Assert.True(resultado.IsFailure);
        Assert.Equal("Transferência não realizada. Verifique o saldo e os dados das contas.", resultado.Error);
        Assert.Equal(500m, origem.Value.Saldo);
        Assert.Equal(0m, destino.Value.Saldo);
    }

    [Fact]
    public void Transferir_MesmaConta_DeveRetornarFalha()
    {
        //Arrange
        var banco = CriarBanco();
        var cliente = CadastrarClienteValido(banco, cpf: "22222222222");
        var conta = banco.AbrirConta(cliente.Value.ID);

        conta.Value.Depositar(500m);

        //Act
        var resultado = banco.Transferir(conta.Value.Numero, conta.Value.Numero, 100m);

        //Assert
        Assert.True(resultado.IsFailure);
        Assert.Equal(500m, conta.Value.Saldo);
    }
}
