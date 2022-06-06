using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Models
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string myIssuer { get; set; }
        public string myAudience { get; set; }
        public int ExpiryTime { get; set; }
        public string ProtectorValue { get; set; }
    }
}
