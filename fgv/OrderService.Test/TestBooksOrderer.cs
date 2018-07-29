using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrderService.Test
{
    [TestClass]
    public class TestBooksOrderer
    {
        BooksOrderer service = new BooksOrderer();
        Book livro1;
        Book livro2;
        Book livro3;
        Book livro4;

        [TestInitialize]
        public void TestInitialize()
        {
            livro1 = new Book { Title = "Java How to Program", AuthorName = "Deitel & Deitel", EditionYear = 2007 };
            livro2 = new Book { Title = "Patterns of Enterprise Application Architecture", AuthorName = "Martin Fowler", EditionYear = 2002 };
            livro3 = new Book { Title = "Head First Design Patterns", AuthorName = "Elisabeth Freeman", EditionYear = 2004 };
            livro4 = new Book { Title = "Internet & World Wide Web: How to Program", AuthorName = "Deitel & Deitel", EditionYear = 2007 };

            service.AddBook(livro1);
            service.AddBook(livro2);
            service.AddBook(livro3);
            service.AddBook(livro4);
        }

        [TestMethod]
        public void Test1()
        {
            List<Book> expected = new List<Book>
            {
                livro3,
                livro4,
                livro1,
                livro2
            };

            var actual = new List<Book>(service.Order("title, asc"));

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test2()
        {
            List<Book> expected = new List<Book>
            {
                livro1,
                livro4,
                livro3,
                livro2
            };

            var actual = new List<Book>(service.Order("autor, asc", "title, desc"));

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test3()
        {
            List<Book> expected = new List<Book>
            {
                livro4,
                livro1,
                livro3,
                livro2
            };

            var actual = new List<Book>(service.Order("edicao, desc", "autor, desc", "titulo, asc"));

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(OrderException), "Regra nula.")]
        public void Test4()
        {
            service.Order(null);
        }

        [TestMethod]
        public void Test5()
        {
            List<Book> expected = new List<Book>();

            var actual = new List<Book>(service.Order());

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
