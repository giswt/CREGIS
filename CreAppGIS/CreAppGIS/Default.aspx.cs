using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CreAppGIS
{
    public partial class _Default : Page
    {

        public static string ste111 = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            string path = Server.MapPath(".");
            //System.IO.Stream body = Request.InputStream;
           // System.Text.Encoding encoding = Request.ContentEncoding;
           // System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
          //  string json = reader.ReadToEnd();
            string r = GetIP();

           // Response.ContentType = "application/json";
          //  Response.ContentType = "application/octet-stream";
            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            //通知浏览器下载文件而不是打开
           // string filename = savefilepath.Substring(savefilepath.LastIndexOf(@"\") + 1);
            //Response.AddHeader("Content-Disposition", "attachment;   filename=" + HttpUtility.UrlEncode(savefilepath, System.Text.Encoding.UTF8));
          //  Response.AddHeader("Content-Disposition", "attachment; filename=" + json);
            // Response.BinaryWrite(bytes);            
           // Response.Write(r+":"+json);

            //string callback = Request.QueryString["callback"];

            //var msg = new { message = "收到！" };
            //string result = new JavaScriptSerializer().Serialize(msg);
            //Response.Write(callback + "({'result':" + "200" + "})");
            //Response.Flush();
            //Response.End();

           
                string callback = Request.QueryString["callback"];
                string q1 = Request.QueryString["q"];
                if (callback == null || q1 == null)
                    return;

                if (q1.Length > 0)
                {

                    int a = InsertLog(path, r, q1, "");
                    // return;
                    //string data = "{\"title\": \"" + r + "ss" + q1 + "\",\"link\": \"http://www.sina.com.cn\",\"items\": [{\"title\": \"Russell 003\",\"color\": \"red\"},{\"title\": \"Cat [07.04.11]\",\"color\": \"yellow\"}]}";
                    //string data = "{\"title\":\"window.localStorage.clear();\"}";
                    string data = "{\"title\":\" var a=1;\"}";
                    if (a == 0)
                    {
                        data = "{\"title\": \"" + "111" + "\"}";
                    }
                    string result = string.Format("{0}({1})", callback, data);
                    Response.Expires = -1;
                    Response.Clear();
                    Response.ContentEncoding = Encoding.UTF8;
                    Response.ContentType = "application/json";
                    Response.Write(result);
                    Response.Flush();
                    Response.End();
                }
           
      

        }
        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.UserHostAddress;
            }
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        public static int InsertLog(string path,string ip,string type,string remark)
        {

            
            try
            {
                OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+@path+"\\log.accdb");
                //这里，   @是解决转义字符串或者把后面的\全部变为\\，如  
                //OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\\Database1.accdb");  

                conn.Open();

                string str = "insert into log (TIME,IPADDRESS,TYPE,REMARK) values ('" + DateTime.Now.ToString() + "','" + ip + "','" + type + "','" + remark + "')";
                OleDbCommand cd = new OleDbCommand("insert into log ([TIME],[IPADDRESS],[TYPE],[REMARK]) values (#"+DateTime.Now.ToString()+"#,'"+ip+"','"+type+"','"+remark+"')", conn);
                OleDbDataReader dr = cd.ExecuteReader();
                conn.Close();
            }
            catch(Exception ex)
            {
                ste111 = ex.ToString();
                WriteLog(ste111);
                return 0;
            }

            return 1;
        }

        ///
        public static void WriteLog(string text)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = System.IO.Path.Combine(path
              , "OutputStreamLogs\\" + DateTime.Now.ToString("yy-MM-dd"));

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            string fileFullName = System.IO.Path.Combine(path
              , string.Format("{0}.log", DateTime.Now.ToString("yyMMdd-HHmmss")));


            using (StreamWriter output = System.IO.File.AppendText(fileFullName))
            {
                output.WriteLine(text);

                output.Close();
            }



        }
    
    
    
    
    }
}