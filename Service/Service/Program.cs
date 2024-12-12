// See https://aka.ms/new-console-template for more information
using Service;


Console.WriteLine("Please enter the COM port number:");
var com = "Com" + Console.ReadLine();

Console.WriteLine("Please enter the server URL (e.g https://yourip-yourdomain.com):");
var serverUrl = Console.ReadLine();

// Start the SerialPortService
SerialPortService service = new(com, 9600, serverUrl ?? default);
service.Start();

Console.ReadKey();