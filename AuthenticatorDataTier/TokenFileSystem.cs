using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DataTypes;

namespace AuthenticatorDataTier
{
    public class TokenFileSystem
    {
        static String path;
        public TokenFileSystem(String _path)
        {
            path = _path;
        }
        public List<int> getTokens()
        {
            /*List<int> tokens = new List<int>();
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);

                while (fs.Read(b, 0, b.Length) > 0)
                {
                     tokens.Add(int.Parse(temp.GetString(b)));
                }
            }
            return tokens;*/
            var result = new List<int>();
            FileStream fs = File.Open(GlobalPath.path + "tokens.txt", FileMode.Open);
            using (var reader = new StreamReader(fs))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        result.Add(int.Parse(line));
                    }
                }
            }
            fs.Close();
            return result;
        }
        public int getToken(int i)
        {
            using (FileStream fs = File.Open(GlobalPath.path + "tokens.txt", FileMode.Open))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                int count = 0;

                while (fs.Read(b, 0, b.Length) > 0)
                {
                    if(count == i)
                    {
                        return int.Parse(temp.GetString(b));
                    }
                    count += 1;
                }
            }
            return -1;
        }
        public async void addToken(int token)
        {
            /*if (!File.Exists(path))
            {
                File.Create(path);
            }*/
            
            StreamWriter file = new StreamWriter(GlobalPath.path + "tokens.txt", append: true);
            await file.WriteLineAsync(token.ToString());
            file.Close();
            /*using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.None))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(token.ToString().ToCharArray());
                fs.Write(info, 0, info.Length);     
            }*/
        }
        public static void clearToken()
        {
            File.Delete(GlobalPath.path + "tokens.txt");
        }
        /*public void setFilePath(String _path)
        {
            path = _path;
        }*/
    }
}
