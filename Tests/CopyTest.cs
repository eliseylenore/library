using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
    public class CopyTest : IDisposable
    {
        public CopyTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_EqualOverride_true()
        {
            Copy firstCopy = new Copy (1);
            Copy secondCopy = new Copy (1);

            Assert.Equal(firstCopy, secondCopy);
        }

        [Fact]
        public void Save_SavesNewCopy()
        {
            Copy newCopy = new Copy (1);
            newCopy.Save();

            List<Copy> expectedResult = new List<Copy>{newCopy};
            List<Copy> actual = Copy.GetAll();

            Assert.Equal(expectedResult, actual);
        }

        [Fact]
        public void FindById_ReturnsCopyWhenSearchedById()
        {
            Copy newCopy = new Copy(1);
            newCopy.Save();

            Copy result = Copy.FindById(newCopy.GetId());

            Assert.Equal(newCopy, result);
        }

        [Fact]
        public void Delete_DeletesCopyFromDatabase()
        {
            Copy newCopy = new Copy(1);
            newCopy.Save();

            newCopy.Delete();

            List<Copy> expectedResult = new List<Copy>{};
            List<Copy> actualResult = Copy.GetAll();

            Assert.Equal(expectedResult, actualResult);
        }

        public void Dispose()
        {
            Copy.DeleteAll();
        }
    }
}
