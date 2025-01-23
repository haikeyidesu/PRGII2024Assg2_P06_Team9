using System;

public class Flight
{
	public string FlightName { get; set; }
	public string Origin { get; set; }
	public string Destination { get; set; }
	public DateTime ExpectedTime { get; set; }
	public string Status { get; set; }

	public abstract double CalculateFees();


	public Flight (string Flightname , string origin  , string dest, DateTime Expectedtime, string status )
	{
		this.FlightName = Flightname;
		this.Origin = origin;
		this.Destination = dest;
		this.ExpectedTime = Expectedtime;
		this.Status = status;
	}
	public Flight() { }	
}
