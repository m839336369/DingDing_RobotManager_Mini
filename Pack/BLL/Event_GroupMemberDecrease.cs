using Native.Core;
using Native.Csharp.App;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pack.BLL
{
    public class Event_GroupMemberDecrease:IGroupMemberDecrease
    {
        public void GroupMemberDecrease(object sender, CQGroupMemberDecreaseEventArgs e)
        {
            try
            {
                if (WS.webSocketRunStatus)
                {
                    if (e.FromQQ == AppInfo.self)
                    {
                        //自己被T
                    }
                    else
                    {
                        WS.MessageForSend data = new WS.MessageForSend();
                        data.rid = WS.Rid();
                        data.from = e.BeingOperateQQ;
                        data.content = $"[退群通知]:\n入群用户:{data.from}\n入群时间:{DateTime.Now}";
                        data.qun = e.FromGroup;
                        if(Robot.groupnumber.TryGetValue(e.FromGroup,out int value))
                        {
                            Robot.groupnumber.Remove(e.FromGroup);
                            Robot.groupnumber.Add(e.FromGroup, value - 1);
                            Robot.group_member_number.Remove((e.FromGroup, e.BeingOperateQQ));

                        }
                        var qqInfo = e.CQApi.GetStrangerInfo(e.BeingOperateQQ).Nick; 
                        if (!WS.cacheQQ.TryGetValue(e.BeingOperateQQ, out data.fromname))
                        {
                            if (qqInfo != null)
                            {
                                data.fromname = qqInfo;
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
                        if (Customize.config.member_leave_send == 1)
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
                                $"{data.content}");
                        }
                        else if (Customize.config.member_leave_send == 2)
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
                WS.Log("处理群成员退出事件时出错, 错误信息:" + ex.Message);
            }
        }
    }
}
