using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Net.Plugins.GameCenter
{
    public class PWAccount
    {
        public string UserId;
        public string Title;

        public override string ToString()
        {
            return Title;
        }
    }
}
