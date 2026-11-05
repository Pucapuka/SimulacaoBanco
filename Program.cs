using SimulacaoBanco.Services;
using SimulacaoBanco.UI;

BancoService banco = new("Banco do Brasil");

Menu menu = new(banco);

menu.Iniciar();