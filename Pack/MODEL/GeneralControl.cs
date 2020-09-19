using Native.Sdk.Cqp;
using Newtonsoft.Json;
using Pack.BLL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Make.MODEL
{
    /// <summary>
    /// 总控
    /// </summary>
    public static class GeneralControl
    {
        public static int MaxLevel=5;   //技能卡的最大等级
        public static int MaxStates = 9; //当前状态的最大量
        public static bool LazyLoad_SkillCards = false; //是否惰性加载UI卡片
        public static string directory = System.IO.Directory.GetCurrentDirectory() + "\\app\\仙战";
        public static Socket_Client socket;
        public static CQApi CQApi;
        public static CQLog CQLog;
        public static Pack.MainWindow MainMenu;
        /// <summary>
        /// 技能卡MODEL
        /// </summary>
        public static List<SkillCardsModel> Skill_Cards = new  List<SkillCardsModel>();//总技能卡 //总引用,但UI层还有一层引用，删掉的同时记得删掉UI层
        /// <summary>
        /// 总技能卡
        /// </summary>
        public static Dictionary<string, SkillCard> Skill_Card_Dictionary = new Dictionary<string, SkillCard>(); //总奇遇
        /// <summary>
        /// 总奇遇
        /// </summary>
        public static List<Adventure> Adventures = new  List<Adventure>();//总引用,但UI层还有一层引用，删掉的同时记得删掉UI层
        /// <summary>
        /// 总状态
        /// </summary>
        public static Dictionary<string,State> States = new Dictionary<string,State>();

        public class Menu_Skill_Cards_Class
        {
            static Menu_Skill_Cards_Class()
            {
                Instance = new Menu_Skill_Cards_Class();
            }
            private Menu_Skill_Cards_Class()
            {

            }
            public static Menu_Skill_Cards_Class Instance { get; private set; } = null;
        }
        public class Menu_Adventure_Cards_Class
        {
            static Menu_Adventure_Cards_Class()
            {
                Instance = new Menu_Adventure_Cards_Class();
            }
            private Menu_Adventure_Cards_Class()
            {

            }
            public static Menu_Adventure_Cards_Class Instance { get; private set; } = null;
        }

        public class Menu_Version_Informations_Class
        {
            static Menu_Version_Informations_Class()
            {
                Instance = new Menu_Version_Informations_Class();
            }
            private Menu_Version_Informations_Class()
            {

            }
            public static Menu_Version_Informations_Class Instance { get; private set; } = null;

            public string Expiration_Date { get; set; } = DateTime.Now.ToString();
        }
        public class Menu_Person_Informations_Class : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged = delegate { };
            static Menu_Person_Informations_Class()
            {               
                Instance = new Menu_Person_Informations_Class();
            }
            private Menu_Person_Informations_Class()
            {

            }
            public static Menu_Person_Informations_Class Instance { get; private set; } = null;
            private Author author;
            public Author Author
            {
                get { return author; }
                set
                {
                    if (Author != value)
                    {
                        author = value;
                        PropertyChanged(this, new PropertyChangedEventArgs("Author"));
                    }
                } 
            }
        }

        public class Menu_Lience_Class
        {
            static Menu_Lience_Class()
            {
                Instance = new Menu_Lience_Class();
            }
            private Menu_Lience_Class()
            {

            }
            public static Menu_Lience_Class Instance { get; private set; } = null;
        }
        public class Menu_GameControl_Class
        {
            private static readonly Menu_GameControl_Class _instance = null;
            static Menu_GameControl_Class()
            {
                _instance = new Menu_GameControl_Class();
            }
            private Menu_GameControl_Class()
            {

            }
            public static Menu_GameControl_Class Instance
            {
                get { return _instance; }
            }
            public int Immediate_To_Round { get; set; } = 10;
        }
    }
}
