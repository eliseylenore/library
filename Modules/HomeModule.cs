using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace Library
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => {
                List<Patron> AllPatrons = Patron.GetAll();
                return View["Patron/index.cshtml", AllPatrons];
            };
        }
    }
}
