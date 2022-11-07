using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class ServiceDescription
    {
        public string name;
        public string description;
        public string endpoint;
        public string operands;
        public string type;
        public ServiceDescription(string _name, string _description, string _endpoint, string _operands, string _type)
        {
            name = _name;
            description = _description;
            endpoint = _endpoint;
            operands = _operands;
            type = _type;
        }
    }
}
