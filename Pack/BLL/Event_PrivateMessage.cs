using Clansty.tianlang;
using Native.Core;
using Native.Csharp.App;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pack.BLL
{
    public class Event_PrivateMessage : IPrivateMessage
    {
        public void PrivateMessage(object sender, CQPrivateMessageEventArgs e)
        {
            try
            {
                if (e.Message.Text.Contains("群盯盯存图") && e.Message.Text != "群盯盯存图")
                {
                Thread th = new Thread(new ThreadStart(delegate ()
                {
                    Application.Run(new Form3(false, e.Message.Text.GetRight("群盯盯存图")) as Form);
                }));
                th.TrySetApartmentState(ApartmentState.STA);
                th.Start();
                }
                string message = WS.HandleMsg(e.Message);
                if (string.IsNullOrWhiteSpace(message))
                {
                    return;
                }
                if (WS.webSocketRunStatus)
                {
                    WS.MessageForSend data = new WS.MessageForSend();
                    data.rid = WS.Rid();
                    data.from = e.FromQQ;
                    data.content = message;

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
                            WS.Log("好友消息接口没有获取到QQ详细信息, QQ号码:" + e.FromQQ);
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
                WS.Log("处理好友消息时发生未知错误, 错误信息:" + ex.Message);
            }
        }
    }
}
