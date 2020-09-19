using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Make.MODEL
{
    public class SkillCard
    {
        private string name="";//技能卡名称
        private int level;//技能卡等级
        private int father_ID;//父卡类
        private string description="";//技能介绍
        private int need_Mp;//所需能量
        private int probability;//概率
        private int attack;//攻击力
        private int cure;//治疗量
        private int self_Mp;//自我能量
        private int direct_Mp;//指向能量
        private List<State> effect_States=new List<State>();//状态
        private long owner;
        private bool is_Magic;//是否魔法
        private bool is_Physics;//是否物理
        private bool is_Self;//是否释放于自己
        private bool is_Cure;//是否治疗
        private bool is_Attack;//是否攻击
        private bool is_Eternal;//是否永恒
        private bool is_Basic;//是否基础卡组
        private int state=1;//技能卡状态(0 禁用 1 开启 2售卖)
        private string messages="";//技能反馈
        private int amount;//技能卡数量
        private DateTime date_Latest;
        private int attack_Number=1;
        public string Name 
        { 
            get => name;
            set
            {
                if (GeneralControl.Skill_Card_Dictionary.ContainsKey(name) && GeneralControl.Skill_Card_Dictionary.ContainsValue(this))
                {
                    if (GeneralControl.Skill_Card_Dictionary.ContainsKey(value))
                    {
                        Name = value + "-副本";
                        return;
                    }
                    else
                    {
                        GeneralControl.Skill_Card_Dictionary.Remove(name);
                        GeneralControl.Skill_Card_Dictionary.Add(value, this);
                    }
                }
                name = value;
            }
        }
        public bool Is_Basic { get => is_Basic; set => is_Basic = value; }
        public int Level { get => level; set => level = value; }
        public int Amount { get => amount; set => amount = value; }
        public int Need_Mp { get => need_Mp; set => need_Mp = value; }
        public int Probability { get => probability; set => probability = value; }
        public int Attack { get => attack; set => attack = value; }
        public int Cure { get => cure; set => cure = value; }
        public int Self_Mp { get => self_Mp; set => self_Mp = value; }
        public int Direct_Mp { get => direct_Mp; set => direct_Mp = value; }
        public List<State> Effect_States { get => effect_States; set => effect_States = value;}
        public bool Is_Magic { get => is_Magic; set => is_Magic = value; }
        public string Messages { get => messages; set => messages = value; }
        public int State { get => state; set => state = value; }
        public string Description { get => description; set => description = value; }
        public int Attack_Number { get => attack_Number; set => attack_Number = value; }
        public bool Is_Self { get => is_Self; set => is_Self = value; }
        public long Owner { get => owner; set => owner = value; }
        public bool Is_Cure { get => is_Cure; set => is_Cure = value; }
        public bool Is_Attack { get => is_Attack; set => is_Attack = value; }
        public bool Is_Eternal { get => is_Eternal; set => is_Eternal = value; }
        public bool Is_Physics { get => is_Physics; set => is_Physics = value; }
        public int Father_ID { get => father_ID; set => father_ID = value; }
        public DateTime Date_Latest { get => date_Latest; set => date_Latest = value; }
        public void SetName(string skill_name)
        {
            name = skill_name;
        }
    }
}
