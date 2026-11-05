using System;

namespace SimulacaoBanco.Models;

public class Cliente : IEquatable<Cliente>
{
    public int ID { get; }
    public string Nome { get; private set; }
    public DateOnly DataNascimento { get; private set; }
    public string Cpf { get; private set; }
    public Endereco Endereco { get; private set; }
    public string Telefone { get; private set; }

    public Cliente(
        int id,
        string nome,
        DateOnly dataNascimento,
        string cpf,
        Endereco endereco,
        string telefone)
    {
        if (id <= 0)
            throw new ArgumentException("ID inválido.");

        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome inválido.");

        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF inválido.");

        ID = id;
        Nome = nome;
        DataNascimento = dataNascimento;
        Cpf = cpf;
        Endereco = endereco ?? throw new ArgumentNullException(nameof(endereco));
        Telefone = telefone;
    }

    public bool Equals(Cliente? other)
    {
        if (other is null) return false;
        return ID == other.ID;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Cliente);
    }

    public override int GetHashCode()
    {
        return ID.GetHashCode();
    }

    public static bool operator ==(Cliente? left, Cliente? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) return false;
        return left.ID == right.ID;
    }

    public static bool operator !=(Cliente? left, Cliente? right)
    {
        return !(left == right);
    }
}