using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Library
{
    public class Copy
    {
        private int _id;
        private int _bookId;
        private bool _availability;

        public Copy(int BookId, int Id = 0)
        {
            _id = Id;
            _bookId = BookId;
            _availability = true;
        }

        public int GetId()
        {
            return _id;
        }

        public int GetBookId()
        {
            return _bookId;
        }

        public bool GetAvailability()
        {
            return _availability;
        }

        public void SetAvailability(bool newAvailability)
        {
            _availability = newAvailability;
        }

        public int TranslateAvailability()
        {
            if (this._availability == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
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
                bool availableEquality = (this.GetAvailability() == newCopy.GetAvailability());
                return (idEquality && bookIdEquality && availableEquality);
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
                bool copyAvailability;
                if (rdr.GetByte(2) == 1)
                {
                    copyAvailability = true;
                }
                else{
                    copyAvailability = false;
                }

                Copy newCopy = new Copy(bookId, copyId);
                newCopy._availability = copyAvailability;
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

            SqlCommand cmd = new SqlCommand("INSERT INTO copies(book_id, availability) OUTPUT INSERTED.id VALUES(@BookId, @copyAvailability);", conn);

            SqlParameter bookId = new SqlParameter();
            bookId.ParameterName = "@BookId";
            bookId.Value = this.GetBookId();
            cmd.Parameters.Add(bookId);

            SqlParameter copyAvailability = new SqlParameter();
            copyAvailability.ParameterName = "@copyAvailability";
            copyAvailability.Value = this.TranslateAvailability();
            cmd.Parameters.Add(copyAvailability);

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
            bool copyAvailability = false;


            while(rdr.Read())
            {
                foundCopyId = rdr.GetInt32(0);
                foundBookId = rdr.GetInt32(1);
                if (rdr.GetByte(2) == 1)
                {
                    copyAvailability = true;
                }
                else{
                    copyAvailability = false;
                }

            }

            Copy foundCopy = new Copy(foundBookId, foundCopyId);
            foundCopy._availability = copyAvailability;

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
