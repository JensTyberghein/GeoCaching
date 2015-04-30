using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoWebservice.Models
{
    public class Location
    {
        public int Id { get; set; }
        public Latitude Latitude { get; set; }
        public Longitude Longitude { get; set; }
        public Altitude Altitude { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}