using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSIGNMENT
{
    internal class Airline
    {
       public string Name {  get; set; }
       public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }
        public Airline(string name , string code)
        {
            Name = name;    
            Code = code;
            Flights = new Dictionary<string, Flight>();
        }
       public bool Addflight(Flight s)
        {
            if (Flights.ContainsKey(s.FlightNumber))
            {
                Flights.Add(s.FlightNumber, s);
                return true;
            }
            return false;
        }

        public bool Removeflight(Flight s)
        {
            return Flights.Remove(s.FlightNumber);
        }
        public double CalculateFees()
        {
            double totalfees = 0;
            bool code = false;
            foreach (Flight s in Flights.Values)
            {
                if (s.Origin == "SIN")
                {
                    totalfees += 500;
                }
                if (s.Destination == "SIN")
                {
                    totalfees += 800;

                }
                if (Code == "DDJB")
                {
                    totalfees += 300;
                    code = true;
                }
                else if (Code == "CFFT")
                {
                    totalfees += 150;
                    code = true;
                }
                else if (Code == "LWTT")
                {
                    totalfees += 500;
                    code = true;
                }

                if (s.Origin == "DXB" ) 
                {
                    totalfees -= 25;
                }
                else if (s.Origin == "BKK")
                {
                    totalfees -= 25;
                }
                else if (s.Origin == "NRT")
                {
                    totalfees -= 25;
                }
                foreach (var flight in Flights.Values)
                {
                    if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour >= 21)
                    {
                        totalfees += 110;
                    }
                }
            }
                if (Flights.Count < 4)
                {
                    totalfees -= 300;
                }
                if (code == false)
                {
                    totalfees -= 50;
                }
                
                if (Flights.Count > 5)
                {
                    return totalfees *= 0.97;
                }
                return totalfees;
            
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
