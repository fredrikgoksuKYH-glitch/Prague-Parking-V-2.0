using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking.Core.Models
{
    public class Bus : Vehicle
    {
        public override int Size => 16;
        public override string VehicleType => "Bus";

        public Bus(string regNumber) : base(regNumber) { }
        public Bus(string regNumber, DateTime parkedTime) : base(regNumber, parkedTime) { }
    }
}
