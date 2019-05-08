using DAL;
using Helpers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace vjudgeCrawlForm
{
    public static class OJCrawl
    {
        public static OJ hdu = OJService.SelectByID(1);
        public static OJ nbu = OJService.SelectByID(2);
        public static OJ poj = OJService.SelectByID(3);
        public static OJ zoj = OJService.SelectByID(4);
        public static bool HDU(int i)
        {
            string patternT = hdu.PatternTitle;
            string patternTM = hdu.PatternTimeMemory;
            string patternP = hdu.PatternProblem;
            string patternH = hdu.PatternHint;

            Regex regexT = new Regex(patternT);
            Regex regexTM = new Regex(patternTM);
            Regex regexP = new Regex(patternP);
            Regex regexH = new Regex(patternH);

            //for (int i = from; i < to; ++i)
            {

                string html = HttpHelper.GetHtml(hdu.UrlPid + i.ToString(), new CookieContainer(), MainWindow.header);
                Match mT = regexT.Match(html);
                Match mTM = regexTM.Match(html);
                Match mP = regexP.Match(html);
                Match mH = regexH.Match(html);

                if (!mT.Success || !mTM.Success || !mP.Success)
                {
                    return false;
                }

                Problem problem = new Problem();
                problem.Title = mT.Groups[1].ToString();

                problem.TimeLimit = mTM.Groups[1].ToString();
                problem.MemoryLimit = mTM.Groups[2].ToString();

                problem.Description = mP.Groups[1].ToString();
                problem.Input = mP.Groups[2].ToString();
                problem.Output = mP.Groups[3].ToString();
                problem.SampleInput = mP.Groups[4].ToString();
                problem.SampleOutput = mP.Groups[5].ToString();

                problem.Hint = mH.Success?mH.Groups[1].ToString():"";
                
                problem.Source = "";

                problem.OJID = 1;
                problem.OJProblemID = i.ToString();

                return AddProblem(problem, hdu.PatternA, hdu.PatternImg, hdu.Url);
            }
        }
        public static bool NBU(int i)
        {
            string patternT = nbu.PatternTitle;
            string patternTM = nbu.PatternTimeMemory;
            string patternP = nbu.PatternProblem;
            string patternH = nbu.PatternHint;
            
            Regex regexT = new Regex(patternT);
            Regex regexTM = new Regex(patternTM);
            Regex regexP = new Regex(patternP);
            Regex regexH = new Regex(patternH);

            //for (int i = from; i < to; ++i)
            {
                string html = HttpHelper.GetHtml(nbu.UrlPid + i.ToString(), new CookieContainer(), MainWindow.header);

                Match mT = regexT.Match(html);
                Match mTM = regexTM.Match(html);
                Match mP = regexP.Match(html);
                Match mH = regexH.Match(html);

                if (!mT.Success || !mTM.Success || !mP.Success)
                {
                    return false;
                }

                Problem problem = new Problem();
                problem.Title = mT.Groups[1].ToString();

                problem.TimeLimit = mTM.Groups[1].ToString();
                problem.MemoryLimit = mTM.Groups[2].ToString();

                problem.Description = mP.Groups[1].ToString();
                problem.Input = mP.Groups[2].ToString();
                problem.Output = mP.Groups[3].ToString();
                problem.SampleInput = mP.Groups[4].ToString();
                problem.SampleOutput = mP.Groups[5].ToString();

                problem.Hint = mH.Success ? mH.Groups[1].ToString() : "";

                problem.Source = "";

                problem.OJID = 2;
                problem.OJProblemID = i.ToString();

                return AddProblem(problem, nbu.PatternA, nbu.PatternImg, nbu.Url);
            }
        }
        public static bool POJ(int i)
        {
            string patternT = poj.PatternTitle;
            string patternTM = poj.PatternTimeMemory;
            string patternP = poj.PatternProblem;
            string patternH = poj.PatternHint;

            Regex regexT = new Regex(patternT);
            Regex regexTM = new Regex(patternTM);
            Regex regexP = new Regex(patternP);
            Regex regexH = new Regex(patternH);

            //for (int i = from; i < to; ++i)
            {
                string html = HttpHelper.GetHtml(poj.UrlPid + i.ToString(), new CookieContainer(), MainWindow.header);

                Match mT = regexT.Match(html);
                Match mTM = regexTM.Match(html);
                Match mP = regexP.Match(html);
                Match mH = regexH.Match(html);

                if (!mT.Success || !mTM.Success || !mP.Success)
                {
                    return false;
                }

                Problem problem = new Problem();
                problem.Title = mT.Groups[1].ToString();

                problem.TimeLimit = mTM.Groups[1].ToString();
                problem.MemoryLimit = mTM.Groups[2].ToString();

                problem.Description = mP.Groups[1].ToString();
                problem.Input = mP.Groups[2].ToString();
                problem.Output = mP.Groups[3].ToString();
                problem.SampleInput = mP.Groups[4].ToString();
                problem.SampleOutput = mP.Groups[5].ToString();

                problem.Hint = mH.Success ? mH.Groups[1].ToString() : "";

                problem.Source = "";

                problem.OJID = 3;
                problem.OJProblemID = i.ToString();

                return AddProblem(problem, poj.PatternA, poj.PatternImg, poj.Url);
            }
        }
        public static bool ZOJ(int i)
        {
            string patternT = zoj.PatternTitle;
            string patternTM = zoj.PatternTimeMemory;
            string patternP = zoj.PatternProblem;

            Regex regexT = new Regex(patternT);
            Regex regexTM = new Regex(patternTM);
            Regex regexP = new Regex(patternP);

            //for (int i = from; i < to; ++i)
            {
                string html = HttpHelper.GetHtml(zoj.UrlPid + i.ToString(), new CookieContainer(), MainWindow.header);

                Match mT = regexT.Match(html);
                Match mTM = regexTM.Match(html);
                Match mP = regexP.Match(html);

                if (!mT.Success || !mTM.Success || !mP.Success)
                {
                    return false;
                }

                Problem problem = new Problem();
                problem.Title = mT.Groups[1].ToString();

                problem.TimeLimit = mTM.Groups[1].ToString();
                problem.MemoryLimit = mTM.Groups[2].ToString();

                problem.Description = mP.Groups[1].ToString();

                problem.Input = "";
                problem.Output = "";
                problem.SampleInput = "";
                problem.SampleOutput = "";
                problem.Hint = "";
                problem.Source = "";

                problem.OJID = 4;
                problem.OJProblemID = i.ToString();
                return AddProblem(problem, zoj.PatternA, zoj.PatternImg, zoj.Url);
            }
        }
        public static bool AddProblem(Problem problem, string pattern_a, string pattern_img, string URL)
        {
            if (problem.Title.Length <= 0)
            {
                return false;
            }
            
            problem.Description = Link(problem.Description, pattern_a, URL);
            problem.Description = Link(problem.Description, pattern_img, URL);
            problem.Input = Link(problem.Input, pattern_a, URL);
            problem.Input = Link(problem.Input, pattern_img, URL);
            problem.Output = Link(problem.Output, pattern_a, URL);
            problem.Output = Link(problem.Output, pattern_img, URL);
            problem.SampleInput = Link(problem.SampleInput, pattern_a, URL);
            problem.SampleInput = Link(problem.SampleInput, pattern_img, URL);
            problem.SampleOutput = Link(problem.SampleOutput, pattern_a, URL);
            problem.SampleOutput = Link(problem.SampleOutput, pattern_img, URL);
            problem.Hint = Link(problem.Hint, pattern_a, URL);
            problem.Hint = Link(problem.Hint, pattern_img, URL);

            ProblemService.Insert(problem);
            return true;
        }
        private static string Link(string str, string pattern, string URL)
        {
            Regex regex = new Regex(pattern);
            Match m = regex.Match(str);
            int cnt = 0;
            while (m.Groups.Count >= 2)
            {
                if (++cnt > 100)
                {
                    throw new Exception("Endless LinkChange: " + str + " " + pattern + " " + URL);
                }
                string src = m.Groups[1].ToString().Trim();
                string temp = src;
                bool flag = false;
                while (temp.Substring(0, 3) == "../")
                {
                    flag = true;
                    temp = temp.Substring(3);
                }
                while (temp.Substring(0, 1) == "/")
                {
                    flag = true;
                    temp = temp.Substring(1);
                }
                if (flag || temp.Substring(0, 4) != "http")
                {
                    str = str.Replace(src, URL + "/" + temp);
                    m = m.NextMatch();
                }
            }
            return str;
        }
    }
}
