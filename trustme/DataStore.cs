using System.Collections.Generic;
using System.IO;
using Jil;

namespace trustme
{
    public class DataStore
    {
        public DataStore()
        {
            string cd = Directory.GetCurrentDirectory();
            var fn = cd + "/trust.tm";

            if (File.Exists(fn))
            {
                var cdata = File.ReadAllBytes(fn);
                string data = Crypto.Instance.decrypt(cdata);
                tmData = Jil.JSON.Deserialize<Dictionary<string, byte[]>>(data);
            }
            else
            {
                tmData = new Dictionary<string, byte[]>();
            }
            
        }
        public void Save()
        {
            string cd = Directory.GetCurrentDirectory();
            var fn = cd + "/trust.tm";
            var data = Jil.JSON.Serialize(tmData);
            byte[] cypherdata = Crypto.Instance.encrypt(data);
            File.WriteAllBytes(fn, cypherdata);
        }
        private Dictionary<string, byte[]> tmData;

        public void AddKeyV(string k, string v)
        {
            tmData.Remove(k);
            tmData.Add(k, Crypto.Instance.encrypt(v));
            
        }
        public List<string> GetKeys()
        {
            var list = new List<string>();
            var data = tmData.Keys;
            foreach(var d in data)
            {
                list.Add(d);
            }
            return list;
        }
        public string GetKeyV(string k)
        {
            byte[] data;
            bool c = tmData.TryGetValue(k, out data);
            if (c)
            {
                return Crypto.Instance.decrypt(data);
            }
            return "Not Found";
        }
    }
    
}