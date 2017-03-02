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
        public void GetCheckedOutCopies_ReturnListOfCopies()
        {
            Patron newPatron = new Patron("Joe");
            newPatron.Save();

            Book newBook = new Book("Gone With the Wind");
            newBook.Save();

            Copy newCopy = new Copy(newBook.GetId());
            newCopy.Save();


            DateTime dueDate = new DateTime(2017, 1, 1);
            newPatron.Checkout(newCopy.GetId(), newPatron.GetId(), dueDate);


            List<Copy> expectedResult = new List<Copy>{newCopy};
            List<Copy> actualResult = newPatron.GetCheckedOutCopies();

            Console.WriteLine(actualResult[0].GetId());
            Console.WriteLine(expectedResult[0].GetId());

            Assert.Equal(expectedResult, actualResult);
        }

        // [Fact]
        // public void CheckIn_MarksCopyAsCheckedIn()
        // {
        //
        // }
        //
        // [Fact]
        // public void GetOverDueBooks_ReturnsListOfOverDueBooks()
        // {
        //     Patron newPatron = new Patron("Joe");
        //     newPatron.Save();
        //
        //     Book newBook = new Book("Cotton Eyed Joe");
        //     newBook.Save();
        //
        //     Copy newCopy = new Copy(newBook.GetId());
        //     newCopy.Save();
        //
        //     Book secondBook = new Book("Johnny Boy");
        //     secondBook.Save();
        //
        //     Copy secondCopy = new Copy(secondBook.GetId());
        //     secondCopy.Save();
        //
        //     newPatron.Checkout(newCopy.GetId(), "June 1", "June 12");
        //     newPatron.Checkout(secondCopy.GetId(), "June 1", "June 14");
        //
        //     List<Book> expectedResult = new List<Book> {newBook};
        //     List<Book> actualResult = newPatron.GetOverDue();
        //
        //     Assert.Equal(expectedResult, actualResult);
        //
        // }


        public void Dispose()
        {
            Checkout.DeleteAll();
            Book.DeleteAll();
            Copy.DeleteAll();
            Patron.DeleteAll();
        }
    }
}
