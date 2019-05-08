using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Item
    {
        public TimeSpan Time { get; set; }
        public UInt32 Penalty { get; set; }
        public Boolean IsSolved { get; set; }
        public Boolean IsFirst { get; set; }
        public Item()
        {
            Time = TimeSpan.Zero;
            Penalty = 0;
            IsFirst = false;
            IsSolved = false;
        }
    }
}
