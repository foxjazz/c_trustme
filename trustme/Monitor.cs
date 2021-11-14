using System;
using System.Collections.Generic;
using System.Linq;

namespace trustme
{
    public class Monitor
    {
        public Monitor()
        {
           
        }

        public static Dictionary<string, string> tmData;
        public static void start()
        {
            while (true)
            {
                Console.Write(">>");
                string line = readl();
                if (line.StartsWith("q") || line.StartsWith("e"))
                {
                    Console.WriteLine("Exiting Trustme.");
                    setup.Save();
                    return;
                }
                if (line == "add")
                {
                    Console.Write("\r\nname:");
                    string data = readl();
                    Console.Write("password:");
                    string pw = readl();
                    tmData.Remove(data);
                    tmData.Add(data, pw);
                }

                if (line == "find")
                {
                    Console.Write("\r\nname:");
                    string data = readl();
                    Console.WriteLine("Key List");
                    for (int i = 0; i < tmData.Count; i++)
                    {
                        string key = tmData.Keys.ToList()[i];
                        if (key.StartsWith(data))
                        {
                            Console.WriteLine(key);
                        }
                    }
                    Console.WriteLine("Ended ------");
                }

                if (line == "get")
                {
                    Console.Write("\r\nname:");
                    string data = readl();
                    string val;
                    if (tmData.TryGetValue(data, out val)) 
                    {

                        Console.WriteLine(val);
                    }
                    else
                    {
                        Console.WriteLine("not found");
                    }
                }

            }

        }
        public static string readl()
        {
            return Console.ReadLine().ToLower();
        }
    }
}
