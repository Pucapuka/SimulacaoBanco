using System;

namespace SimulacaoBanco.Models;

public class ClienteNaoEncontradoException : Exception
{
    public int Id { get; }

    public ClienteNaoEncontradoException(int id)
        : base($"Cliente com ID {id} não foi encontrado.")
    {
        Id = id;
    }
}

public class ContaNaoEncontradaException : Exception
{
    public int Numero { get; }

    public ContaNaoEncontradaException(int numero)
        : base($"Conta com número {numero} não foi encontrada.")
    {
        Numero = numero;
    }
}

public class SaldoInsuficienteException : Exception
{
    public decimal SaldoAtual { get; }

    public SaldoInsuficienteException(decimal saldoAtual)
        : base($"Saldo insuficiente. Saldo atual: {saldoAtual:C}.")
    {
        SaldoAtual = saldoAtual;
    }
}

public class TransferenciaNaoPermitidaException : Exception
{
    public decimal ValorSolicitado { get; }

    public TransferenciaNaoPermitidaException(decimal valorSolicitado)
        : base($"Transferência não permitida. Valor solicitado: {valorSolicitado:C}.")
    {
        ValorSolicitado = valorSolicitado;
    }
}