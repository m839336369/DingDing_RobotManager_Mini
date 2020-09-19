
using Make.MODEL;
using Native.Sdk.Cqp;
using Newtonsoft.Json;
using Pack.Element;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pack.BLL
{
    public class XY
    {
        public static void CQ_To_Server(string msg,long frompersonal, long fromgroup=-1)
        {
            GeneralControl.socket.Request_Client("CQ$"+frompersonal.ToString()+ "|" + fromgroup.ToString() + "|" + msg);
        }
        public static void Send_To_Server(string msg)
        {
            GeneralControl.socket.Request_Client("客户端$" + msg);
        }
        public static void Send_To_CQ(string msg, long frompersonal, long fromgroup = -1)
        {
            if (fromgroup == -1)
            {
                Console.WriteLine(escape(File.ReadAllText(@"D:\YuanMa\XianYu_XianZhan_CQ\Pack\TextFile1.txt")));
                GeneralControl.CQApi.SendPrivateMessage(frompersonal, "[CQ: xml, data = " + File.ReadAllText(@"D:\YuanMa\XianYu_XianZhan_CQ\Pack\TextFile1.txt") + "]");
            }
            else
            {
                GeneralControl.CQApi.SendGroupMessage(fromgroup, msg);
            }
        }
        private static string escape(string msg)
        {
            return msg.Replace("&", "&amp;").Replace("[", "&#91;").Replace("]", "&#93;").Replace(",", "&#44;");
        }
        public static void Client_Recieve_Server(string msg)
        {
            string[] data = msg.Split('#');
            if (data.Length > 0)
            {
                if(data.Length==2 && data[0] == "用户")
                {
                    if (data[1] != null)
                    {
                        GeneralControl.Menu_Person_Informations_Class.Instance.Author = JsonConvert.DeserializeObject<Make.MODEL.Author>(data[1]);                        
                    }
                }
                else if(data.Length==2 && data[0] == "获取技能卡")
                {
                    List<SkillCardsModel> skillCardsModels = JsonConvert.DeserializeObject<List<SkillCardsModel>>(data[1]);
                    foreach(SkillCardsModel item in skillCardsModels)
                    {
                        item.Cloud = "云端";
                        GeneralControl.MainMenu.Dispatcher.Invoke((Action)delegate ()
                        {
                            Custom_Card_SkillCard skillCards = (from Custom_Card_SkillCard ienum_item in GeneralControl.MainMenu.CardPanle.CardsPanel.Children where item.ID == ienum_item.SkillCardsModel.ID select ienum_item).FirstOrDefault();
                            if (skillCards!=null)
                            {
                                skillCards.SkillCardsModel.Delete();
                                skillCards.SkillCardsModel = item;
                                skillCards.Cloud.Content = item.Cloud;
                                skillCards.DataContext = skillCards.SkillCardsModel.SkillCards[skillCards.Rate.Value - 1];
                            }
                            else
                            {
                                GeneralControl.MainMenu.CardPanle.Add_Card(item);
                            }
                            item.Add_To_General();
                            item.Save();
                        });                      
                    }
                }
                else if (data.Length == 2 && data[0] == "获取奇遇")
                {
                    List<Adventure> Adventures = JsonConvert.DeserializeObject<List<Adventure>>(data[1]);
                    foreach (Adventure item in Adventures)
                    {
                        item.Cloud = "云端";
                        GeneralControl.MainMenu.Dispatcher.Invoke((Action)delegate ()
                        {
                            IEnumerable<Custom_Card_Adventure> ienum = from Custom_Card_Adventure ienum_item in GeneralControl.MainMenu.AdventurePanle.AdventurePanel.Children where item.ID == ienum_item.AdventureCard.ID select ienum_item;
                            if (ienum.Any())
                            {
                                Custom_Card_Adventure adventure = ienum.First();
                                GeneralControl.Adventures.Remove(adventure.AdventureCard);
                                adventure.AdventureCard = item;
                                adventure.DataContext = item;
                                item.Save();
                            }
                            else
                            {
                                GeneralControl.MainMenu.AdventurePanle.Add_Adventure(item);
                                item.Add_To_General();
                                item.Save();
                            }
                        });
                    }
                }
            }
        }
    }
}
