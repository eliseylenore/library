using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Library
{
    public class Author
    {
        private int _id;
        private string _name;

        public Author(string Name, int Id = 0)
        {
            _id = Id;
            _name = Name;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetName()
        {
            return _name;
        }

        public override bool Equals(System.Object otherAuthor)
        {
            if(!(otherAuthor is Author))
            {
                return false;
            }
            else
            {
                Author newAuthor = (Author) otherAuthor;
                bool idEquality = (this.GetId() == newAuthor.GetId());
                bool nameEquality = (this.GetName() == newAuthor.GetName());
                return (idEquality && nameEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }

        public static List<Author> GetAll()
        {
            List<Author> AllAuthors = new List<Author> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM authors;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int authorId = rdr.GetInt32(0);
                string authorName = rdr.GetString(1);

                Author newAuthor = new Author(authorName, authorId);
                AllAuthors.Add(newAuthor);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return AllAuthors;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO authors(name) OUTPUT INSERTED.id VALUES(@AuthorName);", conn);

            SqlParameter authorName = new SqlParameter();
            authorName.ParameterName = "@AuthorName";
            authorName.Value = this.GetName();
            cmd.Parameters.Add(authorName);

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

        public static Author FindById(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM authors WHERE id = @AuthorId;", conn);

            SqlParameter authorIdParameter = new SqlParameter();
            authorIdParameter.ParameterName = "@AuthorId";
            authorIdParameter.Value = id.ToString();
            cmd.Parameters.Add(authorIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundAuthorId = 0;
            string foundAuthorName = null;

            while(rdr.Read())
            {
                foundAuthorId = rdr.GetInt32(0);
                foundAuthorName = rdr.GetString(1);
            }
            Author foundAuthor = new Author(foundAuthorName, foundAuthorId);

            if(rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundAuthor;
        }

        public void Update(string newName)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE authors SET name = @NewAuthorName OUTPUT INSERTED.name WHERE id = @AuthorId;", conn);

            SqlParameter newAuthorName = new SqlParameter();
            newAuthorName.ParameterName = "@NewAuthorName";
            newAuthorName.Value = newName;
            cmd.Parameters.Add(newAuthorName);

            SqlParameter authorId = new SqlParameter();
            authorId.ParameterName = "@AuthorId";
            authorId.Value = this.GetId();
            cmd.Parameters.Add(authorId);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._name = rdr.GetString(0);
            }
            if(rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
        }


        public List<Book> GetBooks()
        {
            List<Book> AllBooks = new List<Book>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT books.* FROM authors JOIN authors_books ON (authors.id = authors_books.author_id) JOIN books ON (books.id = authors_books.book_id) WHERE author_id = @AuthorId;", conn);

            SqlParameter authorIdParameter = new SqlParameter();
            authorIdParameter.ParameterName = "@AuthorId";
            authorIdParameter.Value = this.GetId().ToString();
            cmd.Parameters.Add(authorIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int bookId = rdr.GetInt32(0);
                string bookTitle = rdr.GetString(1);

                Book newBook = new Book(bookTitle, bookId);
                AllBooks.Add(newBook);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }

            return AllBooks;
        }

        public void Delete()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM authors WHERE id = @AuthorId;", conn);

            SqlParameter authorIdParameter = new SqlParameter();
            authorIdParameter.ParameterName = "@AuthorId";
            authorIdParameter.Value = this.GetId();
            cmd.Parameters.Add(authorIdParameter);

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
            SqlCommand cmd = new SqlCommand("DELETE FROM authors;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
