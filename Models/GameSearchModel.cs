using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Game_Library_2._0.Models
{
    public class GameSearchModel
    {
        public string Name { get; set; }
        public string Publisher { get; set; }
        public double Price { get; set; }
        public string Operator { get; set; }
    }
}