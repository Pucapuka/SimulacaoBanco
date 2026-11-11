using SimulacaoBanco.Models;

namespace SimulacaoBanco.Tests.Models;

public class ContaBancariaTests
{
    private static Cliente CriarCliente(int id = 1)
    {
        var endereco = new Endereco(
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

        return new Cliente(
            id,
            "João",
            new DateOnly(1990, 5, 10),
            $"1234567890{id}",
            endereco,
            "999999999"
        );
    }

    private static ContaBancaria CriarConta(int numero = 1001)
    {
        var cliente = CriarCliente(numero);

        return new ContaBancaria(numero, cliente);
    }

    [Fact]
    public void Depositar_ValorPositivo_DeveAumentarSaldo()
    {
        // Arrange
        var conta = CriarConta();

        // Act
        bool resultado = conta.Depositar(500m);

        // Assert
        Assert.True(resultado);
        Assert.Equal(500m, conta.Saldo);
    }

    [Fact]
    public void Depositar_ValorNegativo_DeveRetornarFalse()
    {
        // Arrange
        var conta = CriarConta();

        // Act
        bool resultado = conta.Depositar(-500m);

        // Assert
        Assert.False(resultado);
        Assert.Equal(0m, conta.Saldo);
    }
    
    [Fact]
    public void Depositar_ValorZero_DeveRetornarFalse()
    {
        // Arrange
        var conta = CriarConta();

        // Act
        bool resultado = conta.Depositar(0m);

        // Assert
        Assert.False(resultado);
        Assert.Equal(0m, conta.Saldo);
    }

    [Fact]
    public void Sacar_ValorDisponivel_DeveDiminuirSaldo()
    {
        // Arrange
        var conta = CriarConta();
        conta.Depositar(500m);

        // Act
        bool resultado = conta.Sacar(200m);

        // Assert
        Assert.True(resultado);
        Assert.Equal(300m, conta.Saldo);
    }

    [Fact]
    public void Sacar_ValorSuperiorAoSaldo_DeveRetornarFalse()
    {
        // Arrange
        var conta = CriarConta();
        conta.Depositar(500m);

        // Act
        bool resultado = conta.Sacar(600m);

        // Assert
        Assert.False(resultado);
        Assert.Equal(500m, conta.Saldo);
    }

    [Fact]
    public void Transferir_ValorValido_DeveAtualizarOsDoisSaldos()
    {
        // Arrange
        var origem = CriarConta(1001);
        var destino = CriarConta(1002);

        origem.Depositar(500m);

        // Act
        bool resultado = origem.Transferir(destino, 200m);

        // Assert
        Assert.True(resultado);
        Assert.Equal(300m, origem.Saldo);
        Assert.Equal(200m, destino.Saldo);
    }

    [Fact]
    public void Transferir_ValorSuperiorAoSaldo_DeveRetornarFalse()
    {
        // Arrange
        var origem = CriarConta(1001);
        var destino = CriarConta(1002);

        origem.Depositar(100m);

        // Act
        bool resultado = origem.Transferir(destino, 200m);

        // Assert
        Assert.False(resultado);
        Assert.Equal(100m, origem.Saldo);
        Assert.Equal(0m, destino.Saldo);
    }

    [Fact]
    public void Transferir_MesmaConta_DeveRetornarFalse()
    {
        // Arrange
        var conta = CriarConta();
        conta.Depositar(500m);

        // Act
        bool resultado = conta.Transferir(conta, 100m);

        // Assert
        Assert.False(resultado);
        Assert.Equal(500m, conta.Saldo);
    }

    [Theory]

    [InlineData(-10)]
    [InlineData(-100)]
    [InlineData(-500)]
    [InlineData(-1000)]
    [InlineData(0)]
    public void Depositar_ValorInvalido_DeveRetornarFalse(decimal valor)
    {
        // Arrange
        var conta = CriarConta();

        // Act
        bool resultado = conta.Depositar(valor);

        // Assert
        Assert.False(resultado);
        Assert.Equal(0m, conta.Saldo);
    }

    [Theory]
    
    [InlineData(-10)]
    [InlineData(-100)]
    [InlineData(0)]
    [InlineData(600)]
    [InlineData(1000)]
    public void Sacar_ValorInvalido_DeveRetornarFalse(decimal valor)
    {
        // Arrange
        var conta = CriarConta();
        conta.Depositar(500m);

        // Act
        bool resultado = conta.Sacar(valor);

        // Assert
        Assert.False(resultado);
        Assert.Equal(500m, conta.Saldo);
    }

    [Theory]
    
    [InlineData(-100)]
    [InlineData(-10)]
    [InlineData(0)]
    [InlineData(600)]
    [InlineData(1000)]
    public void Transferir_ValorInvalido_DeveRetornarFalse(decimal valor)
    {
        // Arrange
        var origem = CriarConta(1001);
        var destino = CriarConta(1002);

        origem.Depositar(500m);

        // Act
        bool resultado = origem.Transferir(destino, valor);

        // Assert
        Assert.False(resultado);
        Assert.Equal(500m, origem.Saldo);
        Assert.Equal(0m, destino.Saldo);
    }

    [Fact]
    public void Transferir_ValorExato_DeveAtualizarOsDoisSaldos(){
        // Arrange
        var origem = CriarConta(1001);
        var destino = CriarConta(1002);

        origem.Depositar(500m);

        // Act
        bool resultado = origem.Transferir(destino, 500m);

        // Assert
        Assert.True(resultado);
        Assert.Equal(0m, origem.Saldo);
        Assert.Equal(500m, destino.Saldo);
    }

    [Fact]

    public void Transferir_Valor_ManterSoma(){
        // Arrange
        var origem = CriarConta(1001);
        var destino = CriarConta(1002);

        origem.Depositar(500m);
        destino.Depositar(100m);

        // Act
        bool resultado = origem.Transferir(destino, 200m);
        
        // Assert
        Assert.True(resultado);
        Assert.Equal(300m, origem.Saldo);
        Assert.Equal(300m, destino.Saldo);
        Assert.Equal(600m, origem.Saldo + destino.Saldo);
    }
}