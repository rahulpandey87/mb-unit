using System;
using System.IO;
using System.Text;
using System.Xml;

namespace MbUnit.Documentation
{
    public class HHKFile
    {
        //Automatically inserted when processing.
        const string APIFLAG = "INSERT_SANDCASTLE_DOCUMENTATION_HERE";

        public static void MergeTOC(string rootFolder, string sourceFolder)
        {
            System.Diagnostics.Debug.Write("---MergeTOC---");
            string[] projectTOCFiles = Directory.GetFiles(rootFolder, "*.hhk");

            foreach (string tocPath in projectTOCFiles)
            {
                System.Diagnostics.Debug.Write(tocPath);

                string contents = GetSandcastleTOC(tocPath);

                string newTOCString = ProcessXmlToString(sourceFolder);
                
                string mergedContent;

                if (newTOCString.Contains(APIFLAG))
                {
                    string cleanedContents = CleanContents(contents);

                    string newContents = newTOCString.Replace(APIFLAG, cleanedContents);
                    mergedContent = string.Format("<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML/EN\"><HTML><BODY>{0}</BODY></HTML>", newContents);
                }
                else
                {
                    int indexOfTOCEnd = contents.IndexOf("<UL>") + 4;
                    mergedContent = contents.Insert(indexOfTOCEnd, newTOCString);
                }

                StreamWriter sw = new StreamWriter(tocPath);
                sw.Write(mergedContent);
                sw.Close();

            }
        }

        private static string CleanContents(string contents)
        {
            string result = contents.Replace("<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML/EN\">", "");
            result = result.Replace("<HTML>", ""); result = result.Replace("</HTML>", "");
            result = result.Replace("<BODY>", ""); result = result.Replace("</BODY>", "");
            return result;
        }

        private static string ProcessXmlToString(string sourceFolder)
        {
            //    <LI><OBJECT type="text/sitemap">
            //      <param name="Name" value="">
            //      <param name="Local" value="html\xxx.htm">
            //    </OBJECT></LI>
            System.Diagnostics.Debug.Write("---ProcessXmlToString---");

            string path = Path.Combine(sourceFolder, "toc.xml");

            if (!File.Exists(path))
                throw new ArgumentException("File not found: " + path);

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNodeList nodes = doc.SelectNodes("/Items/Item");

            string toc = ProcessXmlNodes(nodes);
            System.Diagnostics.Debug.Write(toc);
            return toc;
        }

        private static string ProcessXmlNodes(XmlNodeList nodes)
        {
            System.Diagnostics.Debug.Write("---ProcessXmlNodes---");
            System.Diagnostics.Debug.Write(nodes.Count);
            StringBuilder toc = new StringBuilder();
            foreach (XmlNode node in nodes)
            {
                string temptoc = string.Empty;

                if (node.HasChildNodes)
                {
                    temptoc = ProcessXmlNodes(node.ChildNodes);
                }

                string fileName = node.Attributes["Filename"].InnerText;
                string title = node.Attributes["Title"].InnerText;

                StringBuilder stringBuilder = new StringBuilder(185);
                stringBuilder.AppendFormat(@"    <LI><OBJECT type=""text/sitemap"">{0}", Environment.NewLine);
                stringBuilder.AppendFormat(@"      <param name=""Name"" value=""{0}"">{1}", title, Environment.NewLine);
                stringBuilder.AppendFormat(@"      <param name=""Local"" value=""html\{0}"">{1}", fileName, Environment.NewLine);
                stringBuilder.AppendFormat(@"    </OBJECT></LI>");

                try
                {
                    if (node.Attributes["IsAPI"].InnerText.Equals("True"))
                        stringBuilder.Append(APIFLAG);
                }
                catch { }

                stringBuilder.Append(temptoc);

                toc.Append(stringBuilder.ToString());


            }
            System.Diagnostics.Debug.Write("DONE-------");

            return toc.ToString();
        }

        private static string GetSandcastleTOC(string tocPath)
        {
            StreamReader sr = new StreamReader(tocPath);
            string contents = sr.ReadToEnd();
            sr.Close();
            System.Diagnostics.Debug.Write(">>>GetSandcastleTOC>>>");
            return contents;
        }
    }
}
