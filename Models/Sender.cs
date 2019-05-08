using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Models
{
    public class Sender
    {
        public UInt32 SenderID { get; set; }
        public UInt32 OJID { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
    }
}
