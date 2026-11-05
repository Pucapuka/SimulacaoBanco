namespace SimulacaoBanco.UI;

using SimulacaoBanco.Models;
using SimulacaoBanco.Services;
using System;
using System.Globalization;

public class Menu
{
    private readonly BancoService _banco;

    public Menu(BancoService banco)
    {
        _banco = banco;
    }

    // MENU PRINCIPAL

    public void Iniciar()
    {
        bool executando = true;

        while (executando)
        {
            Console.WriteLine("\n====================================");
            Console.WriteLine($"       {_banco.Nome.ToUpper()}");
            Console.WriteLine("====================================");
            Console.WriteLine("1. Cadastrar cliente");
            Console.WriteLine("2. Abrir conta bancária");
            Console.WriteLine("3. Acessar conta");
            Console.WriteLine("4. Listar clientes");
            Console.WriteLine("5. Listar contas");
            Console.WriteLine("0. Sair");
            Console.Write("\nEscolha uma opção: ");

            string? opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    CadastrarCliente();
                    break;

                case "2":
                    AbrirConta();
                    break;

                case "3":
                    AcessarConta();
                    break;

                case "4":
                    ListarClientes();
                    break;

                case "5":
                    ListarContas();
                    break;

                case "0":
                    executando = false;
                    Console.WriteLine("Obrigado por utilizar nossos serviços!");
                    break;

                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
    }

    // CADASTRO DE CLIENTE

    private void CadastrarCliente()
    {
        Console.WriteLine("\n========== CADASTRO DE CLIENTE ==========");

        Console.Write("Nome: ");
        string nome = Console.ReadLine() ?? "";

        Console.Write("CPF: ");
        string cpf = Console.ReadLine() ?? "";

        Console.Write("Data de nascimento (dd/MM/yyyy): ");
        DateOnly nascimento = LerDataNascimento();

        Console.WriteLine("\n========== ENDEREÇO ==========");

        Console.Write("Logradouro: ");
        string logradouro = Console.ReadLine() ?? "";

        Console.Write("Número: ");
        int numeroEndereco = LerNumero();

        Console.Write("Complemento: ");
        string complemento = Console.ReadLine() ?? "";

        Console.Write("Bairro: ");
        string bairro = Console.ReadLine() ?? "";

        Console.Write("Cidade: ");
        string cidade = Console.ReadLine() ?? "";

        Console.Write("Estado: ");
        string estado = Console.ReadLine() ?? "";

        Console.Write("CEP: ");
        string cep = Console.ReadLine() ?? "";

        Console.Write("País: ");
        string pais = Console.ReadLine() ?? "";

        Console.Write("Zona: ");
        string zona = Console.ReadLine() ?? "";

        Console.Write("Telefone: ");
        string telefone = Console.ReadLine() ?? "";

        try
        {
            Endereco endereco = new(
                logradouro,
                numeroEndereco,
                complemento,
                bairro,
                cidade,
                estado,
                cep,
                pais,
                zona
            );

            var resultado = _banco.CadastrarCliente(
                nome,
                nascimento,
                cpf,
                endereco,
                telefone
            );

            if (resultado.IsSuccess)
            {
                Console.WriteLine("\nCliente cadastrado com sucesso!");
                Console.WriteLine($"Nome: {resultado.Value.Nome}");
                Console.WriteLine($"Identificador: {resultado.Value.ID}");
            }
            else
            {
                Console.WriteLine($"Não foi possível cadastrar: {resultado.Error}");
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Erro de validação: {ex.Message}");
        }
    }

    // ABERTURA DE CONTA

    private void AbrirConta()
    {
        Console.WriteLine("\n========== ABERTURA DE CONTA ==========");

        if (_banco.ListarClientes().Count == 0)
        {
            Console.WriteLine("Não existem clientes cadastrados.");
            return;
        }

        ListarClientes();

        Console.Write("\nDigite o ID do cliente: ");

        int idCliente = LerNumero();

        Result<ContaBancaria> resultado = _banco.AbrirConta(idCliente);

        if (resultado.IsFailure)
        {
            Console.WriteLine($"Não foi possível abrir a conta: {resultado.Error}");
            return;
        }

        ContaBancaria conta = resultado.Value;

        Console.WriteLine("\nConta aberta com sucesso!");
        Console.WriteLine($"Titular: {conta.Cliente.Nome}");
        Console.WriteLine($"Número da conta: {conta.Numero}");
    }

    // ACESSO À CONTA

    private void AcessarConta()
    {
        Console.WriteLine("\n========== ACESSO À CONTA ==========");

        Console.Write("Digite o número da conta: ");
        int numero = LerNumero();

        Result<ContaBancaria> resultado = _banco.BuscarConta(numero);

        if (resultado.IsFailure)
        {
            Console.WriteLine(resultado.Error);
            return;
        }

        MenuConta(resultado.Value);
    }

    // MENU DE OPERAÇÕES DA CONTA

    private void MenuConta(ContaBancaria conta)
    {
        bool continuar = true;

        while (continuar)
        {
            Console.WriteLine("\n====================================");
            Console.WriteLine("           ÁREA DO CLIENTE");
            Console.WriteLine("====================================");
            Console.WriteLine($"Titular: {conta.Cliente.Nome}");
            Console.WriteLine($"Conta: {conta.Numero}");
            Console.WriteLine("------------------------------------");
            Console.WriteLine("1. Consultar saldo");
            Console.WriteLine("2. Depositar");
            Console.WriteLine("3. Sacar");
            Console.WriteLine("4. Transferir");
            Console.WriteLine("0. Voltar ao menu principal");

            Console.Write("\nEscolha uma opção: ");

            string? opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    Console.WriteLine($"Saldo atual: {conta.ConsultarSaldo():C}");
                    break;

                case "2":
                    Depositar(conta);
                    break;

                case "3":
                    Sacar(conta);
                    break;

                case "4":
                    Transferir(conta);
                    break;

                case "0":
                    continuar = false;
                    break;

                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
    }

    // DEPÓSITO

    private void Depositar(ContaBancaria conta)
    {
        Console.Write("\nValor do depósito (0 para voltar): ");

        decimal valor = LerValor();

        if (valor == 0)
            return;

        if (conta.Depositar(valor))
            Console.WriteLine("Depósito realizado com sucesso!");
        else
            Console.WriteLine("Não foi possível realizar o depósito.");
    }

    // SAQUE

    private void Sacar(ContaBancaria conta)
    {
        Console.Write("\nValor do saque (0 para voltar): ");

        decimal valor = LerValor();

        if (valor == 0)
            return;

        if (conta.Sacar(valor))
            Console.WriteLine("Saque realizado com sucesso!");
        else
            Console.WriteLine("Saldo insuficiente ou valor inválido.");
    }

    // TRANSFERÊNCIA

    private void Transferir(ContaBancaria origem)
    {
        Console.Write("\nNúmero da conta beneficiária (0 para voltar): ");
    
        int numero = LerNumero();
    
        if (numero == 0)
            return;
    
        Console.Write("\nValor da transferência (0 para voltar): ");
    
        decimal valor = LerValor();
    
        if (valor == 0)
            return;
    
        Result<bool> resultado = _banco.Transferir(
            origem.Numero,
            numero,
            valor
        );
    
        if (resultado.IsSuccess)
            Console.WriteLine("Transferência realizada com sucesso!");
        else
            Console.WriteLine($"Transferência não realizada: {resultado.Error}");
    }

    // LISTAGEM DE CLIENTES

    private void ListarClientes()
    {
        Console.WriteLine("\n========== CLIENTES CADASTRADOS ==========");

        var clientes = _banco.ListarClientes();

        if (clientes.Count == 0)
        {
            Console.WriteLine("Nenhum cliente cadastrado.");
            return;
        }

        foreach (var cliente in clientes)
        {
            Console.WriteLine(
                $"ID: {cliente.ID} | Nome: {cliente.Nome} | CPF: {cliente.Cpf}"
            );
        }
    }

    // LISTAGEM DE CONTAS

    private void ListarContas()
    {
        Console.WriteLine("\n========== CONTAS CADASTRADAS ==========");

        var contas = _banco.ListarContas();

        if (contas.Count == 0)
        {
            Console.WriteLine("Nenhuma conta cadastrada.");
            return;
        }

        foreach (var conta in contas)
        {
            Console.WriteLine(
                $"Conta: {conta.Numero} | Titular: {conta.Cliente.Nome} | Saldo: {conta.Saldo:C}"
            );
        }
    }

    // MÉTODOS AUXILIARES

    private static int LerNumero()
    {
        while (true)
        {
            string? entrada = Console.ReadLine();

            if (int.TryParse(entrada, out int numero) && numero >= 0)
                return numero;

            Console.Write("Número inválido. Digite novamente: ");
        }
    }

    private static decimal LerValor()
    {
        while (true)
        {
            string? entrada = Console.ReadLine();

            if (decimal.TryParse(
                entrada,
                NumberStyles.Number,
                CultureInfo.GetCultureInfo("pt-BR"),
                out decimal valor) && valor >= 0)
            {
                return valor;
            }

            Console.Write("Valor inválido. Digite novamente: ");
        }
    }

    private static DateOnly LerDataNascimento()
    {
        while (true)
        {
            string? entrada = Console.ReadLine();

            if (DateOnly.TryParseExact(
                entrada,
                "dd/MM/yyyy",
                CultureInfo.GetCultureInfo("pt-BR"),
                DateTimeStyles.None,
                out DateOnly data))
            {
                return data;
            }

            Console.Write("Data inválida. Digite novamente (dd/MM/yyyy): ");
        }
    }
}