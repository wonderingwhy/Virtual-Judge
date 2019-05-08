using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Models
{
    public class Compiler
    {
        public UInt32 CompilerID { get; set; }
        public UInt32 OJID { get; set; }
        public UInt32 OJCompilerID { get; set; }
        public String Name { get; set; }
    }
}
