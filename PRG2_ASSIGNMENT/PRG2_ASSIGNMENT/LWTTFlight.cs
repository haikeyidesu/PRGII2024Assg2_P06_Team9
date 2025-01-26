using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSIGNMENT
{
    internal class LWTTFlight:Flight
    {
        public double RequestFee { get; set; }
        public override double CalculateFees()
        {
            return RequestFee;
        }
        public override string ToString()
        {
            return base.ToString();
        }

        public LWTTFlight() { }
        public LWTTFlight(double requestFee, string Flightno, string origin, string dest, DateTime Expectedtime, string status) : base(Flightno, origin, dest, Expectedtime, status)
        {
            RequestFee = requestFee;
        }
    }
}
