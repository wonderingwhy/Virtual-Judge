using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Models
{
    public class Contest
    {
        public UInt32 ContestID { get; set; }
        public String Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public String Status { get; set; }
        public String Declaration { get; set; }
        public String Type { get; set; }
        public UInt32 UserID { get; set; }
        public String Username { get; set; }
        public String Nickname { get; set; }
        public String Password { get; set; }
        public UInt32 ProsNum { get; set; }
    }
}
