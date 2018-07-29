using AllanSerraVasconcellos;
using System;

namespace AvaliacaoTecnicaDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            string original = "28/02/2010 23:10";

            long removeMinutos = 4000;
            long adicionarMinutos = 4000;
            //cwi.AddMinuto(4000);
            //string changedate = cwi.ChangeDate("01/03/2010 23:00", '+', 4000);
            Console.WriteLine($"Original Date: {original}");

            Console.WriteLine("CWI");
            Console.WriteLine($"Changed Date - Adiciona minutos: {CWIHelper.ChangeDate(original, '+', adicionarMinutos)}");
            Console.WriteLine($"Changed Date - Remover minutos: {CWIHelper.ChangeDate(original, '-', removeMinutos)}");

            Console.WriteLine(".NET");
            DateTime date = DateTime.Parse(original);
            Console.WriteLine($"Changed Date - Adicionar minutos: {date.AddMinutes(adicionarMinutos)}");
            Console.WriteLine($"Changed Date - Remover minutos: {date.AddMinutes(removeMinutos * -1)}");
            Console.ReadKey();
        }
    }
}
