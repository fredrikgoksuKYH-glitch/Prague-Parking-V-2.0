using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking.Core.Models
{
    public class Car : Vehicle
    {
        public override int Size => 4;
        public override string VehicleType => "Car";

        public Car(string regNumber) : base(regNumber) { }
        public Car(string regNumber, DateTime parkedTime) : base(regNumber, parkedTime) { }
    }
}
