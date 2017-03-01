using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Library
{
    public class Book
    {
        private int _id;
        private string _title;

        public Book(string Title, int Id = 0)
        {
            _id = Id;
            _title = Title;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetTitle()
        {
            return _title;
        }

        public override bool Equals(System.Object otherBook)
        {
            if(!(otherBook is Book))
            {
                return false;
            }
            else
            {
                Book newBook = (Book) otherBook;
                bool idEquality = (this.GetId() == newBook.GetId());
                bool titleEquality = (this.GetTitle() == newBook.GetTitle());
                return (idEquality && titleEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetTitle().GetHashCode();
        }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM books;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
