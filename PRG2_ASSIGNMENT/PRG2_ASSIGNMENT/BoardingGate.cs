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
    internal class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsCFFT {  get; set; }
        public bool SupportsDDJB {  get; set; }
        public bool SupportsLWTT {  get; set; }
        public Flight flight { get; set; }

        public double CalculateFees()
        {
            double total = 0;
            if (SupportsCFFT == true)
            {
                total += 150;
            }
            else if (SupportsDDJB == true)
            {
                total += 300;
            }
            else if (SupportsLWTT == true)
            {
                total += 500;
            }
            return total;
        }
        public override string ToString()
        {
            return base.ToString();
        }
        public BoardingGate() { }
        public BoardingGate(string gateName,bool supportscfft, bool supportsDDJB, bool supportsLWTT, Flight f)
        {
            GateName = gateName;
            SupportsCFFT = supportscfft;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
            flight = f;
        }
    }
}
