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


        public void Dispose()
        {
            Book.DeleteAll();
        }
    }
}
