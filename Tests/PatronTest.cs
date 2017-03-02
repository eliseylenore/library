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


            DateTime dueDate = new DateTime(2017, 6, 1);
            newPatron.Checkout(newCopy.GetId(), newPatron.GetId(), dueDate);

            DateTime currentDate = new DateTime(2017, 7, 1);
            List<Copy> expectedResult = new List<Copy>{newCopy};
            List<Copy> actualResult = newPatron.GetCheckedOutCopies(currentDate);



            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetCheckedOutCopies_OnlyReturnsCheckedoutBook()
        {
            DateTime dueDate = new DateTime(2017, 6, 1);

            Patron newPatron = new Patron("Joe");
            newPatron.Save();

            Book newBook = new Book("Gone With the Wind");
            newBook.Save();

            Copy copy1 = new Copy(newBook.GetId());
            copy1.Save();
            newPatron.Checkout(copy1.GetId(), newPatron.GetId(), dueDate);

            Copy copy2 = new Copy(newBook.GetId());
            copy2.Save();
            Checkout newCheckout = newPatron.Checkout(copy2.GetId(), newPatron.GetId(), dueDate);
            newCheckout.CheckIn();


            DateTime currentDate = new DateTime(2017, 7, 1);
            List<Copy> expectedResult = new List<Copy>{copy1};
            List<Copy> actualResult = newPatron.GetCheckedOutCopies(currentDate);

            Assert.Equal(expectedResult, actualResult);
        }




        public void Dispose()
        {
            Checkout.DeleteAll();
            Book.DeleteAll();
            Copy.DeleteAll();
            Patron.DeleteAll();
        }
    }
}
