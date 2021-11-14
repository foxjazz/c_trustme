
using System.IO;
using System.Collections.Generic;

using Jil;
namespace trustme
{
    public class setup
    {
        public static void Begin()
        {
            string cd = Directory.GetCurrentDirectory();
            var fn = cd + "\\trust.tm";
            if (File.Exists(fn))
            {
                var data = File.ReadAllText(fn);
                Monitor.tmData = Jil.JSON.Deserialize<Dictionary<string,string>>(data);
            }
            else
            {
                Monitor.tmData = new Dictionary<string, string>();
            }
        }
        public static void Save()
        {
            string cd = Directory.GetCurrentDirectory();
            var fn = cd + "\\trust.tm";
            var data = Jil.JSON.Serialize(Monitor.tmData);
            File.WriteAllText(fn, data);
        }
    }
}