using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Helper
{
    public static class HtmlExtension
    {
        public static string IfPageActive(this IHtmlHelper html, string value)
        {
            var controller = html.ViewContext.RouteData.Values["Controller"].ToString();
            if (value == null || value == "")
            {
                return "";
            }
            if (value.Equals(controller))
            {
                return "active";
            }
            else
            {
                return "";
            }
        }

        public static string IfPageActive(this IHtmlHelper html, string value,string value2)
        {
            var controller = html.ViewContext.RouteData.Values["Controller"].ToString();
            var action  = html.ViewContext.RouteData.Values["Action"].ToString();
            if (value == null || value == "" || value2 == null || value2 == "")
            {
                return "";
            }
            if (value.Equals(controller) && value2.Equals(action))
            {
                return "active";
            }
            else
            {
                return "";
            }
        }
        public static string IfAnyPageActive(this IHtmlHelper html, string[] value)
        {
            var controller = html.ViewContext.RouteData.Values["Controller"].ToString();
            //var action = html.ViewContext.RouteData.Values["Action"].ToString();
            if (value == null)
            {
                return "";
            }

            foreach (var p in value)
            {
                if (p.Equals(controller))
                {
                    return "menu-open";
                }
            }



            return "";

        }

        public static string IfAnyPageActive2(this IHtmlHelper html, string[] value)
        {
            var controller = html.ViewContext.RouteData.Values["Controller"].ToString();
            //var action = html.ViewContext.RouteData.Values["Action"].ToString();
            if (value == null)
            {
                return "";
            }

            foreach (var p in value)
            {
                if (p.Equals(controller))
                {
                    return "active";
                }
            }



            return "";

        }
    }
}
