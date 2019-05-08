using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;

namespace Models
{
    public class OJ
    {
        public UInt32 OJID { get; set; }
        public String OJName { get; set; }
        public String PatternProblemID { get; set; }
        public String PatternTitle { get; set; }
        public String PatternTimeMemory { get; set; }
        public String PatternProblem { get; set; }
        public String PatternHint { get; set; }
        public String PatternStatus { get; set; }
        public String PatternA { get; set; }
        public String PatternImg { get; set; }
        public String Url { get; set; }
        public String UrlPid { get; set; }
        public String UrlLogin { get; set; }
        public String UrlLoginPart1 { get; set; }
        public String UrlLoginPart2 { get; set; }
        public String UrlLoginPart3 { get; set; }
        public String UrlSubmit { get; set; }
        public String UrlSubmitPart1 { get; set; }
        public String UrlSubmitPart2 { get; set; }
        public String UrlSubmitPart3 { get; set; }
        public String UrlSubmitPart4 { get; set; }
        public String UrlStatus { get; set; }
        public Queue<int> QSenders { get; set; }
        public HashSet<String> Statuses { get; set; }
        public List<int> MatchOrder { get; set; }
        public List<Sender> Senders { set; get; }
        public List<CookieContainer> CookieContainers { get; set; }
        public Boolean IsWork { get; set; }
    }
}
