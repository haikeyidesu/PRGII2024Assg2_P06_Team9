using System.IO;
using PRG2_ASSIGNMENT;

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