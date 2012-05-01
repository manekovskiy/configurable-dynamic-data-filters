using System;
using System.Web;
using ConfigurableDynamicDataFilters.Model;

namespace ConfigurableDynamicDataFilters
{
    public class Application : HttpApplication
    {
        //private const string EntitiesContentKey = "EntitiesContext.Instance";

        //public Application()
        //{
        //    BeginRequest += (sender, args) =>
        //    {
        //        HttpContext.Current.Items[EntitiesContentKey] = new EntitiesContext();
        //    };

        //    EndRequest += (sender, args) =>
        //    {
        //        CurrentDbContext.SaveChanges();

        //        var disposable = HttpContext.Current.Items[EntitiesContentKey] as IDisposable;
        //        if (disposable != null)
        //        {
        //            disposable.Dispose();
        //            HttpContext.Current.Items[EntitiesContentKey] = null;
        //        }
        //    };
        //}

        //public static EntitiesContext CurrentDbContext
        //{
        //    get { return HttpContext.Current.Items[EntitiesContentKey] as EntitiesContext; }
        //}
    }
}
