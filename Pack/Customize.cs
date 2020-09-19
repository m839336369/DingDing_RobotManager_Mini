using Native.Csharp.App;
using Native.Sdk.Cqp;
using Native.Sdk.Cqp.EventArgs;
using Pack;
using Pack.BLL;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Native.Core
{
    static class Customize
    {
        public static bool running = false;
        public class Config
        {
            public class Welcome
            {
                public bool enabled = false;
                public string words = "";
            }
            public Welcome gWelcome = new Welcome();
            public Welcome pWelcome = new Welcome();
            public Dictionary<long, bool> gList = new Dictionary<long, bool>();
            public Dictionary<long, bool> pList = new Dictionary<long, bool>();
            public Dictionary<long, bool> fList = new Dictionary<long, bool>();
            public Dictionary<string, string> gKws = new Dictionary<string, string>();
            public Dictionary<string, string> pKws = new Dictionary<string, string>();
            public string[] f = new string[] { };
            public bool realF = false;
            public bool CSV = false;
            public string CSV_SavePath;
            public bool SMTP = false;
            public string SMTP_User;
            public string SMTP_Pass;
            public string SMTP_Acieve;
            public string SMTP_Server;
            public long fg = 0;
            public int gdb = 0;
            public int gdt = 20;
            public int pdb = 0;
            public int pdt = 20;
        }
        public static Config config = new Config();
        internal static string configPath;
        public const string split = "\n*#*#nextRandAnswer#*#*\n";
        static readonly Random r = new Random();
        internal static async Task Msg(CQGroupMessageEventArgs e)
        {
            try
            {
                if (!running)
                    return;
                if (config.gList.ContainsKey(e.FromGroup))
                    if (config.gList[e.FromGroup])
                        foreach (var i in config.gKws)
                            if (e.Message.Text.Contains(i.Key))
                            {
                                var ss = i.Value.Split(new string[] { split }, StringSplitOptions.RemoveEmptyEntries);
                                var rv = r.Next(ss.Length);
                                var mts = ss[rv].Replace("[at]", $"{CQApi.CQCode_At(e.FromQQ)}");//at
                                await Task.Delay(r.Next(config.gdb, config.gdt + 1) * 1000);
                                e.CQApi.SendGroupMessage(e.FromGroup, mts);
                            }
                if (config.pList.ContainsKey(e.FromGroup))
                    if (config.pList[e.FromGroup])
                        foreach (var i in config.pKws)
                            if (e.Message.Text.Contains(i.Key))
                            {
                                var ss = i.Value.Split(new string[] { split }, StringSplitOptions.RemoveEmptyEntries);
                                var rv = r.Next(ss.Length);
                                await Task.Delay(r.Next(config.pdb, config.pdt + 1) * 1000);
                                e.CQApi.SendPrivateMessage(e.FromQQ, ss[rv]);
                            }
                if (config.fList.ContainsKey(e.FromGroup) && config.fg != 0)
                    if (config.fList[e.FromGroup])
                        if (config.f.Length > 0)
                            for (var i = 0; i < config.f.Length; i++)
                            {
                                if (string.IsNullOrWhiteSpace(config.f[i]))
                                    continue;
                                var r = new Regex(config.f[i]);
                                if (r.IsMatch(e.Message.Text))
                                {
                                    if (config.realF)
                                    {
                                        if (!WS.cacheQQ.TryGetValue(e.FromQQ, out var nick))
                                        {
                                            nick = e.CQApi.GetStrangerInfo(e.FromQQ.Id).Nick;
                                            WS.cacheQQ.TryAdd(e.FromQQ, nick);
                                        }
                                        if (!WS.cacheGroup.TryGetValue(e.FromGroup, out var gn))
                                        {
                                            gn = Robot.GetGroupName(e.FromGroup);
                                            WS.cacheGroup.TryAdd(e.FromGroup, gn);
                                        }
                                        Genecontrol.CQApi.SendGroupMessage(config.fg, $"QQ: {e.FromQQ}\n" +
                                            $"昵称: {nick}\n" +
                                            $"群号: {e.FromGroup}\n" +
                                            $"群名: {gn}\n" +
                                            $"消息内容: \n" +
                                            $"{e.Message.Text}");
                                    }
                                    else
                                    {
                                        WS.MessageForSend data = new WS.MessageForSend
                                        {
                                            rid = WS.Rid(),
                                            from = e.FromQQ,
                                            content = e.Message.Text,
                                            qun = config.fg
                                        };
                                        //优先从缓存中查找数据
                                        if (!WS.cacheQQ.TryGetValue(e.FromQQ, out data.fromname))
                                        {
                                            var qqInfo = e.Name;
                                            if (qqInfo != null)
                                            {
                                                data.fromname = qqInfo;
                                                WS.cacheQQ.TryAdd(e.FromQQ, qqInfo);
                                            }
                                            else
                                            {
                                                WS.Log("群消息接口没有获取到QQ详细信息, QQ号码:" + e.FromQQ);
                                                return;
                                            }
                                        }

                                        //优先从缓存中查找数据
                                        if (!WS.cacheGroup.TryGetValue(config.fg, out data.qunname))
                                        {
                                            var groupInfo = Robot.GetGroupName(config.fg);
                                            if (groupInfo != null)
                                            {
                                                data.qunname = groupInfo;
                                                WS.cacheGroup.TryAdd(config.fg, groupInfo);
                                            }
                                            else
                                            {
                                                WS.Log("群消息接口没有获取到群信息, 群号码:" + config.fg);
                                                return;
                                            }
                                        }
                                        WS.postMessage(data);
                                    }
                                    if (Customize.config.CSV)
                                    {
                                        string title;
                                        if (e.Message.Text.Length > 20) title = e.Message.Text.Substring(0, 20);
                                        else title = e.Message.Text;
                                        Robot.Write_Key_Log(config.f[i], DateTime.Now, e.FromQQ, e.CQApi.GetStrangerInfo(e.FromQQ.Id).Nick, e.FromGroup.Id, e.FromGroup.GetGroupInfo().Name, e.Message.Text, "群聊");

                                    }
                                    if (Customize.config.SMTP)
                                    {
                                        string title;
                                        if (e.Message.Text.Length > 20) title = e.Message.Text.Substring(0, 20);
                                        else title = e.Message.Text;
                                        string msg = $"关键字:{config.f[i] }\n方式:群聊\n日期:{DateTime.Now.ToString()}\nQQ号:{e.FromQQ}\nQQ昵称:{e.CQApi.GetStrangerInfo(e.FromQQ.Id).Nick}\n群号码:{e.FromGroup}\n群名称:{e.FromGroup.GetGroupInfo().Name}\n消息内容:{e.Message.Text}";
                                        Robot.SendEmail(config.SMTP_User, config.SMTP_Pass, config.SMTP_Acieve, config.SMTP_Server, title, msg);
                                    }
                                }
                            }
            }
            catch (Exception err) { WS.Log(err.Message); }
        }

        internal static async Task Add(long BeingOperateQQ, long FromGroup)
        {
            try
            {
                if (!running)
                    return;
                if (config.gWelcome.enabled)
                {
                    if (config.gList.ContainsKey(FromGroup))
                        if (config.gList[FromGroup])
                        {
                            var mts = config.gWelcome.words.Replace("[at]", $"{CQApi.CQCode_At(BeingOperateQQ)}");//at
                            await Task.Delay(r.Next(config.gdb, config.gdt + 1) * 1000);
                            Genecontrol.CQApi.SendGroupMessage(FromGroup, mts);
                        }
                }
                if (config.pWelcome.enabled)
                {
                    if (config.pList.ContainsKey(FromGroup))
                        if (config.pList[FromGroup])
                        {
                            var mts = config.pWelcome.words;
                            await Task.Delay(r.Next(config.pdb, config.pdt + 1) * 1000);
                            Genecontrol.CQApi.SendPrivateMessage(FromGroup, BeingOperateQQ, mts);
                        }
                }
            }
            catch(Exception err) { WS.Log(err.Message); }

        }
    }
}
