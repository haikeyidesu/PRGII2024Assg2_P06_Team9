using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Transactions;
using PRG2_ASSIGNMENT;
//==========================================================
// Student Number	: S10268302
// Student Name	: Mah Kai Sheng
// Partner Name	: Asher Ng
//==========================================================

void displaymenu()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Welcome to Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
    Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights");
    Console.WriteLine("6. Modify Flight Details");
    Console.WriteLine("7. Display Flight Schedule");
    Console.WriteLine("8. (ADV) Assign Flights to boarding gates");
    Console.WriteLine("0. Exit");
}
//1(Kai Sheng)
Terminal Terminal = new Terminal("T5");
using (StreamReader sr = new StreamReader("airlines.csv"))
{
    string header = sr.ReadLine();
    string line;
    int count = 0;
    while ((line = sr.ReadLine()) != null)
    {
        string[] temp = line.Split(',');
        Airline s = new Airline(temp[0],temp[1]);
        Terminal.Addairline(s);
        count += 1;
    }
    Console.WriteLine("Loading Airlines...");
    Console.WriteLine($"{count} Airlines Loaded!");
}

using(StreamReader sr = new StreamReader("boardinggates.csv")) 
{
    string header = sr.ReadLine() ;
    string line;
    int count = 0;
    while ((line = sr.ReadLine()) != null)
    {
        string[] temp = line.Split(",");
        BoardingGate s = new BoardingGate(temp[0], Convert.ToBoolean(temp[1]), Convert.ToBoolean(temp[2]), Convert.ToBoolean(temp[3]), null);
        Terminal.AddBoardingGate(s);
        count += 1;
    }
    Console.WriteLine("Loading Boarding Gates...");
    Console.WriteLine($"{count} Boarding Gates Loaded!");
}


//2 

using (StreamReader sw = new StreamReader("flights.csv"))
{
    string header = sw.ReadLine();
    string line;
    int count = 0;
    while ((line = sw.ReadLine()) != null)
    {
        string[] temp = line.Split(",");
        if (temp[4] == "CFFT")
        {
            Flight s = new CFFTFlight(150, temp[0], temp[1], temp[2], Convert.ToDateTime(temp[3]), "Unassigned");
            Airline a = Terminal.GetAirlineFromFlight(s);
            a.Addflight(s);
            Terminal.Flights.Add(temp[0], s);
        }
        else if (temp[4] == "DDJB")
        {
            Flight s = new DDJBFlight(300, temp[0], temp[1], temp[2], Convert.ToDateTime(temp[3]), "Unassigned");
            Airline a = Terminal.GetAirlineFromFlight(s);
            a.Addflight(s);
            Terminal.Flights.Add(temp[0], s);
        }
        else if (temp[4] == "LWTT")
        {
            Flight s = new LWTTFlight(500, temp[0], temp[1], temp[2], Convert.ToDateTime(temp[3]), "Unassigned");
            Airline a = Terminal.GetAirlineFromFlight(s);
            a.Addflight(s);
            Terminal.Flights.Add(temp[0], s);
        }
        else if (string.IsNullOrWhiteSpace(temp[4]))
        {
            Flight s = new NORMFlight(temp[0], temp[1], temp[2], Convert.ToDateTime(temp[3]), "Unassigned");
            Airline a = Terminal.GetAirlineFromFlight(s);
            a.Addflight(s);
            Terminal.Flights.Add(temp[0], s);
        }
        count += 1;
    }
    Console.WriteLine("Loading Flights...");
    Console.WriteLine($"{count} Flights Loaded!");
}

//4(Kai sheng)
void ListAllGates()
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

//7(Kai sheng)
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
string? DetailsFromairlines()
{
    try
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
        string airlinecode = Console.ReadLine().ToUpper();

        if (checkairline(airlinecode) == true)
        {
            Airline s = Terminal.Airlines[airlinecode];
            Console.WriteLine("=============================================");
            Console.WriteLine($"List of Flights for {s.Name}");
            Console.WriteLine("=============================================");
            Console.WriteLine($"{"Flight Number",-15}{"Airline name",-25}{"Origin",-20}{"Detination",-20}{"Expected Departure/Arrival time",-30}");
            foreach (KeyValuePair<string, Flight> kvp in s.Flights)
            {
                Console.WriteLine($"{kvp.Key,-15}{s.Name,-25}{kvp.Value.Origin,-20}{kvp.Value.Destination,-20}{kvp.Value.ExpectedTime,-30}{kvp.Value.GetType().Name}");
            }
            return airlinecode;
        }
        else
        {
            Console.WriteLine("Enter a valid aircode!");
            return null;
        }
    }
    catch
    {
        Console.WriteLine("Please Enter a valid airline code ! ");
        return null;
    }
}
//8(Kai Sheng)
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
    try
    {
        string? airlinecode = DetailsFromairlines();
        if (airlinecode == null) throw new Exception("Please try again!");
        Console.Write("Choose an existing Flight to modify or delete: ");
        string flightno = Console.ReadLine();
        if (checkairline(airlinecode) == true)
        {
            bool condition = false;
            Airline s = Terminal.Airlines[airlinecode];
            foreach (string a in s.Flights.Keys)
            {
                if (a == flightno)
                {
                    condition = true;

                }

            }
            if (condition==false) throw new Exception("Invalid code entered!");
        }
        Console.WriteLine("1.Modify Flight");
        Console.WriteLine("2.Delete Flight");
        Console.Write("Choose an option: ");
        string option = Console.ReadLine();
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
                string stat = Console.ReadLine();
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
                string code = Console.ReadLine();
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
                TargetAirline.Removeflight(TargetFlight.FlightNumber);
                TargetAirline.Addflight(newflight);
                Console.WriteLine($"FlightNo: {newflight.FlightNumber}\nAirline Name :{TargetAirline.Name}\nOrigin:{newflight.Origin}\nDestination:{newflight.Destination}\nExpected Departure/Arrival Time: {newflight.ExpectedTime}\nStatus: {newflight.Status}\nSpecial Request Code:{newflight.GetType().Name.Substring(0, 4)}\nBoarding Gate:Unassigned");
            }
            if (option2 == "4")
            {
                Console.Write("Enter a new boarding gate: ");
                
                string gate = Console.ReadLine();
                if (gate != null)
                {
                    bool condition = false;
                    foreach (string s in Terminal.BoardingGates.Keys)
                    {
                        if (s == gate)
                        {
                            condition = true;
                        }
                    }
                    if (condition = false)
                    {
                        throw new Exception("Boarding Gate Not Found.");
                    }
                }
                else { throw new Exception("Please enter a code!"); }
              
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
                    
                }
            }
        }
        else if (option == "2")
        {
            Airline s = Terminal.Airlines[airlinecode];
            s.Removeflight(flightno);
        }
    }
    
    catch(FormatException e)
    {
        Console.WriteLine("Please follow the required format!");
    }
    catch (Exception E)
    {
        Console.WriteLine(E.Message);
    }
    catch
    {
        Console.WriteLine("Please enter a valid input!");
    }

}

//9 
void SortedFlightInfo(Dictionary<String, Flight> flighdict, Dictionary<string, Airline> airlinesDictionary, Dictionary<String, BoardingGate> boardingatedict)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected Departure/Arrival Time",-40}{"Status",-17}{"Boarding Gate",-23}");
    List<Flight> Sortlis = new List<Flight>(flighdict.Values);
    Sortlis.Sort();
    foreach (Flight kv in Sortlis)
    {

        string AirlineName = "";
        string BoardinGate = "";
        string flightcode = kv.FlightNumber.Substring(0, 2);
        foreach (KeyValuePair<String, Airline> ad in airlinesDictionary)
        {
            if (ad.Value.Code == flightcode) { AirlineName = ad.Value.Name; break; }
        }
        foreach (KeyValuePair<String, BoardingGate> bg in boardingatedict)
        {
            if (bg.Value.flight != null && bg.Value.flight.FlightNumber == kv.FlightNumber) { BoardinGate = bg.Key; break; }
            else { BoardinGate = "Unassigned"; }
        }
        if (String.IsNullOrWhiteSpace(kv.Status) == false)
        { Console.WriteLine($"{kv.FlightNumber,-16}{AirlineName,-23}{kv.Origin,-23}{kv.Destination,-23}{kv.ExpectedTime,-40}{kv.Status,-17}{BoardinGate,-23}"); }
        else { Console.WriteLine($"{kv.FlightNumber,-16}{AirlineName,-23}{kv.Origin,-23}{kv.Destination,-23}{kv.ExpectedTime,-40}{"None",-17}{BoardinGate,-23}"); }
    }
}


//Advanced Features 
//A (Kai Sheng)

void BulkassignFlights(Dictionary<string, Flight> flightsdict, Dictionary<string, BoardingGate> BoardinggateDict, Dictionary<string, Airline> AirlineDict)
{
    Queue<Flight> UnassignedFlights = new Queue<Flight>();
    List<Flight> AssignedFlights = new List<Flight>();
    int UnassignedBoardingGateCount = 0;
    foreach (KeyValuePair<string, BoardingGate> kvp in BoardinggateDict)
    {
        if (kvp.Value.flight != null)
        {
            AssignedFlights.Add(kvp.Value.flight);
        }
        else
        {
            UnassignedBoardingGateCount += 1;
        }
    }
    foreach (KeyValuePair<string, Flight> flightval in flightsdict)
    {
        if (!AssignedFlights.Contains(flightval.Value))//checks if the flight inside flight dictionary is NOT inside Assigned flights list
        {
            UnassignedFlights.Enqueue(flightval.Value);//if its NOT inside the dictionary , enqueue it
        }
    }
    Console.WriteLine($"{UnassignedFlights.Count()} Flights have not been assigned to a boarding gate.");
    Console.WriteLine($"{UnassignedBoardingGateCount} Boardings Gates do not have a flight assigned to them.");

    int AssignedCount = 0;
    foreach (Flight flight in UnassignedFlights)
    {
        string code = flight.GetType().Name.Substring(0, 4);

        Dictionary<string, List<BoardingGate>> UnassignedBoardingGates = new Dictionary<string, List<BoardingGate>>();
        List<BoardingGate> NORMFlightList = new List<BoardingGate>();
        List<BoardingGate> DDJBFlightList = new List<BoardingGate>();
        List<BoardingGate> LWTTFlightList = new List<BoardingGate>();
        List<BoardingGate> CFFTFlightList = new List<BoardingGate>();
        foreach (KeyValuePair<string, BoardingGate> s in BoardinggateDict)
        {
            if (s.Value.SupportsCFFT == false && s.Value.SupportsLWTT == false && s.Value.SupportsDDJB == false)
            {
                NORMFlightList.Add(s.Value);
                UnassignedBoardingGates["NORM"] = NORMFlightList;
            }
            if (s.Value.SupportsDDJB == true) { DDJBFlightList.Add(s.Value); UnassignedBoardingGates["DDJB"] = DDJBFlightList; }
            if (s.Value.SupportsCFFT == true) { CFFTFlightList.Add(s.Value); UnassignedBoardingGates["CFFT"] = CFFTFlightList; }
            if (s.Value.SupportsLWTT == true) { LWTTFlightList.Add(s.Value); UnassignedBoardingGates["LWTT"] = LWTTFlightList; }
        }

        foreach (BoardingGate s in UnassignedBoardingGates[code])
        {
            if (s.flight == null)
            {
                BoardinggateDict[s.GateName].flight = flight;   //properly assigning them to the global boardinggatedictionary
                break;
            }
        }
        AssignedCount += 1;
    }
    SortedFlightInfo(flightsdict, AirlineDict, BoardinggateDict);
    Console.WriteLine($"{AssignedCount} Flights and Boarding Gates were processed and assigned");

    double AutoVsBefore = ((double)AssignedCount / flightsdict.Count) * 100;
    Console.WriteLine($"{AutoVsBefore:F2}% of Flights and Boarding Gates were processed automatically over those that were already assigned");
}

    //Main Program to be executed
    bool condition = true;
while (condition)
{
    try
    {
        displaymenu();
        Console.Write("Please Select your option: ");
        int option = Convert.ToInt32(Console.ReadLine());
        if (option < 0 || option > 8) throw new Exception("Enter a valid option from 0 to 8!");
        if (option == 2)
        {
            ListAllGates();
        }
        else if(option == 5)
        {
            DetailsFromairlines();
        }
        else if (option == 6)
        {
            ModifyFlightDetails();
        }
        else if (option == 8)
        {
            BulkassignFlights(Terminal.Flights,Terminal.BoardingGates,Terminal.Airlines);
        }
        else if (option == 0) 
        {
            condition = false;
            Console.WriteLine("Bye bye !");
        }
    }
   
    catch(FormatException)
    {
        Console.WriteLine("Please enter a integer!");
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}