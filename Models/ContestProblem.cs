using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Models
{
    public class ContestProblem
    {
        public UInt32 ContestProblemID { get; set; }
        public UInt32 OJID { get; set; }
        public String OJName { get; set; }
        public UInt32 ContestID { get; set; }
        public UInt32 ProblemID { get; set; }
        public String OJProblemID { get; set; }
        public String Title { get; set; }
        public String PreTitle { get; set; }
        public UInt32 Submit { get; set; }
        public UInt32 Accepted { get; set; }
        public Char OrderID { get; set; }
        public String Status { get; set; }
        public String Description { get; set; }
        public String Input { get; set; }
        public String Output { get; set; }
        public String SampleInput { get; set; }
        public String SampleOutput { get; set; }
        public String Hint { get; set; }
        public String TimeLimit { get; set; }
        public String MemoryLimit { get; set; }
        public List<Compiler> Compilers { get; set; }
    }
}
