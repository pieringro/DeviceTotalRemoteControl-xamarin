using PCLAppConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTRC.Server {
    public class ServerConfig {
        private static ServerConfig _instance = null;
        public static ServerConfig Instance {
            get {
                if(_instance == null) {
                    _instance = new ServerConfig();
                }
                return _instance;
            }
        }
        
        private ServerConfig() {
            server_url                  = ConfigurationManager.AppSettings["server_url"];
            server_url_send_json_data   = server_url + ConfigurationManager.AppSettings["server_url_send_json_data"];
            server_url_send_pic         = server_url + ConfigurationManager.AppSettings["server_url_send_pic"];
            server_url_update_token     = server_url + ConfigurationManager.AppSettings["server_url_update_token"];
            server_url_login            = server_url + ConfigurationManager.AppSettings["server_url_login"];
            server_url_new_device       = server_url + ConfigurationManager.AppSettings["server_url_new_device"];
        }

        public string server_url;
        public string server_url_send_json_data;
        public string server_url_send_pic;
        public string server_url_update_token;
        public string server_url_login;
        public string server_url_new_device;
    }



    public static class SystemErrors {
        public static string DEVICE_EXISTS = "0";
    }


}
