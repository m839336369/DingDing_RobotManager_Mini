
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using System;
using System.Text;
using System.Threading.Tasks;
using Native.Core;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;
using Native.Csharp.App;

namespace Pack.BLL
{
    public class Event_Enable : IAppEnable
    {
        public void AppEnable(object sender, CQAppEnableEventArgs e)
        {
            Genecontrol.CQApi = e.CQApi;
            Genecontrol.CQLog = e.CQLog;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Customize.configPath = AppDomain.CurrentDomain.BaseDirectory + e.CQApi.GetLoginQQ() + "_enlovoconfig";
            if (File.Exists(Customize.configPath))
                try
                {
                    Customize.config = JsonConvert.DeserializeObject<Customize.Config>(File.ReadAllText(Customize.configPath, Encoding.UTF8));
                }
                catch
                {
                    Customize.config = new Customize.Config();
                }

            Task.Run(new Action(() => WS.Start(null))) ;
        }
    }
}
