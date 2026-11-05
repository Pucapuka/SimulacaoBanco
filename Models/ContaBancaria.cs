using System;

namespace SimulacaoBanco.Models;

public class ContaBancaria : IEquatable<ContaBancaria>
{
    public int Numero { get; private set; }
    public Cliente Cliente { get; }
    public decimal Saldo { get; private set; }

    public ContaBancaria(int numero, Cliente cliente)
    {
        if (numero <= 0)
            throw new ArgumentException("Número da conta inválido.");

        Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));

        Numero = numero;
        Saldo = 0;
    }

    public bool Equals(ContaBancaria? other)
    {
        if (other is null) return false;
        return Numero == other.Numero;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ContaBancaria);
    }

    public override int GetHashCode()
    {
        return Numero.GetHashCode();
    }

    public static bool operator ==(ContaBancaria? left, ContaBancaria? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) return false;
        return left.Numero == right.Numero;
    }

    public static bool operator !=(ContaBancaria? left, ContaBancaria? right)
    {
        return !(left == right);
    }

    public bool Depositar(decimal valor)
    {
        if (valor <= 0)
            return false;

        Saldo += valor;

        return true;
    }

    public bool Sacar(decimal valor)
    {
        if (valor <= 0 || valor > Saldo)
            return false;

        Saldo -= valor;

        return true;
    }

    public bool Transferir(ContaBancaria beneficiario, decimal valor)
    {
        if (beneficiario is null)
            return false;

        if (beneficiario == this)
            return false;

        if (valor <= 0 || valor > Saldo)
            return false;

        Saldo -= valor;
        beneficiario.Saldo += valor;

        return true;
    }

    public decimal ConsultarSaldo()
    {
        return this.Saldo;
    }
}