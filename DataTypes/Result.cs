using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class Result
    {
        public string status;
        public string result;
        public Result(string _status, string  _result)
        {
            status = _status;
            result  = _result;
        }
    }
}
