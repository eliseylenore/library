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

            Get["/patron/{id}"] = parameters => {
                Dictionary<string, object> model = new Dictionary<string, object>{};
                var SelectedPatron = Patron.FindById(parameters.id);
                List<Copy> PatronCheckouts = SelectedPatron.GetAllCheckedOutCopies();
                model.Add("patron", SelectedPatron);
                model.Add("copies", PatronCheckouts);
                return View["Patron/patron.cshtml", model];
            };

            Post["/patron/{id}"] = parameters => {
                Dictionary<string, object> model = new Dictionary<string, object>{};
                var SelectedPatron = Patron.FindById(parameters.id);
                List<Copy> PatronCheckouts = SelectedPatron.GetCheckedOutCopies(Request.Form["today-date"]);
                model.Add("patron", SelectedPatron);
                model.Add("copies", PatronCheckouts);
                return View["Patron/patron.cshtml", model];
            };
        }
    }
}
