using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace FBINetSend
{
    class Program
    {
        static Socket fbiSocket = null;

        static void Main(string[] args)
        {
            try
            {
                fbiSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IAsyncResult result = fbiSocket.BeginConnect("192.168.1.18", 5000, null, null);
                // result.AsyncWaitHandle.WaitOne(5000, true);

                if (!fbiSocket.Connected)
                {
                    fbiSocket.Close();
                    Console.WriteLine("Failed to connect to 3DS");
                    return;
                }

                Console.WriteLine("Calculating file...");

                String message = "";

                message += "http://192.168.1.17/file1.cia\n";
                message += "http://192.168.1.17/file2.cia\n";
                message += "http://192.168.1.17/file3.cia\n";

                uint addressLength = (uint) Encoding.ASCII.GetBytes(message).Length;
                byte[] addressSize = BitConverter.GetBytes(addressLength);
                byte[] address = Encoding.ASCII.GetBytes(message);

                Array.Reverse(addressSize);
                fbiSocket.Send(MergeAsPayload(addressSize, address));

                Console.WriteLine("Sending file...");

                // fbiSocket.BeginReceive(new byte[1], 0, 1, 0, new AsyncCallback(GotData), null);

                Console.WriteLine("done");
                Console.ReadKey();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        static byte[] MergeAsPayload(byte[] arrayA, byte[] arrayB) //Aux function to append the 2 byte arrays.
        {
            byte[] outputBytes = new byte[arrayA.Length + arrayB.Length];
            Buffer.BlockCopy(arrayA, 0, outputBytes, 0, arrayA.Length);
            Buffer.BlockCopy(arrayB, 0, outputBytes, arrayA.Length, arrayB.Length);
            return outputBytes;
        }

        /*
        private static void GotData(IAsyncResult ar)
        {
            Console.WriteLine("Operation completed");
            s.Close();
            Console.ReadKey();
        }
        */
    }
}
