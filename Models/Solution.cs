using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Models
{
    public class Solution
    {
        public UInt32 SolutionID { get; set; }
        public UInt32 ProblemID { get; set; }
        public UInt32 OJID { get; set; }
        public String OJProblemID { get; set; }
        public String OJRunID { get; set; }
        public Char OrderID { get; set; }
        public UInt32 ContestID { get; set; }
        public UInt32 ContestProblemID { get; set; }
        public String Title { get; set; }
        public UInt32 UserID { get; set; }
        public String Username { get; set; }
        public String Nickname { get; set; } 
        public String RunTime { get; set; }
        public String RunMemory { get; set; }
        public DateTime SubmitTime { get; set; }
        public String Status { get; set; }
        public Boolean IsAccepted { get; set; }
        public UInt32 CompilerID { get; set; }
        public UInt32 OJCompilerID { get; set; }
        public String CompilerName { get; set; }
        public String CodeLength { get; set; }
        public String SourceCode { get; set; }
        public Boolean IsJudged { get; set; }
        public Boolean IsVisible { get; set; }
    }
}
