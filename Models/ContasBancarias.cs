namespace SimulacaoBanco.Models;

using System;
using System.Collections.Generic;

public class ContasBancarias
{
    HashSet<int> IdsList;
    
    public ContasBancarias()
    {
        IdsList = new HashSet<int>();
    }
}