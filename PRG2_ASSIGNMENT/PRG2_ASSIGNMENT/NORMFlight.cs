using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSIGNMENT
{
    internal class NORMFlight:Flight
    {
        
        public override double CalculateFees()
        {
            return 0;
        }
        public override string ToString()
        {
            return base.ToString();
        }

        public NORMFlight() { }
        public NORMFlight(string Flightno, string origin, string dest, DateTime Expectedtime, string status) : base(Flightno, origin, dest, Expectedtime, status)
        {

        }
    }
}
