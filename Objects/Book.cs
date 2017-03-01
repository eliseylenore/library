using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Library
{
  public class Book
  {
      private int _id;
      private string _title;

      public Book(string title, int Id = 0)
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

      public overrides bool Equals(Book otherBook)
      {
          if(!(otherBook is Book))
          {
              return false;
          }
          else
          {
              Book newBook = (Book) otherBook;
              bool idEquality = (this.GetId() == newBook.GetId());
              bool titleEquality = (this.GetTitle() == newBook.GetId());
              return (idEquality && titleEquality);
          }
      }
  }
}
