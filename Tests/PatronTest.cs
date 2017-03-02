using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
    public class PatronTest : IDisposable
    {
        public PatronTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_EqualOverride_true()
        {
            Patron firstPatron = new Patron ("Joe");
            Patron secondPatron = new Patron ("Joe");

            Assert.Equal(firstPatron, secondPatron);
        }

        [Fact]
        public void Save_SavesNewPatron()
        {
            Patron newPatron = new Patron ("Joe");
            newPatron.Save();

            List<Patron> expectedResult = new List<Patron>{newPatron};
            List<Patron> actual = Patron.GetAll();


            Assert.Equal(expectedResult, actual);
        }

        [Fact]
        public void FindById_ReturnsPatronWhenSearchedById()
        {
            Patron newPatron = new Patron("Joe");
            newPatron.Save();

            Patron result = Patron.FindById(newPatron.GetId());

            Assert.Equal(newPatron, result);
        }

        [Fact]
        public void Update_UpdatesPatronName_Name()
        {
            Patron newPatron = new Patron("Joe");
            newPatron.Save();

            string newPatronName = "John";
            newPatron.Update(newPatronName);

            string actual = newPatron.GetName();

            Assert.Equal(newPatronName, actual);
        }

        [Fact]
        public void Delete_DeletesPatronFromDatabase()
        {
            Patron newPatron = new Patron("Joe");
            newPatron.Save();

            newPatron.Delete();

            List<Patron> expectedResult = new List<Patron>{};
            List<Patron> actualResult = Patron.GetAll();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetCheckedOutBooks_ReturnListOfBooks()
        {
            Patron newPatron = new Patron("Joe");
            newPatron.Save();

            Book newBook = new Book("Cotton Eyed Joe");
            newBook.Save();

            Copy newCopy = new Copy(newBook.GetId());
            newCopy.Save();

            newPatron.Checkout(newCopy.GetId(), "June 12", "Aug 14");

            List<Book> expectedResult = new List<Book>{newBook};
            List<Book> actualResult = newPatron.GetCheckedOutBooks();

            Assert.Equal(expectedResult, actualResult);
        }

        public void Dispose()
        {
            Patron.DeleteAll();
        }
    }
}
