using GeoWebservice.Models;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Web;

namespace GeoWebservice.Helper
{
    public class SerialPortHelpers
    {
        // Create a new SerialPort object with default settings.
        SerialPort SerialPort = new SerialPort();

        public void Init()
        {
            // Allow the user to set the appropriate properties.
            SerialPort.PortName = "COM3";
            SerialPort.BaudRate = 4800;
            SerialPort.Parity = Parity.None;
            SerialPort.DataBits = 8;
            SerialPort.StopBits = StopBits.One;
            SerialPort.Handshake = Handshake.None;            
        }

        public void Open()
        {
            if (SerialPort.IsOpen == false)
            {
                SerialPort.Open();
            }
        }

        public void Close()
        {
            if (SerialPort.IsOpen == true)
            {
                SerialPort.Close();
            }            
        }

        public string ReadExisting()
        {
            string data = SerialPort.ReadExisting();
            return data;
        }

        public Location GetLocation(string data)
        {
            Location Location = new Location();

            Latitude Latitude  = new Latitude();
            Longitude Longitude = new Longitude();
            Altitude Altitude = new Altitude();

            string[] DollarStukken = data.Split('$');
            foreach (string DollarStuk in DollarStukken)
            {
                if (Latitude.Value != null) break;                
                    
                string[] Stukken = DollarStuk.Split(',');
                if (Stukken[0] == "GPGGA")
                {
                    if (Stukken.Length == 15)
                    {
                        Latitude.Value = convertLatitudeToGlenn(Stukken[2]);
                        Latitude.LatitudeDirection = Stukken[3];
                        Longitude.Value = convertLongitudeToGlenn(Stukken[4]);
                        Longitude.LongitudeDirection = Stukken[5];
                        Altitude.Value = Stukken[9];
                        Altitude.Eenheid = Stukken[10];
                    }
                }                          
            }

            Location.Latitude = Latitude;
            Location.Longitude = Longitude;
            Location.Altitude = Altitude;

            return Location;
        }

        public string convertLatitudeToGlenn(string value)
        {
            double GlennLatitude = 0;
            double hoi = value.Length;

            if (value.Length == 9)
            {
                double Degrees = int.Parse(value.Substring(0, 2));
                double Minutes = int.Parse(value.Substring(2, 2));
                double SecondsTemp = int.Parse(value.Split('.')[1]);
                double Seconds = (SecondsTemp / 10000) * 60;
                GlennLatitude = Degrees + (Minutes / 60) + (Seconds / 3600);
                return "" + GlennLatitude;
            }
            
            return "";
        }

        public string convertLongitudeToGlenn(string value)
        {
            double GlennLongitude = 0;

            if (value != "")
            {
                double Degrees;
                double Minutes;
                double SecondsTemp;

                double.TryParse(value.Substring(0, 3), out Degrees);
                double.TryParse(value.Substring(3, 2), out Minutes);
                double.TryParse(value.Split('.')[1], out SecondsTemp);            
                
                
                double Seconds = (SecondsTemp / 10000) * 60;
                GlennLongitude = Degrees + (Minutes / 60) + (Seconds / 3600);
                return "" + GlennLongitude;
            }

            return "";
        }
    }
}