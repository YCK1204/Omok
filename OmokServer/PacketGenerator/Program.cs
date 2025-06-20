using System.IO;
using System.Text;
using System.Windows;

namespace PacketGenerator
{
    public class Program
    {
        static string path = "./Protocol.fbs";
        static void Main(string[] args)
        {
            if (args.Length == 1)
                path = args[0];

            using (StreamReader sr = new StreamReader(path, encoding: Encoding.UTF8))
            {
                string ClientPacketStr = "";
                string ServerPacketStr = "";
                string clientRegister = "";
                string serverRegister = "";
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null)
                        break;
                    line = line.Trim();
                    if (line.IndexOf("PKT_Type") != -1)
                    {
                        while (true)
                        {
                            line = sr.ReadLine();
                            if (line == null || line.Equals("}"))
                                break;
                            line = line.Trim();
                            if (line.Length == 0)
                                continue;
                            if (line.Equals("}"))
                                break;
                            line = line.Substring(0, line.Length - 1);

                            if (line.StartsWith("PKT_S_"))
                            {
                                serverRegister += String.Format(PacketFormat_CSharp.PMRegister, line) + "\n\t\t";
                            }
                            else if (line.StartsWith("PKT_C_"))
                            {
                                clientRegister += String.Format(PacketFormat_CSharp.PMRegister, line) + "\n\t\t";
                            }
                            else if (line.StartsWith("//"))
                            {
                                continue;
                            }
                            else
                            {
                                Console.Error.WriteLine("Error : Packet Name is not valid");
                                List<string> list = null;
                                list.Add("");
                            }
                        }
                        break;
                    }
                }
                if (Directory.Exists("Client") == false)
                    Directory.CreateDirectory("Client");
                if (Directory.Exists("Server") == false)
                    Directory.CreateDirectory("Server");
                ClientPacketStr = String.Format(PacketFormat_CSharp.ClientPMTotal, serverRegister);
                File.WriteAllText("./Client/PacketManager.cs", ClientPacketStr);
                ServerPacketStr = String.Format(PacketFormat_CSharp.ServerPMTotal, clientRegister);
                File.WriteAllText("./Server/PacketManager.cs", ServerPacketStr);
            }
        }
    }
}