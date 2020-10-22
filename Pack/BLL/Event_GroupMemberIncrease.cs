using Native.Core;
using Native.Csharp.App;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
namespace Pack.BLL
{
    /// <summary>
    /// 改动过的人工函数，因为mirai对于进群的支持不咋地
    /// </summary>
    public  class Event_GroupMemberIncrease : IGroupMemberIncrease
    {
        public  void GroupMemberIncrease(object sender, CQGroupMemberIncreaseEventArgs e)
        {
            try
            {
                Customize.Add(e.BeingOperateQQ,e.FromGroup).Wait(1);
                if (WS.webSocketRunStatus)
                {
                    if (e.BeingOperateQQ == AppInfo.self)
                    {
                        //自己新进了一个群
                        WS.SendGroupAndFriend();
                    }
                    else
                    {
                        //新成员加入
                        WS.MessageForSend data = new WS.MessageForSend();
                        data.rid = WS.Rid();
                        data.from = e.BeingOperateQQ;
                        data.content = $"[入群通知]:\n入群用户:{data.from}\n入群时间:{DateTime.Now}";
                        data.qun = e.FromGroup;
                        var qqInfo = Genecontrol.CQApi.GetStrangerInfo(e.BeingOperateQQ).Nick;
                        if (!WS.cacheQQ.TryGetValue(e.BeingOperateQQ, out data.fromname))
                        {
                            if (qqInfo != null)
                            {
                                WS.cacheQQ.TryAdd(e.BeingOperateQQ, qqInfo);
                            }
                            else
                            {
                                WS.Log("群消息接口没有获取到QQ详细信息, QQ号码:" + e.BeingOperateQQ);
                                return;
                            }
                        }

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
                        if (Customize.config.member_enter_send == 1)
                        {
                            if (!WS.cacheQQ.TryGetValue(e.BeingOperateQQ, out var nick))
                            {
                                nick = Genecontrol.CQApi.GetStrangerInfo(e.BeingOperateQQ).Nick;
                                WS.cacheQQ.TryAdd(e.FromQQ, nick);
                            }
                            if (!WS.cacheGroup.TryGetValue(e.BeingOperateQQ, out var gn))
                            {
                                gn = Robot.GetGroupName(e.FromGroup);
                                WS.cacheGroup.TryAdd(e.FromGroup, gn);
                            }
                            Genecontrol.CQApi.SendGroupMessage(Customize.config.fg, $"QQ: {e.BeingOperateQQ}\n" +
                                $"昵称: {nick}\n" +
                                $"群号: {e.FromGroup}\n" +
                                $"群名: {gn}\n" +
                                $"消息内容: \n" +
                                $"{data}");
                        }
                        else if(Customize.config.member_enter_send == 2)
                        {
                            WS.postMessage(data);
                        }
                    }

                }
                else
                {
                    WS.Log("WebSocket状态异常");
                }
            }
            catch (Exception ex)
            {
                WS.Log("处理新增群成员事件时出错, 错误信息:" + ex.Message);
            }
        }
    }
}
