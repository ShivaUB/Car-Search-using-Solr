using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace search.Models
{
    public class Car
    {
        public int ID { get; set; }
        public int Year { get; set; }
        public List<string> Make { get; set; }
        public List<string> Model { get; set; }
        public int Price { get; set; }
        public int HorsePower { get; set; }
        public int TopSpeed { get; set; }
        public List<string> ImageUrl { get; set; }
    }
}