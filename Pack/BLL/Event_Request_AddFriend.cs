using Native.Csharp.App;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pack.BLL
{
    public class Event_Request_AddFriend : IFriendAddRequest
    {
        public void FriendAddRequest(object sender, CQFriendAddRequestEventArgs e)
        {
            try
            {
                if (WS.webSocketRunStatus)
                {
                    //推送好友信息
                    WS.FriendAndQunInfo info = new WS.FriendAndQunInfo();
                    info.number = e.FromQQ;
                    info.type = 1;
                    info.from = 0;
                    //优先从缓存中查找数据
                    if (!WS.cacheQQ.TryGetValue(e.FromQQ, out info.name))
                    {
                        var qqInfo = Genecontrol.CQApi.GetStrangerInfo(e.FromQQ);
                        if (qqInfo != null)
                        {
                            info.name = qqInfo.Nick;
                            WS.cacheQQ.TryAdd(e.FromQQ, qqInfo.Nick);
                        }
                        else
                        {
                            WS.Log("好友消息接口没有获取到QQ详细信息, QQ号码:" + e.FromQQ);
                            e.Handler = false;
                            return;
                        }
                    }
                    WS.FriendAndQun friendInfo = new WS.FriendAndQun();
                    friendInfo.infos = new List<WS.FriendAndQunInfo>();
                    friendInfo.rid = WS.Rid();
                    friendInfo.infos.Add(info);
                    string message = JsonConvert.SerializeObject(friendInfo);
                    WS.sendMessage(message);
                    //推送好友添加通知
                    Task.Run(() => {
                        Thread.Sleep(2000);
                        WS.MessageForSend data = new WS.MessageForSend();
                        data.rid = WS.Rid();
                        data.from = e.FromQQ;
                        data.content = "[好友添加通知]";
                        
                        //优先从缓存中查找数据
                        if (!WS.cacheQQ.ContainsKey(e.FromQQ))
                        {
                            var qqInfo = Genecontrol.CQApi.GetStrangerInfo(e.FromQQ);
                            if (qqInfo != null)
                            {
                                data.fromname = qqInfo.Nick;
                                WS.cacheQQ[e.FromQQ] = qqInfo.Nick;
                            }
                            else
                            {
                                WS.Log("好友消息接口没有获取到QQ详细信息, QQ号码:" + e.FromQQ);
                                e.Handler = false;
                                return;
                            }
                        }
                        else
                        {
                            data.fromname = WS.cacheQQ[e.FromQQ].ToString();
                        }
                        WS.postMessage(data);
                    });
                }
                else
                {
                    WS.Log("WebSocket状态异常");
                }

            }
            catch (Exception ex)
            {
                WS.Log("好友添加成功事件发生未知错误,错误原因:" + ex.Message);
            }
            e.Handler = false;
        }
    }
}
