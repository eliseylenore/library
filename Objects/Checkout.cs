using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Library
{
    public class Checkout
    {
        private int _id;
        private int _copy_id;
        private int _patron_id;
        private DateTime _due;
        private bool _checkin;

        public Checkout(int CopyId, int PatronId, DateTime Due, int Id = 0)
        {
            _copy_id = CopyId;
            _patron_id = PatronId;
            _due = Due;
            _checkin = false;
            _id = Id;
        }

        public int GetId()
        {
            return _id;
        }

        public DateTime GetDueDate()
        {
            return _due;
        }

        public void SetDueDate( DateTime DueDate)
        {
            _due = DueDate;
        }

        public int GetCopyId()
        {
            return _copy_id;
        }

        public int GetPatronId()
        {
            return _patron_id;
        }


        public bool GetCheckInStatus()
        {
            return _checkin;
        }

        public void SetCheckInStatus(bool changeCheckIn)
        {
            _checkin = changeCheckIn;
        }

        public int TranslateCheckin()
        {
            if (this._checkin == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override bool Equals(System.Object otherCheckout)
        {
            if(!(otherCheckout is Checkout))
            {
                return false;
            }
            else
            {
                Checkout newCheckout = (Checkout) otherCheckout;
                bool idEquality = (this.GetId() == newCheckout.GetId());
                bool copyIdEquality = (this.GetCopyId() == newCheckout.GetCopyId());
                bool patronIdEquality = (this.GetPatronId() == newCheckout.GetPatronId());
                bool dueEquality = (this.GetDueDate() == newCheckout.GetDueDate());
                bool checkInEquality = (this.GetCheckInStatus() == newCheckout.GetCheckInStatus());
                return (idEquality && copyIdEquality && patronIdEquality && dueEquality && checkInEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetId().GetHashCode();
        }

        public static List<Checkout> GetAll()
        {
            List<Checkout> AllCheckOutInfo = new List<Checkout> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM checkouts;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int checkoutId = rdr.GetInt32(0);
                int copyId = rdr.GetInt32(1);
                int patronId = rdr.GetInt32(2);
                DateTime due = rdr.GetDateTime(3);
                bool checkIn;
                if (rdr.GetByte(4) == 1)
                {
                    checkIn = true;
                }
                else{
                    checkIn = false;
                }

                Checkout newCheckout = new Checkout(copyId, patronId, due, checkoutId);
                newCheckout._checkin = checkIn;
                AllCheckOutInfo.Add(newCheckout);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return AllCheckOutInfo;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO checkouts (copy_id, patron_id, due, checkin) OUTPUT INSERTED.id VALUES(@CopyId, @PatronId, @Due, @Checkin);", conn);

            SqlParameter copyId = new SqlParameter();
            copyId.ParameterName = "@CopyId";
            copyId.Value = this.GetCopyId();
            cmd.Parameters.Add(copyId);

            SqlParameter patronId = new SqlParameter();
            patronId.ParameterName = "@PatronId";
            patronId.Value = this.GetPatronId();
            cmd.Parameters.Add(patronId);

            SqlParameter dueDate = new SqlParameter();
            dueDate.ParameterName = "@Due";
            dueDate.Value = this.GetDueDate();
            cmd.Parameters.Add(dueDate);

            SqlParameter checkIn = new SqlParameter();
            checkIn.ParameterName = "@CheckIn";
            checkIn.Value = this.TranslateCheckin();
            cmd.Parameters.Add(checkIn);

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

        public static Checkout FindById(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM checkouts WHERE id = @CheckoutId;", conn);

            SqlParameter checkoutId = new SqlParameter();
            checkoutId.ParameterName = "@CheckoutId";
            checkoutId.Value = id.ToString();
            cmd.Parameters.Add(checkoutId);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundCheckoutId = 0;
            int foundCopyId = 0;
            int foundPatronId = 0;
            DateTime due = new DateTime (1900, 01, 01);
            bool checkin = false;


            while(rdr.Read())
            {
                foundCheckoutId = rdr.GetInt32(0);
                foundCopyId = rdr.GetInt32(1);
                foundPatronId = rdr.GetInt32(2);
                due = rdr.GetDateTime(3);
                if (rdr.GetByte(4) == 1)
                {
                    checkin = true;
                }
                else{
                    checkin = false;
                }

            }

            Checkout foundCheckout = new Checkout(foundCopyId, foundPatronId, due, foundCheckoutId);
            foundCheckout._checkin = checkin;

            if(rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundCheckout;
        }


        public void Delete()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM checkouts WHERE id = @CheckoutId;", conn);

            SqlParameter checkoutId = new SqlParameter();
            checkoutId.ParameterName = "@CheckoutId";
            checkoutId.Value = this.GetId();
            cmd.Parameters.Add(checkoutId);

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
            SqlCommand cmd = new SqlCommand("DELETE FROM checkouts;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
