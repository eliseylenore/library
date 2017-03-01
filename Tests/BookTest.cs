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


        public void Dispose()
        {
            Book.DeleteAll();
        }
    }
}
