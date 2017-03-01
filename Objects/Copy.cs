using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Library
{
    public class Copy
    {
        private int _id;
        private int _bookId;

        public Copy(int BookId, int Id = 0)
        {
            _id = Id;
            _bookId = BookId;
        }

        public int GetId()
        {
            return _id;
        }

        public int GetBookId()
        {
            return _bookId;
        }

        public override bool Equals(System.Object otherCopy)
        {
            if(!(otherCopy is Copy))
            {
                return false;
            }
            else
            {
                Copy newCopy = (Copy) otherCopy;
                bool idEquality = (this.GetId() == newCopy.GetId());
                bool bookIdEquality = (this.GetBookId() == newCopy.GetBookId());
                return (idEquality && bookIdEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetBookId().GetHashCode();
        }

        public static List<Copy> GetAll()
        {
            List<Copy> AllCopies = new List<Copy> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM copies;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int copyId = rdr.GetInt32(0);
                int bookId = rdr.GetInt32(1);

                Copy newCopy = new Copy(bookId, copyId);
                AllCopies.Add(newCopy);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return AllCopies;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO copies(book_id) OUTPUT INSERTED.id VALUES(@BookId);", conn);

            SqlParameter bookId = new SqlParameter();
            bookId.ParameterName = "@BookId";
            bookId.Value = this.GetBookId();
            cmd.Parameters.Add(bookId);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }

            if(conn != null)
            {
                conn.Close();
            }
        }

        public static Copy FindById(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM copies WHERE id = @CopyId;", conn);

            SqlParameter copyId = new SqlParameter();
            copyId.ParameterName = "@CopyId";
            copyId.Value = id.ToString();
            cmd.Parameters.Add(copyId);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundCopyId = 0;
            int foundBookId = 0;

            while(rdr.Read())
            {
                foundCopyId = rdr.GetInt32(0);
                foundBookId = rdr.GetInt32(1);
            }
            Copy foundCopy = new Copy(foundBookId, foundCopyId);

            if(rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundCopy;
        }


        public void Delete()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM copies WHERE id = @CopyId;", conn);

            SqlParameter copyId = new SqlParameter();
            copyId.ParameterName = "@CopyId";
            copyId.Value = this.GetId();
            cmd.Parameters.Add(copyId);

            cmd.ExecuteNonQuery();

            if(conn != null)
            {
                conn.Close();
            }
        }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM copies;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
