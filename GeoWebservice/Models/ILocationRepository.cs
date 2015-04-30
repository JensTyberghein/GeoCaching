using System;
namespace GeoWebservice.Models
{
    interface ILocationRepository
    {
        void AddLocation(Location Location);
        Location GetLastLocation();
        int GetAmmountOfLocations();
        void ClearDb();        
    }
}
