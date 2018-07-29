using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService
{
    /// <summary>
    /// Classe serviço para ordernação dos livros
    /// </summary>
    public class BooksOrderer
    {
        private class RuleOrder
        {
            public string Atributo { get; set; }
            public bool Asc { get; set; }
        }

        private List<Book> _books;

        public IEnumerable<Book> Books { get { return _books; } }

        public BooksOrderer()
        {
            _books = new List<Book>();
        }

        public void AddBook(Book book)
        {
            _books.Add(book);
        }

        public void AddBook(string title, string author, int year)
        {
            _books.Add(new Book { Title = title, AuthorName = author, EditionYear = year });
        }

        /// <summary>
        /// Método que realiza a ordenação.
        /// </summary>
        /// <param name="rules">
        /// Lista das regras de ordenação. Cada item da lista deve ser separado por virgula, para indicar o atributo e a direção.
        /// Exemplo:
        /// <code>service.Order("titulo, asc", "autor, desc");</code>
        /// </param>
        /// <returns></returns>
        public IEnumerable<Book> Order(params string[] rules)
        {
            if(rules == null)
            {
                throw new OrderException("Regra não informada");
            }

            if(rules.Length == 0) // Regras não informada, retorna uma ordernação vazia
            {
                return new List<Book>();
            }

            if(_books.Count == 0)
            {
                throw new OrderException("Nenhuma livro informado");
            }

            var rulesProcessed = ProcessRule(rules);
            var rule = rulesProcessed[0];
            var propertyInfoOrder = typeof(Book).GetProperty(rule.Atributo);
            var sortedList = rule.Asc ? _books.OrderBy(i => propertyInfoOrder.GetValue(i, null)) : _books.OrderByDescending(i => propertyInfoOrder.GetValue(i, null));
            for (int i = 1; i < rulesProcessed.Count; i++)
            {
                rule = rulesProcessed[i];
                var propertyInfoThen = typeof(Book).GetProperty(rule.Atributo);
                sortedList = rule.Asc ? sortedList.ThenBy(u => propertyInfoThen.GetValue(u, null)) : sortedList.ThenByDescending(u => propertyInfoThen.GetValue(u, null));
            }
            return sortedList.ToList();
        }

        public void Clear()
        {
            _books.Clear();
        }

        private List<RuleOrder> ProcessRule(string[] rulesChar)
        {
            var rules = new List<RuleOrder>();
            foreach (string r in rulesChar)
            {
                var ruleChar = r.Split(',');
                var rule = new RuleOrder();

                // Direção
                switch(ruleChar[1].Trim().ToUpper())
                {
                    case "ASC":
                    case "ASCENDENTE":
                        rule.Asc = true;
                        break;
                    case "DESC":
                    case "DESCENDENTE":
                        rule.Asc = false;
                        break;
                    default:
                        throw new OrderException("Direção informada inválida");
                }

                // Atributo
                switch (ruleChar[0].Trim().ToUpper())
                {
                    case "TITLE":
                    case "TITULO":
                        rule.Atributo = Helper.GetPropertyName<Book, string>(b => b.Title);
                        break;
                    case "NOME":
                    case "AUTHOR":
                    case "AUTOR":
                        rule.Atributo = Helper.GetPropertyName<Book, string>(b => b.AuthorName);
                        break;
                    case "EDICAO":
                    case "EDITION":
                    case "YEAR":
                    case "ANO":
                        rule.Atributo = Helper.GetPropertyName<Book, int>(b => b.EditionYear);
                        break;
                    default:
                        throw new OrderException("Atributo informado inválido");
                }
                rules.Add(rule);
            }
            return rules;
        }
    }
}
