using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking.Core.Interfaces
{
    public interface IParkingSpot
    {
        List<IVehicle> iVehicles { get; set; }
        int SpotNumber { get; set; }
        int Capacity { get; set; }
        int UsedSpace { get; }
        int AvaliableSpace { get; }
        bool isFull { get; }
        bool isEmpty { get; }
        bool CanFit(IVehicle vehicle);
    }
}
