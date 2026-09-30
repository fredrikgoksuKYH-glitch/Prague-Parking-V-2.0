using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking.Core.Interfaces
{
    public interface IVehicle
    {
    string RegNumber { get; set; }
    int Size { get; }
    string VehicleType { get; }
    DateTime ParkedTime { get; set; }
    
    }
}
