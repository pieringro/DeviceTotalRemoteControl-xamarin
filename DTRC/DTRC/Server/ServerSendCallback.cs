using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTRC.Server {

    public delegate void ServerSendCallback(bool result, string message);

    
    public delegate void ServerSendFileCallback(bool result, string message, string path = null);


}
