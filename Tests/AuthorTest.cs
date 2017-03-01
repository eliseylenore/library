using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
    public class AuthorTest : IDisposable
    {
        public AuthorTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_EqualOverride_true()
        {
            Author firstAuthor = new Author ("Gone with the Wind");
            Author secondAuthor = new Author ("Gone with the Wind");

            Assert.Equal(firstAuthor, secondAuthor);
        }

        [Fact]
        public void Save_SavesNewAuthor()
        {
            Author newAuthor = new Author ("To Kill A Mockingbird");
            newAuthor.Save();

            List<Author> expectedResult = new List<Author>{newAuthor};
            List<Author> actual = Author.GetAll();

            Console.WriteLine("ExpectedResult: " + expectedResult[0].GetName() + " Actual result: " + actual[0].GetName());
            Assert.Equal(expectedResult, actual);
        }

        [Fact]
        public void FindById_ReturnsAuthorWhenSearchedById()
        {
            Author newAuthor = new Author("To Kill a Mockingbird");
            newAuthor.Save();

            Author result = Author.FindById(newAuthor.GetId());

            Assert.Equal(newAuthor, result);
        }

        [Fact]
        public void Update_UpdatesAuthorName_Name()
        {
            Author newAuthor = new Author("To Kill a Mockingbird");
            newAuthor.Save();

            string newAuthorName = "Roger Rabbit";
            newAuthor.Update(newAuthorName);

            string actual = newAuthor.GetName();

            Assert.Equal(newAuthorName, actual);
        }

        [Fact]
        public void Delete_DeletesAuthorFromDatabase()
        {
            Author newAuthor = new Author("Roger Rabbit");
            newAuthor.Save();

            newAuthor.Delete();

            List<Author> expectedResult = new List<Author>{};
            List<Author> actualResult = Author.GetAll();

            Assert.Equal(expectedResult, actualResult);
        }

        public void Dispose()
        {
            Book.DeleteAll();
            Author.DeleteAll();
        }
    }
}
