using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Client
{
    class Program
    {
        static BooksOrderer service = new BooksOrderer();

        static void Main(string[] args)
        {
            char option = '0';
            bool nao_sair = true;
            while(nao_sair)
            {
                switch (option)
                {
                    case '0': // Inicio
                        Console.WriteLine("Informe as seguintes opções");
                        Console.WriteLine("1 - Adicionar livro");
                        Console.WriteLine("2 - Ordernar livros");
                        Console.WriteLine("3 - Limpar livros");
                        Console.WriteLine("4 - Sair");
                        option = Console.ReadKey(true).KeyChar;
                        Console.Clear();
                        break;
                    case '1': // Adicionar livro
                        option = '0';
                        Console.Write("Informe o titulo: ");
                        string titulo = Console.ReadLine();
                        Console.Write("Informe o autor: ");
                        string autor = Console.ReadLine();
                        Console.Write("Informe o ano: ");
                        string anoChar = Console.ReadLine();
                        int ano;
                        if(!Int32.TryParse(anoChar, out ano))
                        {
                            Console.Write("Somente números no ano");
                            break;
                        }
                        service.AddBook(titulo, autor, ano);
                        Console.Write("Livro adicionado");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    case '2': // Ordenar
                        option = '0';
                        try
                        {
                            string rules = ConfigurationManager.AppSettings["rules"];
                            if(String.IsNullOrEmpty(rules))
                            {
                                Console.Write("Configuração da regra de classificação não encontrada");
                                break;
                            }
                            var rulesSplit = rules.Split('|');
                            var ordered = service.Order(rulesSplit);
                            int i = 1;
                            foreach(var item in ordered)
                            {
                                Console.WriteLine($"{i++}°");
                                Console.WriteLine($"Titulo: {item.Title}");
                                Console.WriteLine($"Autor: {item.AuthorName}");
                                Console.WriteLine($"Edição: {item.EditionYear}");
                            }
                            Console.WriteLine();
                            Console.WriteLine("Pressione qualquer tecla para continuar");
                            Console.ReadKey(true);
                        }
                        catch(OrderException ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                            Console.WriteLine("Pressione qualquer tecla para continuar");
                            Console.ReadKey(true);
                        }
                        Console.Clear();
                        break;
                    case '3': // Limpar livros
                        option = '0';
                        service.Clear();
                        Console.Clear();
                        break;
                    case '4': // Sair
                        nao_sair = false;
                        break;
                    default:
                        option = '0';
                        Console.WriteLine("Opção inválida");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                }
            }
        }
    }
}
