using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTDemo
{
    class Program
    {

        
        private const string DeviceConnectionString = "HostName=JBJIOTDemo.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=023jbSZ+uZpdk/o5eQZI+fiSgdeQpsKhXSPKMwxpYOg=";
        //HostName=JBJIOTDemo.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=023jbSZ+uZpdk/o5eQZI+fiSgdeQpsKhXSPKMwxpYOg=
        static ServiceClient serviceClient;
        static void Main(string[] args)
        {
            serviceClient = ServiceClient.CreateFromConnectionString(DeviceConnectionString);
            Program program = new Program();

            /*Console.WriteLine("Send Cloud-to-Device message\n");
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

            Console.WriteLine("Press any key to send a C2D message.");
            Console.ReadLine();
            SendCloudToDeviceMessageAsync().Wait();
            Console.ReadLine();*/

        }

        /*private async static Task SendCloudToDeviceMessageAsync()
        {
            var commandMessage = new
             Message(Encoding.ASCII.GetBytes("Cloud to device message."));
            await serviceClient.SendAsync(targetDevice, commandMessage);
        }*/

        public Program()
        {
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=JBJIOTDemo.azure-devices.net;DeviceId=MyDevice;SharedAccessKey=0VhEHzNXP5Hf8pwdTIoRU8isYQIJIwh4bV6cowWz6zs=");
            SendEvent().Wait();
            ReceiveCommands(deviceClient).Wait();
            //HostName=JBJIOTDemo.azure-devices.net;DeviceId=MyDevice;SharedAccessKeyName=iothubowner;SharedAccessKey=023jbSZ+uZpdk/o5eQZI+fiSgdeQpsKhXSPKMwxpYOg=
        }

        //This method is responsible for sending the Event to the IoT Hub
        static async Task SendEvent()
        {
            //This is a static message the we send to the IoT Hub once the application is launched
            //You can use Device Explorer on your laptop to send the message you want to the IoTHub
            //Make sure to have the right device ID written in device explorer
            string dataBuffer = "IoT in 90 Seconds";
            Microsoft.Azure.Devices.Message eventMessage = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(dataBuffer));
            await serviceClient.SendAsync("MyDevice", eventMessage);
        }

        //This method is responsible to receive the message on the IoT Hub
        async Task ReceiveCommands(DeviceClient deviceClient)
        {
            Console.WriteLine("\n Device waiting for IoT Hub command....\n");
            Microsoft.Azure.Devices.Client.Message receivedMessage;
            string messageData;
            while (true)
            {
                receivedMessage = await deviceClient.ReceiveAsync(TimeSpan.FromSeconds(1));
                if (receivedMessage != null)
                {
                    messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("\t{0}> Message received: {1}", DateTime.Now.ToLocalTime(), messageData);
                    await deviceClient.CompleteAsync(receivedMessage);
                }
            }
        }
    }
}
