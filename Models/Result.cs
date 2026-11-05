using System;

namespace SimulacaoBanco.Models;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }
    public bool IsFailure => !IsSuccess;

    private Result(T value, string error)
    {
        IsSuccess = true;
        Value = value;
        Error = error;
    }

    private Result(string error)
    {
        IsSuccess = false;
        Value = default!;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, string.Empty);
    }

    public static Result<T> Failure(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
            throw new ArgumentException("Erro não pode ser nulo ou vazio.", nameof(error));

        return new Result<T>(error);
    }
}