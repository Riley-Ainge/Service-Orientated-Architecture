using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class AccountStruct
    {
        public String name;
        public String password;
        public AccountStruct()
        {
            name = "";
            password = "";
        }
        public AccountStruct(string _name, string _password)
        {
            this.name = _name;
            this.password = _password;
        }
    }
}
