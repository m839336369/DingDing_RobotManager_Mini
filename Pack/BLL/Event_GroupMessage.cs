using Clansty.tianlang;
using Native.Core;
using Native.Csharp.App;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Sdk.Cqp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pack.BLL
{
    public class Event_GroupMessage : IGroupMessage
    {
        public void GroupMessage(object sender, CQGroupMessageEventArgs e)
        {
            if (!e.IsFromAnonymous)
            {
                try
                {
                    if (e.Message.Text.Contains("群盯盯存图") && e.Message.Text != "群盯盯存图")
                    {
                        Thread th = new Thread(new ThreadStart(delegate ()
                        {
                            var f = new Form3(true, e.Message.Text.GetRight("群盯盯存图"));
                            Application.Run(f as Form);
                        }));
                        th.TrySetApartmentState(ApartmentState.STA);
                        th.Start();
                    }
                    Customize.Msg(e).Wait(1);
                    string message = WS.Group_HandleMsg(e, e.Message);
                    if (string.IsNullOrWhiteSpace(message))
                    {
                        return;
                    }
                    if (WS.webSocketRunStatus)
                    {
                        WS.MessageForSend data = new WS.MessageForSend
                        {
                            rid = WS.Rid(),
                            from = e.FromQQ,
                            content = message,
                            qun = e.FromGroup
                        };
                        //优先从缓存中查找数据
                        if (!WS.cacheQQ.TryGetValue(e.FromQQ, out data.fromname))
                        {
                            var qqInfo = e.CQApi.GetStrangerInfo(e.FromQQ.Id).Nick;
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
                        if (!WS.cacheGroup.TryGetValue(e.FromGroup, out data.qunname))
                        {
                            var groupInfo = Robot.GetGroupName(e.FromGroup);
                            if (groupInfo != null)
                            {
                                data.qunname = groupInfo;
                                WS.cacheGroup.TryAdd(e.FromGroup, groupInfo);
                            }
                            else
                            {
                                WS.Log("群消息接口没有获取到群信息, 群号码:" + e.FromGroup);
                                return;
                            }
                        }
                        WS.postMessage(data);
                    }
                    else
                    {
                        WS.Log("WebSocket状态异常");
                    }
                }
                catch (Exception ex)
                {
                    WS.Log("处理群消息时发生未知错误, 错误信息:" + ex.Message);
                }
            }
            
        }
    }
}
