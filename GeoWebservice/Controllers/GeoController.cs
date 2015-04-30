using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using GeoWebservice.Helper;
using GeoWebservice.Models;
using System.Text;

namespace GeoWebservice.Controllers
{
    public class GeoController : ApiController
    {
        // Link: http://geowebservice.azurewebsites.net/Api/Geo
        //private string HARDCODEDDATA = "$GPGGA,110730.000,5049.4781,N,00314.9857,E,1,10,0.9,44.9,M,47.2,M,,0000*6F\r\n$GPGSA,A,3,07,30,15,20,09,05,28,21,13,10,,,1.5,0.9,1.2*38\r\n$GPGSV,3,1,12,30,70,079,34,05,67,250,21,13,47,281,20,07,39,057,45*73\r\n$GPGSV,3,2,12,20,39,170,21,28,29,141,20,10,28,157,24,15,12,281,32*72\r\n$GPGSV,3,3,12,02,08,219,,09,08,097,23,21,08,330,22,27,04,026,30*79\r\n$GPRMC,110730.000,A,5049.4781,N,00314.9857,E,0.61,161.13,260315,,*00\r\n$GPVTG,161.13,T,,M,0.61,N,1.1,K*63\r\n$GPGGA,110731.000,5049.4777,N,00314.9859,E,1,10,0.9,44.1,M,47.2,M,,0000*61\r\n$GPGSA,A,3,07,30,15,20,09,05,28,21,13,10,,,1.5,0.9,1.2*38\r\n$GPRMC,110731.000,A,5049.4777,N,00314.9859,E,0.24,107.31,260315,,*07\r\n$GPVTG,107.31,T,,M,0.24,N,0.4,K*66\r\n$GPGGA,110732.000,5049.4776,N,00314.9861,E,1,10,0.9,43.7,M,47.2,M,,0000*69\r\n$GPGSA,A,3,07,30,15,20,09,05,28,21,13,10,,,1.5,0.9,1.2*38\r\n$GPRMC,110732.000,A,5049.4776,N,00314.9861,E,0.32,86.57,260315,,*31\r\n$GPVTG,86.57,T,,M,0.32,N,0.6,K*5B\r\n";
        // text
        //fzfaegazg

        public void Get(int id)
        {
            LocationRepository LocRepo = new LocationRepository();
            LocRepo.ClearDb();

            while (1 > 0)
            {
                System.Threading.Thread.Sleep(5000);

                SerialPortHelpers SerialPort = new SerialPortHelpers();
                Location Location = new Location();
                SerialPort.Init();
                SerialPort.Open();
                System.Threading.Thread.Sleep(5000);
                string data = SerialPort.ReadExisting();
                Location = SerialPort.GetLocation(data);
                Location.TimeStamp = DateTime.Now;

                if (Location.Altitude != null && Location.Latitude != null && Location.Longitude != null)
                {
                    if (Location.Altitude.Value != "" && Location.Latitude.Value != "" && Location.Longitude.Value != "") 
                    {
                        LocRepo.AddLocation(Location);
                    }
                }                

                if (LocRepo.GetAmmountOfLocations() > 100)
                {
                    LocRepo.ClearDb();
                }
                Console.WriteLine("Location Updated:" + "Latitude: " + Location.Latitude + ", Longtitude : " + Location.Longitude + ", Altitude : " + Location.Longitude + ".");
                SerialPort.Close();  
            }   
        }

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public Location GetLocation()
        {
            LocationRepository LocRepo = new LocationRepository();
            Location Location =  LocRepo.GetLastLocation();
            return Location;
        }
    }
}
