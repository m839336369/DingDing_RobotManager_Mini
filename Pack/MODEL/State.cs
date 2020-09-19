using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Make.MODEL
{
    public  class State
    {
        private string name;//状态名称
        private long owner;//状态来源
        private long direct;//状态对象
        private int duration_Round;//持续回合
        private int expire_Round;//到期回合
        private int duration_Immediate;//持续时长
        private DateTime expire_Immediate;//到期时间
        private string message;//状态反馈
        private bool is_Self;//是否自身
        private int effect_mp;//作用范围
        public string Name { get => name; set => name = value; }
        public string Message { get => message; set => message = value; }
        public bool Is_Self { get => is_Self; set => is_Self = value; }
        public int Duration_Round { get => duration_Round; set => duration_Round = value; }
        public int Expire_Round { get => expire_Round; set => expire_Round = value; }
        public int Duration_Immediate
        {
            get => duration_Immediate;
            set 
            { 
                duration_Immediate = value;
                int round = Duration_Immediate / GeneralControl.Menu_GameControl_Class.Instance.Immediate_To_Round;
                if (round > 0) Duration_Round = round;
                else Duration_Round = 1;
            }
        }
        public DateTime Expire_Immediate { get => expire_Immediate; set => expire_Immediate = value; }
        public int Effect_mp { get => effect_mp; set => effect_mp = value; }
        public long Owner { get => owner; set => owner = value; }
        public long Direct { get => direct; set => direct = value; }

        public State Clone()
        {
            return MemberwiseClone() as State;
        }
        public bool Is_Exist(string state_Name, int mp)
        {
            if (Name == state_Name && Effect_mp >= mp) return true;
            return false;
        }
    }
}
