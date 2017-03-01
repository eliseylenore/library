using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
    public class BookTest : IDisposable
    {
        public BookTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_EqualOverride_true()
        {
            Book firstBook = new Book ("Gone with the Wind");
            Book secondBook = new Book ("Gone with the Wind");

            Assert.Equal(firstBook, secondBook);
        }

        [Fact]
        public void Save_SavesNewBook()
        {
            Book newBook = new Book ("To Kill A Mockingbird");
            newBook.Save();

            List<Book> expectedResult = new List<Book>{newBook};
            List<Book> actual = Book.GetAll();

            Console.WriteLine("ExpectedResult: " + expectedResult[0].GetTitle() + " Actual result: " + actual[0].GetTitle());
            Assert.Equal(expectedResult, actual);
        }

        [Fact]
        public void FindById_ReturnsBookWhenSearchedById()
        {
            Book newBook = new Book("To Kill a Mockingbird");
            newBook.Save();

            Book result = Book.FindById(newBook.GetId());

            Assert.Equal(newBook, result);
        }

        [Fact]
        public void Update_UpdatesBookName_Name()
        {
            Book newBook = new Book("To Kill a Mockingbird");
            newBook.Save();

            string newBookTitle = "Roger Rabbit";
            newBook.Update(newBookTitle);

            string actual = newBook.GetTitle();

            Assert.Equal(newBookTitle, actual);
        }

        public void Dispose()
        {
            Book.DeleteAll();
        }
    }
}
