using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTRC.Services.Commands {
    public class CommandParameter {

        private IDictionary<string, string> _dict;

        public IDictionary<string, string> dict {
            get {
                if(_dict == null) {
                    _dict = new Dictionary<string, string>();
                }
                return _dict;
            }
            set {
                if(value != null) {
                    _dict = value;
                }
            }
        }
    }
}
