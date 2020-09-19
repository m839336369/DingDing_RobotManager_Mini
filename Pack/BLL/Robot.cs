using Native.Core;
using Native.Csharp.App;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Mail; 
namespace Pack.BLL
{
    public static class Robot
    {
        public static Dictionary<long, int> groupnumber = new Dictionary<long, int>();
        public static Dictionary<ValueTuple<long,long>, int> group_member_number = new Dictionary<ValueTuple<long, long>, int>();
        public static string GetGroupName(long group)
        {
            return Genecontrol.CQApi.GetGroupInfo(group, false).Name;
        }

        public static void Write_Key_Log(string key, DateTime dateTime, long QQ, string nickName, long QQ_Group, string Group_Name, string msg,string style)
        {
            string sFileName = Customize.config.CSV_SavePath;
            if (!File.Exists(sFileName))
            //验证文件是否存在，有则追加，无则创建
            {
                File.AppendAllText(sFileName, "关键字,方式,日期,QQ号,QQ昵称,群号码,群名称,消息内容\n");
            }
            File.AppendAllText(sFileName, $"{key},{style},{dateTime},{QQ},{nickName},{QQ_Group},{Group_Name},{msg}\n");
        }
        //暂时不需要网页获取
        private static string GetNick(long qq)
        {
            try
            {
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                var json = client.DownloadString($"https://api.vvhan.com/api/qq?qq={qq}");
                client.Dispose();
                var jo = JObject.Parse(json);
                return jo.Value<string>("name");
            }
            catch
            {
                try
                {
                    WebClient client = new WebClient();
                    client.Encoding = Encoding.UTF8;
                    var json = client.DownloadString($"https://api.hmister.cn/qq/?type=nick&qq={qq}");
                    client.Dispose();
                    var jo = JObject.Parse(json);
                    if (jo.Value<int>("code") == 200)
                        return jo.Value<string>("data");
                    else throw new Exception();
                }
                catch
                {
                    try
                    {
                        WebClient client = new WebClient();
                        client.Encoding = Encoding.UTF8;
                        var json = client.DownloadString($"https://api.qqsuu.cn/api/qq?qq={qq}");
                        client.Dispose();
                        var jo = JObject.Parse(json);
                        if (jo.Value<int>("code") == 1)
                            return jo.Value<string>("name");
                        else throw new Exception();
                    }
                    catch
                    {
                        return qq.ToString();
                    }
                }
            }
        }
        /*
        string userEmail,  //发件人邮箱
        string userPswd,   //邮箱帐号密码
        string toEmail,    //收件人邮箱
        string mailServer, //邮件服务器
        string subject,    //邮件标题
        string mailBody,   //邮件内容
        string[] attachFiles //邮件附件
        */
        public static void SendEmail(string userEmail, string userPswd, string toEmail, string mailServer, string subject, string mailBody, string port, bool sll)
        {
            //邮件发送者
            MailAddress from = new MailAddress(userEmail);
            //邮件接收者
            MailAddress to = new MailAddress(toEmail);
            MailMessage mailobj = new MailMessage(from, to);
            // 添加发送和抄送
            // mailobj.To.Add("");
            // mailobj.CC.Add("");

            //邮件标题
            mailobj.Subject = subject;
            //邮件内容
            mailobj.Body = mailBody;
            //邮件不是html格式
            mailobj.IsBodyHtml = false;
            //邮件编码格式
            mailobj.BodyEncoding = System.Text.Encoding.GetEncoding("GB2312");
            //邮件优先级
            mailobj.Priority = MailPriority.High;

            //Initializes a new instance of the System.Net.Mail.SmtpClient class
            //that sends e-mail by using the specified SMTP server.
            SmtpClient smtp = new SmtpClient(mailServer);
            //或者用：
            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = mailServer;
            smtp.EnableSsl = sll;
            smtp.Port = int.Parse(port);
            //不使用默认凭据访问服务器
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(userEmail, userPswd);
            //使用network发送到smtp服务器
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                //开始发送邮件
                smtp.Send(mailobj);
            }
            catch (Exception e)
            {
                WS.Log(e.Message);
                WS.Log(e.StackTrace);
            }
        }
    }
}
