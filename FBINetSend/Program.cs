using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace FBINetSend
{
    class Program
    {
        static Socket s = null;

        static void Main(string[] args)
        {
            try
            {
                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IAsyncResult result = s.BeginConnect("192.168.1.18", 5000, null, null);
                result.AsyncWaitHandle.WaitOne(5000, true);

                if (!s.Connected)
                {
                    s.Close();
                    Console.WriteLine("Failed to connect to 3DS");
                    return;
                }

                Console.WriteLine("Calculating file...");

                String message = "";

                message += "http://192.168.1.17/file1.cia\n";
                message += "http://192.168.1.17/file2.cia\n";
                message += "http://192.168.1.17/file3.cia\n";

                byte[] Largo = BitConverter.GetBytes((uint)Encoding.ASCII.GetBytes(message).Length);
                byte[] address = Encoding.ASCII.GetBytes(message);

                Array.Reverse(Largo);
                s.Send(AppendTwoByteArrays(Largo, address));

                Console.WriteLine("Sending file...");

                // s.BeginReceive(new byte[1], 0, 1, 0, new AsyncCallback(GotData), null);

                Console.WriteLine("done");
                Console.ReadKey();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        static byte[] AppendTwoByteArrays(byte[] arrayA, byte[] arrayB) //Aux function to append the 2 byte arrays.
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
