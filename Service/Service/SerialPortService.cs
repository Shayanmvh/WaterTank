using Microsoft.AspNetCore.SignalR.Client;
using RJCP.IO.Ports;
using System.Reflection.Emit;

namespace Service
{
    public class SerialPortService
    {
        private readonly SerialPortStream _serialPort;
        private readonly Thread _t;
        private readonly HubConnection? _hubConnection;

        public SerialPortService(string com, int port, string serverUrl = "https://localhost:7090")
        {
            _serialPort = new SerialPortStream(com, port)
            {
                ReadTimeout = 500,
                WriteTimeout = 500
            };

            _t = new Thread(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    GetLastData();
                }
            })
            { IsBackground = true };

            // Create and start the SignalR connection
            _hubConnection = new HubConnectionBuilder()
                                .WithUrl($"{serverUrl}/messageHub")
                                .WithAutomaticReconnect()
                                .Build();
        }

        public void Start()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();

                _t.Start();

                Console.WriteLine($"Listening on {_serialPort.PortName}");
            }

            Console.WriteLine($"Connecting to UI socket...");

            try
            {
                _hubConnection?.StartAsync().Wait();

                Console.WriteLine($"Connected to socket successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Stop()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();

                if (_t is not null && _t.IsAlive)
                    _t.Interrupt();

                Console.WriteLine($"Stopped listening on {_serialPort.PortName}");
            }
        }

        public string GetLastData()
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    var data = _serialPort.ReadLine();

                    _hubConnection?.InvokeAsync("SendMessage", data);
                    //Console.WriteLine($"Data = {data}");
                    return data;
                }
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Read timeout.");
            }

            return string.Empty;
        }
    }
}
