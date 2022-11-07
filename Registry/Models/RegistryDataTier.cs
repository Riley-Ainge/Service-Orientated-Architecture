using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using DataTypes;
using System.Text;
using Newtonsoft.Json;


namespace Registry.Models
{
    public static class RegistryDataTier
    {
        //public static String path = "C:\\Users\\gameb\\OneDrive\\Documents\\Assignment1\\registry.txt";
        public static List<ServiceDescription> getServices()
        {
            var result = new List<ServiceDescription>();
            FileStream fs = File.Open(GlobalPath.path+ "registry.txt", FileMode.Open);
            using (var reader = new StreamReader(fs))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        result.Add(JsonConvert.DeserializeObject<ServiceDescription>(line));
                    }
                }
            }
            fs.Close();
            return result;
            /*string json = File.ReadAllText(path);
            List<ServiceDescription> services = JsonConvert.DeserializeObject<List<ServiceDescription>>(json);
            return services;*/
        }
        public static ServiceDescription getService(int i)
        {
            string json = File.ReadAllText(GlobalPath.path+"registry.txt");
            List<ServiceDescription> services = JsonConvert.DeserializeObject<List<ServiceDescription>>(json);
            return services[i];
        }
        public async static void addService(ServiceDescription service)
        {
            /*if (!File.Exists(path))
            {
                File.Create(path);
            }
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.None))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(JsonConvert.SerializeObject(service));
                fs.Write(info, 0, info.Length);
            }*/
            StreamWriter file = new StreamWriter(GlobalPath.path+ "registry.txt", append: true);
            await file.WriteLineAsync(JsonConvert.SerializeObject(service));
            file.Close();
        }
        /*public static void setFilePath(String _path)
        {
            path = _path + "registry.txt";
        }*/
        public async static void removeAt(int i)
        {
            List<ServiceDescription> services = getServices();
            services.RemoveAt(i);
            File.WriteAllText(GlobalPath.path+ "registry.txt", "");
            StreamWriter file = new StreamWriter(GlobalPath.path+ "registry.txt", append: true);
            foreach (ServiceDescription service in services)
            {
                await file.WriteLineAsync(JsonConvert.SerializeObject(service));
            }
            file.Close();
            //File.WriteAllText(path, JsonConvert.SerializeObject(services));
        }
    }
}