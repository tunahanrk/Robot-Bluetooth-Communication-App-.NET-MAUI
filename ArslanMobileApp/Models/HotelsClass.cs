using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArslanMobileApp.Models
{
    public class HotelsClass
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; } 
        public int Star { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }
        public string StarSymbol
        {
            get
            {
                return new string('★', Star) + new string('☆', 5 - Star);
            }
        }
    }
}
