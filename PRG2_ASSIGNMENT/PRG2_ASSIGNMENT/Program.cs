using System;
using System.IO;
using System.Transactions;
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


//2 

using (StreamReader sw = new StreamReader("flights.csv"))
{
    string header = sw.ReadLine();
    string line;
    while ((line = sw.ReadLine()) != null)
    {
        string[] temp = line.Split(",");
        if (temp[4] == "CFFT")
        {
            Flight s = new CFFTFlight(150, temp[0], temp[1], temp[2], Convert.ToDateTime(temp[3]), "Unassigned");
            Airline a = Terminal.GetAirlineFromFlight(s);
            a.Addflight(s);
        }
        else if (temp[4] == "DDJB")
        {
            Flight s = new DDJBFlight(300, temp[0], temp[1], temp[2], Convert.ToDateTime(temp[3]), "Unassigned");
            Airline a = Terminal.GetAirlineFromFlight(s);
            a.Addflight(s);
        }
        else if (temp[4] == "LWTT")
        {
            Flight s = new LWTTFlight(500, temp[0], temp[1], temp[2], Convert.ToDateTime(temp[3]), "Unassigned");
            Airline a = Terminal.GetAirlineFromFlight(s);
            a.Addflight(s);
        }
        else if (temp[4] == "")
        {
            Flight s = new NORMFlight(temp[0], temp[1], temp[2], Convert.ToDateTime(temp[3]), "Unassigned");
            Airline a = Terminal.GetAirlineFromFlight(s);
            a.Addflight(s);
        }
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
        Console.WriteLine($"{kvp.Key,-15}{kvp.Value.Name,-30}");
    }

    Console.Write("Enter Airline code:");
    string airlinecode = Console.ReadLine();

    if (checkairline(airlinecode) == true)
    {
        Airline s = Terminal.Airlines[airlinecode];
        Console.WriteLine("=============================================");
        Console.WriteLine($"List of Flights for {s.Name}");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Flight Number",-15}{"Airline name",-25}{"Origin",-20}{"Detination",-20}{"Expected Departure/Arrival time",-30}");
        foreach (KeyValuePair<string, Flight> kvp in s.Flights)
        {
            Console.WriteLine($"{kvp.Key,-15}{s.Name,-25}{kvp.Value.Origin,-20}{kvp.Value.Destination,-20}{kvp.Value.ExpectedTime,-30}");
        }
    }
    else
    {
        Console.WriteLine("Invalid Code entered!");
    }
}
//8
Flight ModifySpecialRequestCode(Flight oldFlight, string newCode)
{
    Flight newFlight;
    switch (newCode)
    {
        case "CFFT":
            newFlight = new CFFTFlight(150, oldFlight.FlightNumber, oldFlight.Origin, oldFlight.Destination, oldFlight.ExpectedTime,oldFlight.Status);
            break;
        case "DDJB":
            newFlight = new DDJBFlight(300, oldFlight.FlightNumber, oldFlight.Origin, oldFlight.Destination, oldFlight.ExpectedTime,oldFlight.Status);
            break;
        case "LWTT":
            newFlight = new LWTTFlight(500, oldFlight.FlightNumber, oldFlight.Origin, oldFlight.Destination, oldFlight.ExpectedTime, oldFlight.Status);
            break;
        default:
            newFlight = new NORMFlight(oldFlight.FlightNumber, oldFlight.Origin, oldFlight.Destination, oldFlight.ExpectedTime, oldFlight.Status);
            break;
    }
    return newFlight;
}
    void ModifyFlightDetails()
{
    DetailsFromairlines();
    Console.Write("Choose an existing Flight to modify or delete: ");
    string flightno = Console.ReadLine();
    Console.WriteLine("1.Modify Flight");
    Console.WriteLine("2.Delete Flight");
    Console.Write("Choose an option: ");
    string option= Console.ReadLine();
    if (option != "1" && option != "2") throw new Exception("Enter either 1 or 2");
    if (option == "1")
    {
        Console.WriteLine("1. Modify Basic Information");
        Console.WriteLine("2. Modify Status");
        Console.WriteLine("3. Modify Special Request Code");
        Console.WriteLine("4. Modify Boarding Gate");
        Console.WriteLine("Choose an option: ");
        string option2 = Console.ReadLine();
        if (option2 != "1" && option2 != "2" && option2 != "3" && option2 != "4") throw new Exception("Enter either 1 , 2 , 3 or 4!");
        if (option2 == "1")
        {
            Console.Write("Enter new origin: ");
            string origin = Console.ReadLine();
            Console.Write("Enter new destination: ");
            string dest = Console.ReadLine();
            Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
            DateTime daTime = Convert.ToDateTime(Console.ReadLine());
            foreach (Airline s in Terminal.Airlines.Values)
            {
                if (s.Code == flightno.Substring(0, 2))
                {
                    foreach (Flight a in s.Flights.Values)
                    {
                        if (a.FlightNumber == flightno)
                        {
                            a.Origin = origin;
                            a.Destination = dest;
                            a.ExpectedTime = daTime;
                            Console.WriteLine($"FlightNo: {a.FlightNumber}\nAirline Name :{s.Name}\nOrigin:{a.Origin}\nDestination:{a.Destination}\nExpected Departure/Arrival Time: {a.ExpectedTime}\nStatus: {a.Status}\nSpecial Request Code:{a.GetType().Name.Substring(0, 4)}\nBoarding Gate:Unassigned");
                        }
                    }
                }
            }
        }
        if (option2 == "2")
        {

            Console.Write("Enter a new status(Scheduled/Unassigned/Boarding/Delayed/On Time): ");
            string stat = Console.ReadLine() ;
            foreach (Airline s in Terminal.Airlines.Values)
            {
                if (s.Code == flightno.Substring(0, 2))
                {
                    foreach (Flight a in s.Flights.Values)
                    {
                        if (a.FlightNumber == flightno)
                        {
                            a.Status = stat;
                            Console.WriteLine($"FlightNo: {a.FlightNumber}\nAirline Name :{s.Name}\nOrigin:{a.Origin}\nDestination:{a.Destination}\nExpected Departure/Arrival Time: {a.ExpectedTime}\nStatus: {a.Status}\nSpecial Request Code:{a.GetType().Name.Substring(0, 4)}\nBoarding Gate:Unassigned");
                        }
                    }
                }
            }
        }
        if (option2 == "3") 
        {
            Console.Write("Enter the new request code(Enter 'NORM' for normal flights): ");
            string code = Console.ReadLine() ;
            if (code != "CFFT" && code != "DDJB" && code != "LWTT" && code != "NORM") throw new Exception("Please enter a valid code!");
            Flight TargetFlight = null;
            Airline TargetAirline = null;
            foreach (Airline s in Terminal.Airlines.Values)
            {
                if (s.Code == flightno.Substring(0, 2))
                {
                    foreach (Flight a in s.Flights.Values)
                    {
                        if (a.FlightNumber == flightno)
                        {
                            TargetFlight = a;
                            TargetAirline = s;
                            break;
                        }
                    }
                    if (TargetFlight != null) break;
                }
            }
            Flight newflight = ModifySpecialRequestCode(TargetFlight, code);
            TargetAirline.Removeflight(TargetFlight);
            TargetAirline.Addflight(newflight);
            Console.WriteLine($"FlightNo: {newflight.FlightNumber}\nAirline Name :{TargetAirline.Name}\nOrigin:{newflight.Origin}\nDestination:{newflight.Destination}\nExpected Departure/Arrival Time: {newflight.ExpectedTime}\nStatus: {newflight.Status}\nSpecial Request Code:{newflight.GetType().Name.Substring(0, 4)}\nBoarding Gate:Unassigned");
        }
        if (option2 == "4")
        {
            Console.Write("Enter a new boarding gate: ");
            string gate = Console.ReadLine() ;
            Flight TargetFlight = null;
            foreach (Airline s in Terminal.Airlines.Values)
            {
                if (s.Code == flightno.Substring(0, 2))
                {
                    foreach (Flight a in s.Flights.Values)
                    {
                        if (a.FlightNumber == flightno)
                        {
                            TargetFlight = a;
                            
                            break;
                        }
                    }
                    if (TargetFlight != null) break;
                }
            }
            foreach (BoardingGate g in Terminal.BoardingGates.Values)
            {
                if (g.GateName == gate)
                {
                    g.flight = TargetFlight;
                    break;
                }
                else
                {
                    Console.Write("Gate not found!");
                }
            }
        }
    }
}


//Advanced Features 
//A (Kai Sheng)

void BulkassignFlights(Dictionary<string , Flight> flightsdict , Dictionary<string,BoardingGate> BoardinggateDict,Dictionary<string,Airline> AirlineDict)
{
    Queue<Flight> UnassignedFlights 
}