using RiseTheBar.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace RaiseTheBar.Services
{
    public class DefaultSearchSettings : IDefaultSearchSettings
    {
        public double DefaultLat
        {
            get
            {
                string defaultLat = ConfigurationManager.AppSettings["defaultLat"];
                if(string.IsNullOrWhiteSpace(defaultLat))
                {
                    return 0;
                }

                return double.Parse(defaultLat, new CultureInfo("us-US"));
            }
        }

        public double DefaultLon
        {
            get
            {
                string defaultLon = ConfigurationManager.AppSettings["defaultLon"];
                if (string.IsNullOrWhiteSpace(defaultLon))
                {
                    return 0;
                }

                return double.Parse(defaultLon, new CultureInfo("us-US"));
            }
        }
    }
}