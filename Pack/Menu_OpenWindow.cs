using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Native.Core;
using System.Windows.Forms;

namespace Pack
{
    public class Menu_OpenWindow : IMenuCall
    {
        ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        public void MenuCall(object sender, CQMenuCallEventArgs e)
        {
            try
            {

                if (Customize.running)
                {
                    Genecontrol.Form1 = new Form1();
                    Genecontrol.Form1.ShowDialog();
                }
                else MessageBox.Show("机器人不存在或已删除，此功能将不会工作");
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 对变量置 null, 因为被关闭的窗口无法重复显示

        }
    }
}
