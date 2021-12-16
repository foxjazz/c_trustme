using System;
using System.Collections.Generic;
using System.Linq;

namespace trustme
{
    public class Monitor
    {
        
        
        
        public static void start()
        {
            var ds = new DataStore();
            while (true)
            {
                Console.Write(">>");
                string line = readl();
                line = line.Replace("-", "");
                if (line.StartsWith("q") || line.StartsWith("e"))
                {
                    Console.WriteLine("Saving--");
                    ds.Save();
                    Console.WriteLine("Exited.");
                    return;
                }
                if (line == "add")
                {
                    Console.Write("\r\nname:");
                    string data = Console.ReadLine();
                    Console.Write("password:");
                    string pw = Console.ReadLine();
                    ds.AddKeyV(data, pw);
                }

                if (line == "find")
                {
                    Console.Write("\r\nname:");
                    string data = readln();
                    Console.WriteLine("Key List");
                    for (int i = 0; i < ds.GetKeys().Count; i++)
                    {
                        string key = ds.GetKeys()[i];
                        if (key.StartsWith(data))
                        {
                            Console.WriteLine(key);
                        }
                    }
                    Console.WriteLine("Ended ------");
                }

                if (line == "list")
                {
                    foreach (var key in ds.GetKeys().ToArray())
                    {
                        Console.WriteLine(key);
                    }
                }
                if (line == "get")
                {
                    Console.Write("\r\nname:");
                    string data = readln();
                    string val;
                    Console.WriteLine(ds.GetKeyV(data));
                }
                if (line == "help")
                {
                    Console.WriteLine("Commands Are: get ; find ; add; list; q: quit; e: exit");
                }

            }

        }
        public static string readl()
        {
            return Console.ReadLine().ToLower();
        }
        
        public static string readln()
        {
            return Console.ReadLine();
        }
    }
}
