using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
    public class CheckoutTest : IDisposable
    {
        public CheckoutTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_EqualOverride_true()
        {
            DateTime dueDate = new DateTime(2017, 1, 1);
            Checkout firstCheckout = new Checkout (1, 2, dueDate);
            Checkout secondCheckout = new Checkout (1, 2, dueDate);

            Assert.Equal(firstCheckout, secondCheckout);
        }

        [Fact]
        public void Save_SavesNewCheckout()
        {
            DateTime dueDate = new DateTime(2017, 1, 1);
            Checkout newCheckout = new Checkout (1, 2, dueDate);
            newCheckout.Save();

            List<Checkout> expectedResult = new List<Checkout>{newCheckout};
            List<Checkout> actual = Checkout.GetAll();

            Assert.Equal(expectedResult, actual);
        }

        [Fact]
        public void FindById_ReturnsCheckoutWhenSearchedById()
        {
            DateTime dueDate = new DateTime(2017, 1, 1);
            Checkout newCheckout = new Checkout (1, 2, dueDate);
            newCheckout.Save();

            Checkout result = Checkout.FindById(newCheckout.GetId());

            Assert.Equal(newCheckout, result);
        }

        [Fact]
        public void Delete_DeletesCheckoutFromDatabase()
        {
            DateTime dueDate = new DateTime(2017, 1, 1);
            Checkout newCheckout = new Checkout (1, 2, dueDate);
            newCheckout.Save();

            newCheckout.Delete();

            List<Checkout> expectedResult = new List<Checkout>{};
            List<Checkout> actualResult = Checkout.GetAll();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void CheckIn_MakeCheckInEqualToTrue()
        {
            DateTime dueDate = new DateTime(2017, 1, 1);
            Checkout newCheckout = new Checkout (1, 2, dueDate);
            newCheckout.Save();

            newCheckout.CheckIn();

            bool expectedResult = true;
            bool actualResult = newCheckout.GetCheckInStatus();

            Assert.Equal(expectedResult, actualResult);
        }

        



        public void Dispose()
        {
            Checkout.DeleteAll();
        }
    }
}
