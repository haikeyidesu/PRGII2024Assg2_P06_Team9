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
    internal class DDJBFlight:Flight
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

        public DDJBFlight() { }
        public DDJBFlight(double requestFee, string Flightno, string origin, string dest, DateTime Expectedtime, string status) : base(Flightno, origin, dest, Expectedtime, status)
        {
            RequestFee = requestFee;
        }
    }
}
