using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTRC.Server {
    public class ServerConfig {

        public const string SERVER_URL = "http://192.168.1.8/DTRC";

        public const string SERVER_URL_SEND_JSON_DATA = SERVER_URL+"/receiveData.php";

        public const string SERVER_URL_SEND_PIC = SERVER_URL+"/newPic.php";

        public const string SERVER_URL_UPDATE_TOKEN = SERVER_URL + "/updateDeviceToken.php";

        public const string SERVER_URL_LOGIN = SERVER_URL + "/loginUser.php";

    }
}
