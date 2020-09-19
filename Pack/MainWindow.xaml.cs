using Make.MODEL;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Newtonsoft.Json;
using Pack.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pack
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public Make.MODEL.Author author;

        public MainWindow()
        {
            GeneralControl.MainMenu = this;           
            InitializeComponent();
            Init();
            BLL.Init init = new Init();
        }

        private void Init()
        {
            UI_Init();
            Menu_Adventure_Cards.DataContext = GeneralControl.Menu_Adventure_Cards_Class.Instance;
            Menu_Lience.DataContext = GeneralControl.Menu_Lience_Class.Instance;
            Menu_Person_Informations.DataContext = GeneralControl.Menu_Person_Informations_Class.Instance;
            Menu_Version_Informations.DataContext = GeneralControl.Menu_Version_Informations_Class.Instance;
            Menu_Skill_Cards.DataContext = GeneralControl.Menu_Skill_Cards_Class.Instance;
            CardPanle.Author.DataContext = author;
        }

        private void SkillCardsPanle_Initialized(object sender, EventArgs e)
        {

        }

        private void SkillCardsPanle_Initialized_1(object sender, EventArgs e)
        {

        }

        private void Menu_Button_3_Click(object sender, RoutedEventArgs e)
        {


        }

        private void Menu_Button_4_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Menu_Button_1_Click(object sender, RoutedEventArgs e)
        {
            Main_TabControl.SelectedIndex = 0;
        }
        private void UI_Init()
        {
            //初始化技能面板
            foreach (SkillCardsModel skillCardsModel in GeneralControl.Skill_Cards)
            {
                CardPanle.Add_Card(skillCardsModel);
            }
            //初始化奇遇
            foreach (Adventure adventure in Make.MODEL.GeneralControl.Adventures)
            {
                AdventurePanle.Add_Adventure(adventure);
            }
        }

        private void Menu_Button_1_Copy4_Click(object sender, RoutedEventArgs e)
        {
            Main_TabControl.SelectedIndex = 3;
        }

        private void Menu_Button_1_Copy5_Click(object sender, RoutedEventArgs e)
        {
            Main_TabControl.SelectedIndex = 4;
        }

        private void Menu_Button_1_Copy_Click(object sender, RoutedEventArgs e)
        {
            Main_TabControl.SelectedIndex = 1;
        }

        private void Menu_Button_1_Copy1_Click(object sender, RoutedEventArgs e)
        {
            Main_TabControl.SelectedIndex = 2;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XY.Send_To_Server("保存用户#" + JsonConvert.SerializeObject(GeneralControl.Menu_Person_Informations_Class.Instance.Author));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            XY.Send_To_Server("用户#" + 3028394801);
        }
    }
}
