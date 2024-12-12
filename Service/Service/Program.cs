// See https://aka.ms/new-console-template for more information
using Service;


Console.WriteLine("Please enter the com number:");
var com = "Com" + Console.ReadLine();

// Start the SerialPortService
SerialPortService service = new(com, 9600);
service.Start();

Console.ReadKey();