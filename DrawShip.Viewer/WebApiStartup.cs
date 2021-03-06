﻿using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace DrawShip.Viewer
{
    public class WebApiStartup
    {
        private static HttpConfiguration _configuation;
        private static List<string> _rootUrls;

        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host.
            _configuation = new HttpConfiguration();
            _configuation.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{directoryKey}/{fileName}/{format}/{version}",
                defaults: new
                {
                    controller = "Drawing",
                    format = DiagramFormat.Html,
                    version = RouteParameter.Optional
                }
            );

            _configuation.Routes.MapHttpRoute(
                name: "Index",
                routeTemplate: "{directoryKey}",
                defaults: new
                {
                    controller = "Index",
                    directoryKey = RouteParameter.Optional
                }
            );

            appBuilder.UseWebApi(_configuation);
        }

        public static IDisposable Start(IEnumerable<string> urls)
        {
            var givenUrls = urls.ToList();

            _rootUrls = givenUrls.Select(url => url.Replace("+", Environment.MachineName)).ToList();

            var options = new StartOptions();
            givenUrls.ForEach(url => options.Urls.Add(url));

            try
            {
                return WebApp.Start<WebApiStartup>(options);
            }
            catch (TargetInvocationException exc)
            {
                throw new InvalidOperationException($"Unable to start web-host - {exc.InnerException.GetType().Name}, for urls: {string.Join(", ", urls)}\r\n{exc.InnerException.Message}");
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException($"Unable to start web-host - {exc.GetType().Name}, for urls: {string.Join(", ", urls)}", exc.InnerException);
            }
        }

        public static Uri FormatUrl(int workingDirectoryKey, string fileName, DiagramFormat format, string version, string routeName = "DefaultApi")
        {
            var route = _configuation.Routes[routeName];
            var allRouteValues = new Dictionary<string, object>
            {
                { "directoryKey", workingDirectoryKey },
                { "fileName", fileName },
                { "format", format },
                { "version", version }
            };
            var routeValues = _RemoveDefaults(allRouteValues, route.Defaults);
            var relativeUrl = route.RouteTemplate.Supplant(routeValues).RemoveAll("//");

            return new Uri(_rootUrls.First() + "/" + relativeUrl, UriKind.Absolute);
        }

        private static IReadOnlyDictionary<string, object> _RemoveDefaults(
            IReadOnlyDictionary<string, object> allRouteArgs,
            IDictionary<string, object> defaultRouteArgs)
        {
            return (from routeArg in allRouteArgs
                    join defaultRouteArg in defaultRouteArgs on routeArg.Key equals defaultRouteArg.Key into outerJoin
                    let defaultRouteValue = outerJoin.SingleOrDefault().Value
                    let routeValue = routeArg.Value
                    where _HasRouteValue(routeValue, defaultRouteValue)
                    select routeArg)
                   .ToDictionary(a => a.Key, a => a.Value);
        }

        private static bool _DefaultValue(object routeValue, object defaultRouteValue)
        {
            return Equals(routeValue, defaultRouteValue);
        }

        private static bool _HasRouteValue(object routeValue)
        {
            var stringRouteValue = routeValue as string;
            return routeValue != null && stringRouteValue != "";
        }

        private static bool _HasRouteValue(object routeValue, object defaultRouteValue)
        {
            var hasRouteValue = _HasRouteValue(routeValue);
            var isOptional = defaultRouteValue == RouteParameter.Optional;

            if (!hasRouteValue && isOptional)
                return false;

            var matchesDefault = Equals(routeValue, defaultRouteValue);
            if (hasRouteValue && matchesDefault)
                return false;

            return true;
        }
    }
}
