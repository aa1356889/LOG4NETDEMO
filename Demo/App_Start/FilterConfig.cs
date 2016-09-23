using System.Web;
using System.Web.Mvc;
using Demo.Attribute;

namespace Demo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new WebHandleErrorAttribute());
        }
    }
}