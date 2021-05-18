using Aplicativo.View.Atributes;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Aplicativo.View.Routing
{
    public class RouteManager
    {
        public Route[] Routes { get; private set; }

        public void Initialise()
        {

            var Components = Assembly.GetExecutingAssembly().ExportedTypes;

            Components = Components.Where(c => c.IsSubclassOf(typeof(ComponentBase)));
            Components = Components.Where(c => c.GetCustomAttributes(inherit: true).Any(c => c.GetType() == typeof(Page)));

            var RoutesList = new List<Route>();

            foreach (var Component in Components)
            {

                var Page = Component.GetCustomAttribute<Page>();
                var LoginRequired = Component.GetCustomAttribute<LoginRequired>();
                var DatabaseRequired = Component.GetCustomAttribute<DatabaseRequired>();

                var newRoute = new Route
                {
                    Uri = Page.Uri,
                    LoginRequired = LoginRequired?.Required ?? false,
                    DatabaseRequired = DatabaseRequired?.Required ?? true,
                    Component = Component,
                };

                var Equals = RoutesList.Where(c => c.Uri.ToLower() == newRoute.Uri.ToLower());

                if (Equals.Any())
                {

                    var following = "The following routes are ambiguous:" +
                            "\n'" + newRoute.Uri + "' in '" + newRoute.Component.FullName + "'";

                    foreach(var item in Equals)
                    {
                        following += "\n" + item.Uri + "' in '" + item.Component.FullName + "'";
                    }

                    throw new Exception(following);

                }

                RoutesList.Add(newRoute);

            }

            Routes = RoutesList.ToArray();
        }

    }
}