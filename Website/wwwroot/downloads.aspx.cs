using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

public partial class downloads : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string downloadDir = ConfigurationManager.AppSettings["downloadDir"];
        string html = GetDownloadDirectoryOutput(downloadDir);
        //string html = "<head><title>build.mutantdesign.co.uk - /builds/</title></head><body><H1>build.mutantdesign.co.uk - /builds/</H1><hr><pre><A HREF=\" / \">[To Parent Directory]</A><br><br>    Monday, November 21, 2005  8:33 PM      2251973 <A HREF=\"/builds/MbUnit-2.3.0.exe\">MbUnit-2.3.0.exe</A><br> Wednesday, November 23, 2005  9:36 AM      2251958 <A HREF=\"/builds/MbUnit-2.3.1.exe\">MbUnit-2.3.1.exe</A><br>    Monday, December 12, 2005  9:43 PM      2231225 <A HREF=\"/builds/MbUnit-2.3.11.exe\">MbUnit-2.3.11.exe</A><br> Wednesday, December 14, 2005 10:51 AM      2239306 <A HREF=\"/builds/MbUnit-2.3.13.exe\">MbUnit-2.3.13.exe</A><br>    Sunday, December 18, 2005  6:04 PM      2238969 <A HREF=\"/builds/MbUnit-2.3.15.exe\">MbUnit-2.3.15.exe</A><br>    Sunday, December 18, 2005  6:10 PM      2238976 <A HREF=\"/builds/MbUnit-2.3.16.exe\">MbUnit-2.3.16.exe</A><br>    Sunday, December 18, 2005  6:13 PM      2238980 <A HREF=\"/builds/MbUnit-2.3.17.exe\">MbUnit-2.3.17.exe</A><br>  Wednesday, January 04, 2006  1:49 AM      1294082 <A HREF=\"/builds/MbUnit-2.3.19.exe\">MbUnit-2.3.19.exe</A><br> Wednesday, November 23, 2005  9:53 AM      2251951 <A HREF=\"/builds/MbUnit-2.3.2.exe\">MbUnit-2.3.2.exe</A><br>   Thursday, January 19, 2006  2:59 PM      1294202 <A HREF=\"/builds/MbUnit-2.3.22.exe\">MbUnit-2.3.22.exe</A><br>     Friday, January 20, 2006  5:30 AM      1294179 <A HREF=\"/builds/MbUnit-2.3.23.exe\">MbUnit-2.3.23.exe</A><br></pre><hr></body>";

        if (html != string.Empty)
        {
            Regex regRelease = new Regex("HREF\\s*=\\s*(?:\"\"(?<1>[^\"\"]*)\"\"|(?<1>\\S+))");
            Regex regDate = new Regex("<br>(.*?)<A HREF");

            MatchCollection mclRelease = regRelease.Matches(html);
            MatchCollection mclDate = regDate.Matches(html);

            if (mclRelease.Count > 0 && mclDate.Count > 0)
            {
                SortedList list = new SortedList(new Comparer(System.Globalization.CultureInfo.CurrentCulture));
                StringBuilder strRelease = new StringBuilder();
                StringBuilder strDate = new StringBuilder();
                StringBuilder strDownload = new StringBuilder();

                int matchCount = 0;

                foreach (Match build in mclRelease)
                {
                    string releaseName = build.Value.Replace("HREF=\"", "").Trim();

                    if (releaseName.Length > 1 && releaseName != "/\">[To")
                    {
                        int i = releaseName.IndexOf("\">");
                        string release = releaseName.Substring(i + 2, releaseName.IndexOf("</A>") - i - 2);

                        string date = mclDate[matchCount].ToString();
                        date = date.Replace("<br>", "")
                                    .Replace("<A HREF", "");
                        date = date.Trim();

                        date = date.Substring(0, date.LastIndexOf(" "))
                                     .Trim();

                        DateTime trueDate = Convert.ToDateTime(date);

                        list.Add(release + " - Released " + trueDate, "<A href=\"" + downloadDir + "/" + release + "\">Download</A>");

                        matchCount++;
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    strRelease.Append(list.GetKey(i) + "<BR />");
                    strDownload.Append(list.GetByIndex(i) + "<BR />");
                    strDate.Append("" + "<BR />");
                }

                releaseOutput.Text = strRelease.ToString();
                downloadOutput.Text = strDownload.ToString();
            }
        }
        else
        {
            releaseOutput.Visible = false;
            errorLabel.Visible = true;
        }

    }

    private string GetDownloadDirectoryOutput(string url)
    {
        string html = string.Empty;

        try
        {
            WebClient webClient = new WebClient();
            byte[] reqHTML;
            reqHTML = webClient.DownloadData(url);
            UTF8Encoding objUTF8 = new UTF8Encoding();
            html = objUTF8.GetString(reqHTML);
        }
        catch
        {
        }

        return html;
    }


}


//Download from Build directory

//string downloadDir = ConfigurationManager.AppSettings["downloadDir"];
//string html = GetDownloadDirectoryOutput(downloadDir);
////string html = "<head><title>build.mutantdesign.co.uk - /builds/</title></head><body><H1>build.mutantdesign.co.uk - /builds/</H1><hr><pre><A HREF=\" / \">[To Parent Directory]</A><br><br>    Monday, November 21, 2005  8:33 PM      2251973 <A HREF=\"/builds/MbUnit-2.3.0.exe\">MbUnit-2.3.0.exe</A><br> Wednesday, November 23, 2005  9:36 AM      2251958 <A HREF=\"/builds/MbUnit-2.3.1.exe\">MbUnit-2.3.1.exe</A><br>    Monday, December 12, 2005  9:43 PM      2231225 <A HREF=\"/builds/MbUnit-2.3.11.exe\">MbUnit-2.3.11.exe</A><br> Wednesday, December 14, 2005 10:51 AM      2239306 <A HREF=\"/builds/MbUnit-2.3.13.exe\">MbUnit-2.3.13.exe</A><br>    Sunday, December 18, 2005  6:04 PM      2238969 <A HREF=\"/builds/MbUnit-2.3.15.exe\">MbUnit-2.3.15.exe</A><br>    Sunday, December 18, 2005  6:10 PM      2238976 <A HREF=\"/builds/MbUnit-2.3.16.exe\">MbUnit-2.3.16.exe</A><br>    Sunday, December 18, 2005  6:13 PM      2238980 <A HREF=\"/builds/MbUnit-2.3.17.exe\">MbUnit-2.3.17.exe</A><br>  Wednesday, January 04, 2006  1:49 AM      1294082 <A HREF=\"/builds/MbUnit-2.3.19.exe\">MbUnit-2.3.19.exe</A><br> Wednesday, November 23, 2005  9:53 AM      2251951 <A HREF=\"/builds/MbUnit-2.3.2.exe\">MbUnit-2.3.2.exe</A><br>   Thursday, January 19, 2006  2:59 PM      1294202 <A HREF=\"/builds/MbUnit-2.3.22.exe\">MbUnit-2.3.22.exe</A><br>     Friday, January 20, 2006  5:30 AM      1294179 <A HREF=\"/builds/MbUnit-2.3.23.exe\">MbUnit-2.3.23.exe</A><br></pre><hr></body>";

//if (html != string.Empty)
//{
//    Regex regRelease = new Regex("HREF\\s*=\\s*(?:\"\"(?<1>[^\"\"]*)\"\"|(?<1>\\S+))");
//    Regex regDate = new Regex("<br>(.*?)(?:AM|PM)");

//    MatchCollection mclRelease = regRelease.Matches(html);
//    MatchCollection mclDate = regDate.Matches(html);

//    if (mclRelease.Count > 0 && mclDate.Count > 0)
//    {
//        SortedList list = new SortedList(new Comparer(System.Globalization.CultureInfo.CurrentCulture));
//        StringBuilder strRelease = new StringBuilder();
//        StringBuilder strDate = new StringBuilder();
//        StringBuilder strDownload = new StringBuilder();

//        int matchCount = 0;

//        foreach (Match build in mclRelease)
//        {
//            string text = build.Value.Replace("HREF=\"", "").Trim();
//            if (text.Length > 1 && text != "/\">[To")
//            {
//                int i = text.IndexOf("\">");
//                string release = text.Substring(i + 2, text.IndexOf("</A>") - i - 2);
//                string date = mclDate[matchCount].ToString();
//                date = date.Replace("<br>", "")
//                            .Replace("<A HREF", "");
//                date = date.Trim();

//                DateTime trueDate = Convert.ToDateTime(date);

//                list.Add(release + " - Released " + trueDate, "<A href=\"" + downloadDir + "/" + release + "\">Download</A>");

//                matchCount++;
//            }
//        }

//        for (int i = 0; i < list.Count; i++)
//        {
//            strRelease.Append(list.GetKey(i) + "<BR />");
//            strDownload.Append(list.GetByIndex(i) + "<BR />");
//            strDate.Append("" + "<BR />");
//        }

//        releaseOutput.Text = strRelease.ToString();
//        downloadOutput.Text = strDownload.ToString();
//    }
//}
//else
//{
//    releaseOutput.Visible = false;
//    errorLabel.Visible = true;
//}

    //private string GetDownloadDirectoryOutput(string url)
    //{
    //    string html = string.Empty;

    //    try
    //    {
    //        WebClient webClient = new WebClient();
    //        byte[] reqHTML;
    //        reqHTML = webClient.DownloadData(url);
    //        UTF8Encoding objUTF8 = new UTF8Encoding();
    //        html = objUTF8.GetString(reqHTML); 
    //    }
    //    catch
    //    {
    //    }

    //    return html;
    //}