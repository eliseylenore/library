using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Library
{
    public class Patron
    {
        private int _id;
        private string _name;

        public Patron(string Name, int Id = 0)
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

        public override bool Equals(System.Object otherPatron)
        {
            if(!(otherPatron is Patron))
            {
                return false;
            }
            else
            {
                Patron newPatron = (Patron) otherPatron;
                bool idEquality = (this.GetId() == newPatron.GetId());
                bool nameEquality = (this.GetName() == newPatron.GetName());
                return (idEquality && nameEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }

        public static List<Patron> GetAll()
        {
            List<Patron> AllPatrons = new List<Patron> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM patrons;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int patronId = rdr.GetInt32(0);
                string patronName = rdr.GetString(1);

                Patron newPatron = new Patron(patronName, patronId);
                AllPatrons.Add(newPatron);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return AllPatrons;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO patrons(name) OUTPUT INSERTED.id VALUES(@PatronName);", conn);

            SqlParameter patronName = new SqlParameter();
            patronName.ParameterName = "@PatronName";
            patronName.Value = this.GetName();
            cmd.Parameters.Add(patronName);

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

        public static Patron FindById(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM patrons WHERE id = @PatronId;", conn);

            SqlParameter patronIdParameter = new SqlParameter();
            patronIdParameter.ParameterName = "@PatronId";
            patronIdParameter.Value = id.ToString();
            cmd.Parameters.Add(patronIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundPatronId = 0;
            string foundPatronName = null;

            while(rdr.Read())
            {
                foundPatronId = rdr.GetInt32(0);
                foundPatronName = rdr.GetString(1);
            }
            Patron foundPatron = new Patron(foundPatronName, foundPatronId);

            if(rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundPatron;
        }

        public void Update(string newName)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE patrons SET name = @NewPatronName OUTPUT INSERTED.name WHERE id = @PatronId;", conn);

            SqlParameter newPatronName = new SqlParameter();
            newPatronName.ParameterName = "@NewPatronName";
            newPatronName.Value = newName;
            cmd.Parameters.Add(newPatronName);

            SqlParameter patronId = new SqlParameter();
            patronId.ParameterName = "@PatronId";
            patronId.Value = this.GetId();
            cmd.Parameters.Add(patronId);

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

        public void Delete()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM patrons WHERE id = @PatronId;", conn);

            SqlParameter patronIdParameter = new SqlParameter();
            patronIdParameter.ParameterName = "@PatronId";
            patronIdParameter.Value = this.GetId();
            cmd.Parameters.Add(patronIdParameter);

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
            SqlCommand cmd = new SqlCommand("DELETE FROM patrons;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
