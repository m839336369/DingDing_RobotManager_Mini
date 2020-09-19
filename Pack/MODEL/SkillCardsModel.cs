using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Make.MODEL
{
    /// <summary>
    /// 内含一种技能卡的不同等级
    /// </summary>

    public class SkillCardsModel 
    {

        private SkillCard[] skillCards;
        private int iD;
        public SkillCard[] SkillCards { get => skillCards; set => skillCards = value; }
        private long author_ID;
        public int ID { get => iD; set => iD = value; }
        public long Author_ID { get => author_ID; set => author_ID = value; }
        public string Cloud { get => cloud; set => cloud = value; }

        private string cloud = "非云端";
        public SkillCardsModel()
        {
            ID = GetHashCode();
        }
        public SkillCardsModel(SkillCard[] Bind)
        {
            skillCards = Bind;
            ID = GetHashCode();
            foreach (SkillCard item in Bind) item.Father_ID = ID;
        }
        public void Save()
        {
            string json = JsonConvert.SerializeObject(this);
            string filepath = GeneralControl.directory + "\\技能卡\\" + ID + ".json";
            File.WriteAllText(filepath, json);
        }
        public void Delete()
        {
            string filepath = GeneralControl.directory + "\\技能卡\\" + ID + ".json";
            GeneralControl.Skill_Cards.Remove(this);
            foreach (SkillCard item in skillCards)
            {
                GeneralControl.Skill_Card_Dictionary.Remove(item.Name);
            }
            File.Delete(filepath);
        }
        public void Add_To_General()
        {
            foreach (SkillCard skill in skillCards)
            {
                while ((from SkillCard item in GeneralControl.Skill_Card_Dictionary.Values where item.Name == skill.Name select item).Any()) skill.Name = skill.Name + "-副本";
                GeneralControl.Skill_Card_Dictionary.Add(skill.Name,skill);
            }
            GeneralControl.Skill_Cards.Add(this);     
        }
    }
}
