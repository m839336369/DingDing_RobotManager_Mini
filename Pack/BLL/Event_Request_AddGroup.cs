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
    public class Event_Request_AddGroup : IGroupAddRequest
    {
        public void GroupAddRequest(object sender, CQGroupAddRequestEventArgs e)
        {
            try
            {
                //群主收到入群申请
                if (e.SubType == Native.Sdk.Cqp.Enum.CQGroupAddRequestType.ApplyAddGroup)
                {
                    if (WS.webSocketRunStatus)
                    {
                        WS.MessageForSend data = new WS.MessageForSend();
                        data.rid = WS.Rid();
                        data.from = e.FromQQ;
                        data.content = "[入群通知]";
                        data.qun = e.FromGroup;
                        //优先从缓存中查找数据
                        if (!WS.cacheQQ.TryGetValue(e.FromQQ, out data.fromname))
                        {
                            var qqInfo = Genecontrol.CQApi.GetStrangerInfo(e.FromQQ);
                            if (qqInfo != null)
                            {
                                data.fromname = qqInfo.Nick;
                                WS.cacheQQ.TryAdd(e.FromQQ, qqInfo.Nick);
                            }
                            else
                            {
                                WS.Log("群消息接口没有获取到QQ详细信息, QQ号码:" + e.FromQQ);
                                e.Handler = false;
                                return;
                            }
                        }
                        var res = Genecontrol.CQApi.SetGroupAddRequest(e.Request, Native.Sdk.Cqp.Enum.CQGroupAddRequestType.ApplyAddGroup, Native.Sdk.Cqp.Enum.CQResponseType.PASS, data.fromname);

                        WS.Log("通过群添加请求, 返回标记:" + res.ToString());
                        //优先从缓存中查找数据
                        if (!WS.cacheGroup.TryGetValue(e.FromGroup, out data.qunname))
                        {
                            Native.Sdk.Cqp.Model.GroupInfo groupInfo = Genecontrol.CQApi.GetGroupInfo(e.FromGroup);
                            if (groupInfo != null)
                            {
                                data.qunname = groupInfo.Name;
                                WS.cacheGroup.TryAdd(e.FromGroup, groupInfo.Name);
                            }
                            else
                            {
                                WS.Log("群消息接口没有获取到群信息, 群号码:" + e.FromGroup);
                                e.Handler = false;
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
            }
            catch (Exception ex)
            {
                WS.Log("处理入群申请时发生未知错误, 错误信息:" + ex.Message);
            }
            try
            {
                //收到邀请入群事件
                if (e.SubType == Native.Sdk.Cqp.Enum.CQGroupAddRequestType.RobotBeInviteAddGroup)
                {
                    if(Customize.config.Manager_Group_Invite_Request == 0)
                    {
                        e.CQApi.SetGroupAddRequest(e.Request.ResponseFlag, Native.Sdk.Cqp.Enum.CQGroupAddRequestType.RobotBeInviteAddGroup, Native.Sdk.Cqp.Enum.CQResponseType.PASS);
                    }
                    else if (Customize.config.Manager_Group_Invite_Request == 1 && Customize.config.Manager_Group_Invite_QQ.Split('&').Contains(e.FromQQ.Id.ToString()))
                    {
                        e.CQApi.SetGroupAddRequest(e.Request.ResponseFlag, Native.Sdk.Cqp.Enum.CQGroupAddRequestType.RobotBeInviteAddGroup, Native.Sdk.Cqp.Enum.CQResponseType.PASS);
                    }
                    else if(Customize.config.Manager_Group_Invite_Request == 2)
                    {
                        e.CQApi.SetGroupAddRequest(e.Request.ResponseFlag, Native.Sdk.Cqp.Enum.CQGroupAddRequestType.RobotBeInviteAddGroup, Native.Sdk.Cqp.Enum.CQResponseType.FAIL);
                    }
                }
            }
            catch (Exception ex)
            {
                WS.Log("处理新增群成员事件时出错, 错误信息:" + ex.Message);
            }
            e.Handler = false;
        }
    }
}
