using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DataTypes;
using Newtonsoft.Json;

namespace AuthenticatorDataTier
{
    public class AccountFileSystem
    {
        static String path;
        public AccountFileSystem(String _path)
        {
            path = _path;
        }
        public List<AccountStruct> getAccounts()
        {
            /*if (!File.Exists(path))
            {
                File.Create(path);
            }*/
            var result = new List<AccountStruct>();
            FileStream fs = File.Open(GlobalPath.path+"accounts.txt", FileMode.Open);
            using (var reader = new StreamReader(fs))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        result.Add(JsonConvert.DeserializeObject<AccountStruct>(line));
                    }
                }
            }
            fs.Close();
            return result;
            /* using (FileStream fs = File.Open(path, FileMode.Open))
             {
                 byte[] b = new byte[1024];
                 UTF8Encoding temp = new UTF8Encoding(true);

                 while (fs.Read(b, 0, b.Length) > 0)
                 {
                     string name = "";
                     string password = "";
                     bool hasFoundSpace = false;
                     foreach (char letter in temp.GetString(b))
                     {
                         if (hasFoundSpace)
                         {
                             password += letter;
                         }
                         else
                         {
                             if (letter != ' ')
                             {
                                 name += letter;
                             }
                             else
                             {
                                 hasFoundSpace = true;
                             }
                         }
                     }
                     accounts.Add(new AccountStruct(name, password));
                 }
             }
             return accounts; */
        }
        public AccountStruct getAccount(int i)
        {
            using (FileStream fs = File.Open(GlobalPath.path + "accounts.txt", FileMode.Open))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                int count = 0;

                while (fs.Read(b, 0, b.Length) > 0)
                {
                    if (count == i)
                    {
                        string name = "";
                        string password = "";
                        bool hasFoundSpace = false;
                        foreach (char letter in temp.GetString(b))
                        {
                            if (hasFoundSpace)
                            {
                                password += letter;
                            }
                            else
                            {
                                if (letter != ' ')
                                {
                                    name += letter;
                                }
                                else
                                {
                                    hasFoundSpace = true;
                                }
                            }
                        }
                        return new AccountStruct(name, password);
                    }
                    count += 1;
                }
            }
            return new AccountStruct();
        }
        public async void addAccount(AccountStruct account)
        {
            /*if(!File.Exists(path))
            {
                File.Create(path);
            }*/
            StreamWriter file = new StreamWriter(GlobalPath.path + "accounts.txt", append: true);
            await file.WriteLineAsync(JsonConvert.SerializeObject(account));
            file.Close();
            /*using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.None))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(account.name + " " + account.password);
                fs.Write(info, 0, info.Length);
            }*/
        }
        /*public void setFilePath(String _path)
        {
            path = _path;
        }*/
    }
}
