using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DTRC.Server {
    
    public class Response {
        public bool Error { get; set; }
        public string Message { get; set; }
    }


    public class ServerResponse {

        public bool ParsingJsonResponse(string jsonString) {
            bool result = true;
            Response response = JsonConvert.DeserializeObject<Response>(jsonString);
            return result;
        }

    }

}
