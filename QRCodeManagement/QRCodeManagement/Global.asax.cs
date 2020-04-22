﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using QRCodeManagement.Models;

namespace QRCodeManagement
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<AdminDbContext>(new CreateDatabaseIfNotExists<AdminDbContext>());
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void Application_BeginRequest(Object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["ForceHttps"].ToString() == "true")
            {
                if (!HttpContext.Current.Request.IsSecureConnection)
                {
                    HttpContext.Current.Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority).Replace("http://", "https://"), true);
                }
            }
        }
    }
}
