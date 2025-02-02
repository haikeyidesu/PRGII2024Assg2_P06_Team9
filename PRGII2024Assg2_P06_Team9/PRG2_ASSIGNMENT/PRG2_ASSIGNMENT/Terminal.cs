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
    internal class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; }
        public Dictionary<string,Flight> Flights { get; set; }
        public Dictionary<string , BoardingGate> BoardingGates { get; set; }
        public Dictionary <string,  double > GateFees {  get; set; }    

        public bool Addairline(Airline s)
        {
            if (!Airlines.ContainsKey(s.Code))
            {
                Airlines.Add(s.Code, s);
                return true;
            }
            return false;
        }

        public bool AddBoardingGate(BoardingGate gate)
        {
            if (!BoardingGates.ContainsKey(gate.GateName))
            {
                BoardingGates.Add(gate.GateName, gate);
                return true;
            }
            return false;
        }
        public Airline GetAirlineFromFlight(Flight flight)
        {
            foreach(Airline s in Airlines.Values)
            {
                if (flight.FlightNumber.Substring(0, 2) == s.Code)
                {
                    return s;
                }
            }
            return null;
        }

        public void PrintAirlineFees()
        {
            foreach (var airline in Airlines.Values)
            {
                Console.WriteLine($"{airline}: Fees = {airline.CalculateFees():C}");
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public Terminal (string name)
        {
            TerminalName = name;
            Airlines = new Dictionary<string, Airline>();
            Flights = new Dictionary<string, Flight>();
            BoardingGates = new Dictionary<string, BoardingGate>();
            GateFees = new Dictionary<string, double>();
        }
    }
}
