using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//==========================================================
// Student Number	: S10268302
// Student Name	: Mah Kai Sheng
// Partner Name	: Asher Ng
//==========================================================
namespace PRG2_ASSIGNMENT
{
    abstract class Flight:IComparable<Flight>
    {
            public string FlightNumber { get; set; }
            public string Origin { get; set; }
            public string Destination { get; set; }
            public DateTime ExpectedTime { get; set; }
            public string? Status { get; set; }

            public abstract double CalculateFees();


            public Flight(string Flightno, string origin, string dest, DateTime Expectedtime, string status)
            {
                this.FlightNumber = Flightno;
                this.Origin = origin;
                this.Destination = dest;
                this.ExpectedTime = Expectedtime;
                this.Status = status;
            }
            public Flight() { }
        public int CompareTo(Flight other)
        {
            if (other == null)
                return 1;

            return this.ExpectedTime.CompareTo(other.ExpectedTime);
        }
    }
}
