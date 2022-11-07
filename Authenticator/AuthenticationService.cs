using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using AuthenticatorDataTier;
using DataTypes;

namespace Authenticator
{
    internal class AuthenticationService : AuthenticationServiceInterface
    {
        AccountFileSystem fileSystem;
        TokenFileSystem tokenSystem;
        public AuthenticationService()
        {
            fileSystem = new AccountFileSystem("../");
            tokenSystem = new TokenFileSystem("../");
        }
        public String Register(String name, String password)
        {
           /* foreach (AccountStruct accounts in fileSystem.getAccounts())
            {
                if (accounts.name == name)
                    return "account name already exsists";
            } */
            fileSystem.addAccount(new AccountStruct(name, password));
            return "sucessfuly registered";
        }
        public int Login(String name, String password)
        {
            foreach(AccountStruct accounts in fileSystem.getAccounts())
            {
                if(accounts.name == name && accounts.password == password)
                {
                    int token = new Random().Next();
                    tokenSystem.addToken(token);
                    return token;
                }
            }
            return -1;
        }
        public String validate(int _token)
        {
            foreach(int token in tokenSystem.getTokens())
            {
                if(_token == token)
                {
                    return "validated";
                }
            }
            return "not validated";
        }
        public static async void clearTokens(int time)
        {
            while (true)
            {
                await Task.Delay(time);
                TokenFileSystem.clearToken();
            }
        }
            /*public void setPath(string path)
            {
                fileSystem.setFilePath(path + "accounts.txt");
                tokenSystem.setFilePath(path + "tokens.txt");
            }*/
        }
}
