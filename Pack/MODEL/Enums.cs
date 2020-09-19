using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Make.MODEL
{
    /// <summary>
    /// 这里根据enum的特性，虽然类无加载，但其实enum这个枚举表已经存在。
    /// </summary>
    public class Enums
    {
        public enum Room_Type { Solo,Team, Battle_Royale};
        public enum Room {  Wait, Raise, Action, Result };
        public enum Power { Human, Monster, Neutral};//所属势力
        public enum Player_Active { Leisure,Queue,Room, Map };//空闲 队列 回合 地图
        public enum Race { Human };//种族：人类
    }
}
