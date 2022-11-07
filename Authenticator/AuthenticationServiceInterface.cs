using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Authenticator
{
    [ServiceContract]
    public interface AuthenticationServiceInterface
    {
        [OperationContract]
        String Register(String name, String password);
        [OperationContract]
        int Login(String name, String password);
        [OperationContract]
        String validate(int token);

    }
}
