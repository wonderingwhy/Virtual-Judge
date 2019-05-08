using DAL;
using Helpers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace vjudgeCrawlForm
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static HttpHeader header = new HttpHeader();
        private delegate void UpdateStatusDelegate(string status);
        private void UpdateStatus(string status)
        {
            list1.Dispatcher.Invoke(new Action(() => { list1.Items.Add(status); }));
        }
        List<OJ> ojs;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                SqlHelper.OpenConnection();
            }
            catch (Exception e)
            {
                MessageBox.Show("DataBase Connection seemed to be Failed");
                button1.IsEnabled = false;
            }
            MakeHttpHeader();

            try
            {
                ojs = OJService.SelectAll();
                foreach (OJ oj in ojs)
                {
                    comboBox1.Items.Add(oj.OJName);
                }

                comboBox1.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                LogService.Insert(1, e);
                button1.IsEnabled = false;
            }
        }
        static void MakeHttpHeader()
        {
            header.accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
            header.contentType = "application/x-www-form-urlencoded";
            header.method = "POST";
            header.userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            header.maxTry = 300;
        }

        public void Run(string from, string to)
        {

            list1.Items.Clear();
            int ojid = comboBox1.SelectedIndex;
            if (ojid < 0 || ojid >= ojs.Count)
            {
                MessageBox.Show("OJ Choose Error");
            }

            Regex regex = new Regex(ojs[ojid].PatternProblemID);
            Match mf = regex.Match(from);
            Match mt = regex.Match(to);

            if (mf.Success == false || mt.Success == false)
            {
                MessageBox.Show("Format the Problem ID");
                return;
            }

            int fromid = Convert.ToInt32(mf.Groups[1].ToString());
            int toid = Convert.ToInt32(mt.Groups[1].ToString());

            for (int i = fromid; i <= toid; ++i)
            {
                OJ oj = ojs[ojid];
                Problem p = ProblemService.SelectByOJProblemID((uint)ojid + 1, i.ToString());
                if (p.ProblemID == 0)
                {
                    try
                    {
                        string status;
                        bool result = false;
                        switch (ojid)
                        {
                            case 0:
                                result = OJCrawl.HDU(i);
                                break;
                            case 1:
                                result = OJCrawl.NBU(i);
                                break;
                            case 2:
                                result = OJCrawl.POJ(i);
                                break;
                            case 3:
                                result = OJCrawl.ZOJ(i);
                                break;
                            default:
                                break;
                        }
                        status = result ? "ok" : "failed";
                        UpdateStatus(oj.OJName + " " + i + " " + status);
                    }
                    catch (Exception e)
                    {
                        LogService.Insert(1, e);
                    }
                }
                else
                {
                    UpdateStatus(oj.OJName + " " + i + " " + "existing");
                }
            }
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Run(textBox1.Text, textBox2.Text);
            }
            catch (Exception ee)
            {
                LogService.Insert(1, ee);
            }
        }
    }
}
