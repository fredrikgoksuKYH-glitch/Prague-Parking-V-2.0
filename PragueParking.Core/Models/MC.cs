using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking.Core.Models
{
    public class MC : Vehicle
    {
        public override int Size => 2;
        public override string VehicleType => "MC";

        public MC(string regNumber) : base(regNumber) { }
        public MC(string regNumber, DateTime parkedTime) : base(regNumber, parkedTime) { }
    }
}
