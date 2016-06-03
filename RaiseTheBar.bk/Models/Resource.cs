using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiseTheBar.Models
{
    public class Resource<T>
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Href
        {
            get
            {
                return Type + "/" + ID; 
            }
        }
        public T Value { get; set; }
    }

}