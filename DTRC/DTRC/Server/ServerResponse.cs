using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace DTRC.Server {
    
    public class Response {
        public bool Error { get; set; }
        public string Message { get; set; }
    }


    public static class ServerResponse {

        public static Response ParsingJsonResponse(string jsonString) {
            Response response = null;
            try {
                response = JsonConvert.DeserializeObject<Response>(jsonString);

            } catch (Exception e) {
                Debug.WriteLine(e.StackTrace);
                response.Error = true;
                response.Message = "Il valore restituito dal server e' in un formato sconosciuto.";
            }

            return response;
        }

    }

}
