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

        [Fact]
        public void Delete_DeletesBookFromDatabase()
        {
            Book newBook = new Book("Roger Rabbit");
            newBook.Save();

            newBook.Delete();

            List<Book> expectedResult = new List<Book>{};
            List<Book> actualResult = Book.GetAll();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void AddAuthor_AddsAuthorBookComboToAuthor_Book()
        {
            Book newBook = new Book("Roger Rabbit");
            newBook.Save();

            Author newAuthor = new Author("John Doe");
            newAuthor.Save();
            newBook.AddAuthor(newAuthor);

            List<Author> expected = new List<Author>{newAuthor};
            List<Author> result = newBook.GetAuthors();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetCopies_ShowsAllCopiesForSelectedBook_List()
        {
            Book newBook = new Book("Roger Rabbit");
            newBook.Save();

            Copy newCopy = new Copy(newBook.GetId());
            newCopy.Save();
            List<Copy> expectedResult = new List<Copy>{newCopy};

            Assert.Equal(expectedResult, newBook.GetCopies());
        }

        public void Dispose()
        {
            Author.DeleteAll();
            Book.DeleteAll();
        }
    }
}
