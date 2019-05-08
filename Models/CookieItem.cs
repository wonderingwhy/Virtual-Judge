using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class CookieItem
    {
        public Guid GUID { set; get; }
        public UInt32 UserID { set; get; }
        public CookieItem(Guid g, uint id)
        {
            GUID = g;
            UserID = id;
        }
    }
}
