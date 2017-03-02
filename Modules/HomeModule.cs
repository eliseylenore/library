using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace Library
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => View["index.cshtml"];

            Get["/patrons"] = _ => {
                List<Patron> AllPatrons = Patron.GetAll();
                return View["Patron/patron_index.cshtml", AllPatrons];
            };


            Post["/patron/new"] = _ => {
                Patron newPatron = new Patron(Request.Form["name"]);
                newPatron.Save();
                List<Patron> AllPatrons = Patron.GetAll();
                return View["Patron/patron_index.cshtml", AllPatrons];
            };
        }
    }
}
