using System.IO;
using PRG2_ASSIGNMENT;
//==========================================================
// Student Number	: S10268302
// Student Name	: Mah Kai Sheng
// Partner Name	: Asher Ng
//==========================================================

//1
Terminal Terminal = new Terminal("T5");
using (StreamReader sr = new StreamReader("airlines.csv"))
{
    string header = sr.ReadLine();
    string line;
    while ((line = sr.ReadLine()) != null)
    {
        string[] temp = line.Split(',');
        Airline s = new Airline(temp[0],temp[1]);
        Terminal.Addairline(s);
    }
}

using(StreamReader sr = new StreamReader("boardinggates.csv")) 
{
    string header = sr.ReadLine() ;
    string line;
    while ((line = sr.ReadLine()) != null)
    {
        string[] temp = line.Split(",");
        BoardingGate s = new BoardingGate(temp[0], Convert.ToBoolean(temp[1]), Convert.ToBoolean(temp[2]), Convert.ToBoolean(temp[3]), null);
        Terminal.AddBoardingGate(s);    
    }
}

//4
void ListAllFlights()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Gate Name",-10}{"DDJB",-10}{"CFFT",-10}{"LWTT",-10}");
    foreach (KeyValuePair<string,BoardingGate> kvp in Terminal.BoardingGates)
    {
        Console.WriteLine($"{kvp.Key,-10}{Convert.ToString(kvp.Value.SupportsDDJB),-10}{Convert.ToString(kvp.Value.SupportsCFFT),-10}{Convert.ToString(kvp.Value.SupportsLWTT),-10}");
    }
}

//7
bool checkairline(string code)
{
    foreach (KeyValuePair<string, Airline> kvp in Terminal.Airlines) {
        if (kvp.Key == code)
        {
            return true;
        }
    }
    return false;
}
void DetailsFromairlines()
{
    
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Airline Code",-15}{"Airline Name",-30}");
        foreach (KeyValuePair<string, Airline> kvp in Terminal.Airlines)
        {
            Console.WriteLine($"{kvp.Key,-15}{kvp.Value.Flights,-30}");
        }

        Console.WriteLine("Enter Airline code:");
        string airlinecode = Console.ReadLine();

        if (checkairline(airlinecode) == true)
        {
            Console.WriteLine("=============================================");
            Console.WriteLine($"List of Flights for {Terminal.Airlines[airlinecode]} Airlines");
            Console.WriteLine("=============================================");
            foreach()
        }
        else
        {
            Console.WriteLine("Invalid Code entered!")
        }
}