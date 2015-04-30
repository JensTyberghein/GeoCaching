using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace GeoWebservice.Models
{
    public class LocationRepository : GenericRepository<Location>, GeoWebservice.Models.ILocationRepository
    {
        public void AddLocation(Location Location)
        {
            context.Location.Add(Location);
            context.SaveChanges();
        }

        public Location GetLastLocation()
        {
            var query = (from l in context.Location.Include(lat => lat.Latitude).Include(lon => lon.Longitude).Include(alt => alt.Altitude) orderby l.TimeStamp descending select l);
            return query.First<Location>();
        }

        public int GetAmmountOfLocations()
        {
            var query = (from l in context.Location select l).Count();
            return query;
        }

        public void ClearDb()
        {
            foreach (var loc in context.Location.Include(lat => lat.Latitude).Include(lon => lon.Longitude).Include(alt => alt.Altitude))
            {
                context.Location.Remove(loc);
            }
            context.SaveChanges();
        }
    }
}