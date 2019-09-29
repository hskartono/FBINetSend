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

		static void showHelp()
		{
			Console.WriteLine("");
			Console.WriteLine("Syntax:");
			Console.WriteLine("  FBINetSend 3ds_ip_address http://baseurl/path/to/cia/folder local_folder_to_cia");
			Console.WriteLine("  FBINetSend 3ds_ip_address http://baseurl/path/to/cia/folder filename.cia");
			Console.WriteLine("");
			Console.WriteLine("Example:");
			Console.WriteLine("  FBINetSend 192.168.2.24 http://192.168.2.23/3ds/cps1 c:/inetpub/wwwroot/3ds/cps1");
			Console.WriteLine("  FBINetSend 192.168.2.24 http://192.168.2.23/3ds/cps1 c:/inetpub/wwwroot/3ds/cps1/Cadillac_and_Dinosaurs.cia");
			Console.WriteLine("");
		}

		// how to use
		// fbinetsend 3ds_ip_address http://baseurl/path local_folder_name
        static void Main(string[] args)
        {
            try
            {
				string n3ds_ip_address = "";
				string base_url = "";
				string base_path = "";

				if (args.Length == 0)
				{
					Console.Write("Enter 3DS IP Address: ");
					n3ds_ip_address = Console.ReadLine();

					Console.Write("Enter Base URL: ");
					base_url = Console.ReadLine();

					Console.Write("Enter cia folder/filename: ");
					base_path = Console.ReadLine();

					Console.WriteLine("Information collected:");
					Console.WriteLine("3DS IP Address: {0}", n3ds_ip_address);
					Console.WriteLine("Base URL: {0}", base_url);
					Console.WriteLine("Base Path/Filename: {0}", base_path);
					Console.WriteLine("Press any key to proceed.");
					Console.ReadKey();
				} else if(args.Length != 3)
				{
					Program.showHelp();
					return;
				} else
				{
					n3ds_ip_address = args[0];
					base_url = args[1];
					base_path = args[2];
				}

				String message = "";

				if (!String.IsNullOrEmpty(System.IO.Path.GetFileName(base_path)))
				{
					// check if file exists
					if (!System.IO.File.Exists(base_path))
					{
						Console.WriteLine("File {0} not exists. Please re-check your entry.", base_path);
						Program.showHelp();
						return;
					}

					Console.WriteLine("Found file: {0}", base_path);
					message = base_url + System.IO.Path.GetFileName(base_path) + "\n";
				}
				else
				{
					// check if folder exists
					if (!System.IO.Directory.Exists(base_path))
					{
						Console.WriteLine("Path {0} not exists. Please re-check your entry.", base_path);
						Program.showHelp();
						return;
					}

					// check if there are .cia files
					string[] cia_files = System.IO.Directory.GetFiles(base_path, "*.cia");
					if (cia_files.Length <= 0)
					{
						Console.WriteLine("There is no .cia files in folder {0}", base_path);
						Program.showHelp();
						return;
					}

					// collect all .cia files
					Console.WriteLine("Scanning .cia files...");
					foreach (string cia_filename in cia_files)
					{
						Console.WriteLine("Found file: {0}", cia_filename);
						message += base_url + System.IO.Path.GetFileName(cia_filename) + "\n";
					}
				}
				

				
				
				// message += "http://192.168.1.20/3ds/Captain_Toad_Treasure_Tracker_USA.cia\n";
				// message += "http://192.168.1.17/file2.cia\n";
				// message += "http://192.168.1.17/file3.cia\n";

				Console.Write("Connecting to 3ds...");
				fbiSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IAsyncResult result = fbiSocket.BeginConnect(n3ds_ip_address, 5000, null, null);
                result.AsyncWaitHandle.WaitOne(5000, true);

                if (!fbiSocket.Connected)
                {
                    fbiSocket.Close();
                    Console.WriteLine("Failed");
                    return;
                }

				Console.WriteLine("Connected");

				Console.WriteLine("Calculating files...");

                uint addressLength = (uint) Encoding.ASCII.GetBytes(message).Length;
                byte[] addressSize = BitConverter.GetBytes(addressLength);
                byte[] address = Encoding.ASCII.GetBytes(message);

                Array.Reverse(addressSize);
                fbiSocket.Send(MergeAsPayload(addressSize, address));

                Console.WriteLine("Sending files...");

                // fbiSocket.BeginReceive(new byte[1], 0, 1, 0, new AsyncCallback(GotData), null);

                Console.WriteLine("done");
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
