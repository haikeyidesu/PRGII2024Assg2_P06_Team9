using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
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

//2 (Asher)
/*
// create dictionary to store flights
Dictionary<string,Flight> flightDict = new Dictionary<string,Flight>();
using (StreamReader sr = new StreamReader("flights.csv"))
{
    // read header first
    sr.ReadLine();
    while (!sr.EndOfStream && sr.ReadLine()!=null)
    {
        string[] flightDetails = sr.ReadLine().Split(",");

        // convert format of flight properties
        string flightNumber = flightDetails[0];
        string origin = flightDetails[1];
        string destination = flightDetails[2];
        // use try to catch error converting string to datetime
        try
        {
            DateTime expectedTime = Convert.ToDateTime(flightDetails[3]);
            string specialRequestCode = flightDetails[4];
        } catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine($"There was an error getting datetime for flight {flightNumber} from flights.csv. ");
        }

        // create flight object based on special requestcode
        // default flight status is "On Time"
        Flight newFlight;
        string status = "On Time";
        if (specialRequestCode == "CFFT")
        {
            newFlight = new CFFTFlight(150, flightNumber, origin, destination, expectedTime, status);
        }
        else if (specialRequestCode == "DDJB")
        {
            newFlight = new DDJBFlight(300, flightNumber, origin, destination, expectedTime, status);
        }
        else if (specialRequestCode == "LWTT")
        {
            newFlight = new LWTTFlight(500, flightNumber, origin, destination, expectedTime, status);
        }
        else
        {
            newFlight = new NORMFlight(flightNumber, origin, destination, expectedTime, status);
        }

        // add new flight to flight dicitonary
        flightDict.Add(newFlight.FlightNumber,newFlight);
    }
}
*/

//2 (Kai SHeng)

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
            // add flight to Flights dictionary in terminal
            Terminal.Flights.Add(temp[0], s);
        }
        else if (temp[4] == "DDJB")
        {
            Flight s = new DDJBFlight(300, temp[0], temp[1], temp[2], Convert.ToDateTime(temp[3]), "Unassigned");
            Airline a = Terminal.GetAirlineFromFlight(s);
            a.Addflight(s);
            // add flight to Flights dictionary in terminal
            Terminal.Flights.Add(temp[0], s);
        }
        else if (temp[4] == "LWTT")
        {
            Flight s = new LWTTFlight(500, temp[0], temp[1], temp[2], Convert.ToDateTime(temp[3]), "Unassigned");
            Airline a = Terminal.GetAirlineFromFlight(s);
            a.Addflight(s);
            // add flight to Flights dictionary in terminal
            Terminal.Flights.Add(temp[0], s);
        }
        else if (string.IsNullOrWhiteSpace(temp[4]))
        {
            Flight s = new NORMFlight(temp[0], temp[1], temp[2], Convert.ToDateTime(temp[3]), "Unassigned");
            Airline a = Terminal.GetAirlineFromFlight(s);
            a.Addflight(s);
            // add flight to Flights dictionary in terminal
            Terminal.Flights.Add(temp[0], s);
            
        }
        count += 1;
    }
    Console.WriteLine("Loading Flights...");
    Console.WriteLine($"{count} Flights Loaded!");
}

//3 (Asher)
void ListAllFlights(Terminal terminal)
{
    Dictionary<string,Flight> flightDict = terminal.Flights;
    Dictionary<string, Airline> airlineDict = terminal.Airlines;
    // list all flight with their basic info
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    // display header
    Console.WriteLine($"{"Flight Number",-14}{"Airline Name",-21}{"Origin",-20} {"Destination",-18}{"Expected Departure/Arrival Time"}");
    foreach (Flight flight in flightDict.Values)
    {
        // initialising variables
        string flightNo;
        string airlineName;
        string origin;
        string destination;
        string expectedTime;

        // getting values to variables
        flightNo = flight.FlightNumber;
        //airline name variable
        string airlineCode = flightNo.Substring(0, 2);
        airlineName = airlineDict[airlineCode].Name;
        origin = flight.Origin;
        destination = flight.Destination;
        expectedTime = flight.ExpectedTime.TimeOfDay.ToString();

        // displaying values
        Console.WriteLine($"{flightNo,-14}{airlineName,-21}{origin,-20} {destination,-18}{expectedTime}");
    }
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

//5 (Asher)
void AssignFlightBoardingGate(Terminal terminal)
{
    // get dictionaries for easy reference
    Dictionary<string, Flight> flightDict = terminal.Flights;
    Dictionary<string, Airline> airlineDict = terminal.Airlines;
    Dictionary<string, BoardingGate> boardingGateDict = terminal.BoardingGates;

    // ask user for flight number
    // loops until an existing flight number has been inputed
    // example flight number: SQ 115
    bool flightFound = false;
    string userFlight = "";
    while (!flightFound)
    {
        Console.Write("Enter Flight Number: ");
        userFlight = Console.ReadLine().ToUpper();
        foreach(string flightNo in flightDict.Keys)
        {
            if (userFlight == flightNo)
            {
                flightFound = true;
                break;
            }
        }
        // breaks loop if flight number was found
        // otherwise, continues loop and asks for flight number again
        if (flightFound)
        {
            break;
        }
        Console.WriteLine($"Flight number {userFlight} could not be found. ");
    }

    // display basic info of selected flight
    Console.WriteLine("====================================");
    Console.WriteLine($"Basic Information for flight {userFlight}");
    Console.WriteLine("====================================");
    // display header
    Console.WriteLine($"{"Flight Number",-21}{"Airline Name",-21}{"Origin",-18} {"Destination",-18}{"Expected Departure/Arrival Time"}");
    // get flight object
    Flight selectedFlight = flightDict[userFlight];
    // get flight airline name
    string airlineCode = userFlight.Substring(0, 2);
    string airlineName = airlineDict[airlineCode].Name;
    // get flight's special request
    // CFFT, DDJB, LWTT
    string specialRequest = "-";
    bool requestCFFT = false;
    bool requestDDJB = false;
    bool requestLWWT = false;

    if (selectedFlight is CFFTFlight)
    {
        requestCFFT = true;
        specialRequest = "CFFT";
    }
    if (selectedFlight is DDJBFlight)
    {
        requestDDJB = true;
        specialRequest = "DDJB";
    }
    if (selectedFlight is LWTTFlight)
    {
        requestLWWT = true;
        specialRequest = "LWTT";
    }

    // displaying info
    Console.WriteLine($"{selectedFlight.FlightNumber,-21}{airlineName,-21}{selectedFlight.Origin,-18} {selectedFlight.Destination,-18}{selectedFlight.ExpectedTime}");
    Console.WriteLine("Special Request");
    Console.WriteLine($"{specialRequest}");
    // prompt user for the boarding gate
    // loops until boarding gate is successfully assigned to flight
    // example boarding gate
    bool successfulAssignment = false;
    string assignedBoardingGate = "";
    while (! successfulAssignment)
    {
        // checks
        bool boardingGateFound = false;
        bool boardingGateAssigned = false;
        bool supportsRequest = false;

        // get boarding gate
        Console.Write("Enter boarding gate: ");
        string userBoardingGate = Console.ReadLine().ToUpper();

        // check if boarding gate can be found
        foreach (string boardingGateName in boardingGateDict.Keys) {
            if (userBoardingGate == boardingGateName)
            {
                boardingGateFound = true;
                break;
            }
        }
        // have user input boarding gate again if boarding gate cannot be found
        // otherwise, continue checks
        if (! boardingGateFound)
        {
            Console.WriteLine($"Boarding gate {userBoardingGate} could not be found. ");
            continue;
        }

        // check if boarding gate has already been assigned to another flight
        // get boarding gate object
        BoardingGate boardingGate = boardingGateDict[userBoardingGate];
        // if boarding gate flight is not null, the gate has already been assigned to a flight

        if (boardingGate.flight != null)
        {
            boardingGateAssigned = true;
        }
        // ask user to enter boarding gate again if boarding gate has already been assigned
        if (boardingGateAssigned)
        {
            Console.WriteLine($"Boarding gate {userBoardingGate} has already been assigned to flight {boardingGate.flight.FlightNumber}. ");
            continue;
        }

        // check request later
        supportsRequest = true;

        // ask user to enter boarding gate again if it does not support the flight's request
        // otherwise, boarding gate is assigned successfully
        if (! supportsRequest)
        {
            Console.WriteLine($"Boarding gate {userBoardingGate} does not support flight {userFlight}. ");
            continue;
        }
        assignedBoardingGate = userBoardingGate;
        successfulAssignment = true;
    }
    // display basic info of flight + boarding gate
    Console.WriteLine("====================================");
    Console.WriteLine($"Basic Information for flight {userFlight}");
    Console.WriteLine("====================================");
    // display header
    Console.WriteLine($"{"Flight Number",-21}{"Airline Name",-21}{"Origin",-18} {"Destination",-18}{"Expected Departure/Arrival Time",-24}");
    Console.WriteLine($"{selectedFlight.FlightNumber,-21}{airlineName,-21}{selectedFlight.Origin,-18} {selectedFlight.Destination,-18}{selectedFlight.ExpectedTime,-24}");
    Console.WriteLine($"{"Special Request",-18}Boarding Gate");
    Console.WriteLine($"{specialRequest,-18}{assignedBoardingGate}");

    // prompt user to update status of selected flight
    // get flight status
    string userOption = "";
    while (true)
    {
        Console.WriteLine($"The current status of flight {selectedFlight.FlightNumber} is {selectedFlight.Status}. ");
        Console.WriteLine("Would you like to change it? [Y/N]");
        Console.Write("Enter option: ");
        userOption = Console.ReadLine().ToUpper();
        if (userOption == "Y" || userOption == "N")
        {
            break;
        }
    }
    if (userOption == "Y")
    {
        Console.WriteLine($"Select the new status for flight {selectedFlight.FlightNumber}: ");
        Console.WriteLine("1. Delayed");
        Console.WriteLine("2. Boarding");
        Console.WriteLine("3. On Time");
        // ask user for new status
        // loops until valid option is inputed
        string userStatusOption = "";
        while (true)
        {
            Console.Write("Enter option: ");
            userStatusOption = Console.ReadLine();
            if (userStatusOption == "1")
            {
                selectedFlight.Status = "Delayed";
                break;
            } else if (userStatusOption == "2")
            {
                selectedFlight.Status = "Boarding";
                break;
            } else if (userStatusOption == "3")
            {
                selectedFlight.Status = "On Time";
                break;
            }
        }
        Console.WriteLine($"The status of flight {selectedFlight.FlightNumber} has been successfully changed to {selectedFlight.Status}. ");
    }
}

//6 (Asher)
void CreateNewFlight(Terminal terminal)
{
    // list to store confirmation messages for each new flight made
    List<string> confirmationMessages = new List<string>();

    while (true)
    {
        // prompt user to enter the specifications for the new flight
        // with validation

        // flight number
        string flightNumber;
        //validate flight number

        while (true)
        {
            Console.Write("Enter flight number: ");
            flightNumber = Console.ReadLine();
            string[] flight;
            try
            {
                flight = flightNumber.Split(" ");
            } catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("The flight number format is wrong");
                continue;
            }
            string airlineCode = flight[0];
            if (!terminal.Airlines.Keys.ToList().Contains(airlineCode))
            {
                Console.WriteLine("The airline code in flight number is not valid");
                continue;
            }
            break;
        }

        // origin
        string origin = Console.ReadLine();
        // destination
        string destination = Console.ReadLine();
        // expectedTime
        string dateTime = Console.ReadLine();
        DateTime expectedTime = new DateTime();

        // status is set to "On Time" unless otherwise stated
        string status = "On Time";

        // ask for special request code
        // if yes, get special request code
        string specialRequestCode = Console.ReadLine();

        // create the flight object based on special request code 

        Flight newFlight;
        if (specialRequestCode == "CFFT")
        {
            newFlight = new CFFTFlight(150, flightNumber, origin, destination, expectedTime, status);
        }
        else if (specialRequestCode == "DDJB")
        {
            newFlight = new DDJBFlight(300, flightNumber, origin, destination, expectedTime, status);
        }
        else if (specialRequestCode == "LWTT")
        {
            newFlight = new LWTTFlight(500, flightNumber, origin, destination, expectedTime, status);
        }
        else
        {
            newFlight = new NORMFlight(flightNumber, origin, destination, expectedTime, status);
        }

        // append newFlight to Flight dictionary in Terminal
        // get Flight dictionary from terminal
        terminal.Flights.Add(newFlight.FlightNumber, newFlight);

        // append newFlight details to flight.csv
        using (StreamWriter sw = new StreamWriter("flight.csv", true))
        {
            // append new line to the csv file
            sw.WriteLine($"{flightNumber},{origin},{destination},{expectedTime},{specialRequestCode}");
        }

        // store confirmation message
        confirmationMessages.Add($"Flight {flightNumber} successfully made!");

        // ask user if they want to add more flights
        // if user does not say yes, break loop by default
        Console.Write("Do you want to add another flight? [Y/N]");
        string continueLoop = Console.ReadLine().ToUpper();
        if ( continueLoop != "Y")
        {
            break;
        }
    }

    // display confirmation message
    foreach (string confirmationMessage in confirmationMessages)
    {
        Console.WriteLine(confirmationMessage);
    }
    Console.WriteLine("All flights have been successfully added1");
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
void DetailsFromairlines()
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
    catch
    {
        Console.WriteLine("Please Enter a valid airline code ! ");
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
        DetailsFromairlines();
        Console.Write("Choose an existing Flight to modify or delete: ");
        string flightno = Console.ReadLine();
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
                TargetAirline.Removeflight(TargetFlight);
                TargetAirline.Addflight(newflight);
                Console.WriteLine($"FlightNo: {newflight.FlightNumber}\nAirline Name :{TargetAirline.Name}\nOrigin:{newflight.Origin}\nDestination:{newflight.Destination}\nExpected Departure/Arrival Time: {newflight.ExpectedTime}\nStatus: {newflight.Status}\nSpecial Request Code:{newflight.GetType().Name.Substring(0, 4)}\nBoarding Gate:Unassigned");
            }
            if (option2 == "4")
            {
                Console.Write("Enter a new boarding gate: ");
                string gate = Console.ReadLine();
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
    catch(FormatException e)
    {
        Console.WriteLine(e.Message);
        Console.WriteLine("Please Enter the correct type of data!");
    }
    catch
    {
        Console.WriteLine("Please enter a valid input!");
    }

}

//9 (Asher)
/*
void DisplaySortedScheduledFlights(Terminal terminal)
{
    // get flight, airlines and boarding gate dictionary
    Dictionary<string, Flight> flightDict = terminal.Flights;
    Dictionary <string, Airline> airlineDict = terminal.Airlines;
    Dictionary <string, BoardingGate> boardingGateDict = terminal.BoardingGates;

    // display all flights for  the day by earliest first
    // IComparable<T> interface is implemented in Flight.cs
    // display basic information of all flights

    List<Flight> flightList = flightDict.Values.ToList();
    // sort list using Icomparable interface
    flightList.Sort();

    // display header
    Console.WriteLine($"{"Flight Number",-16}{"Origin",-21}{"Destination",-21}{"Expected Time",-24}{"Special Request Code",-24}{"Boarding Gate",-5}");
    // display flight details
    foreach (Flight flight in flightList)
    {
        // store flight details in variables for display
        string flightNo = flight.FlightNumber;
        string origin = flight.Origin;
        string dest = flight.Destination;
        DateTime eta = flight.ExpectedTime.ToUniversalTime();
        string requestCode = "-";
        if (flight is CFFTFlight)
        {
            requestCode = "CFFT";
        } else if (flight is DDJBFlight)
        {
            requestCode = "DJJB";
        } else if (flight is LWTTFlight)
        {
            requestCode = "DJJB";
        }
        // loop to find boarding gate assigned to flight
        string gateName = "-";
        foreach (BoardingGate gate in boardingGateDict.Values)
        {
            if (gate.flight != null && gate.flight.FlightNumber == flight.FlightNumber)
            {
                gateName = gate.GateName;
                break;
            }
        }

        // display all the details for each flight
        Console.WriteLine($"{flightNo,-16}{origin,-21}{dest,-21}{eta,-24}{requestCode,-24}{gateName,-5}");
    }
}
*/

//9 (Kai Sheng)
void SortedFlightInfo(Dictionary<String, Flight> flightdict, Dictionary<string, Airline> airlinesDictionary, Dictionary<String, BoardingGate> boardingatedict)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected Departure/Arrival Time",-40}{"Status",-17}{"Boarding Gate",-23}");
    List<Flight> Sortlis = new List<Flight>(flightdict.Values);
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
        if (option < 0 || option > 8) throw new Exception("Enter a valid option from 0 to 7!");
        if (option == 1) {
            ListAllFlights(Terminal);
        }
        else if (option == 2)
        {
            ListAllGates();
        }
        else if (option == 3)
        {
            AssignFlightBoardingGate(Terminal);
        }
        else if (option == 4)
        {
            CreateNewFlight(Terminal);
        }
        else if (option == 5)
        {
            DetailsFromairlines();
        }
        else if (option == 6)
        {
            ModifyFlightDetails();
        }
        else if (option == 8)
        {
            BulkassignFlights(Terminal.Flights, Terminal.BoardingGates, Terminal.Airlines);
        }
        else if (option == 0)
        {
            condition = false;
            Console.WriteLine("Bye bye !");
        }
    }
    catch(FormatException)
    {
        Console.WriteLine("Please enter a valid option!");
    }
}
