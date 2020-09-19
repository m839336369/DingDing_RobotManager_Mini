using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Make.MODEL
{
    public class Author
    {
        private long iD=-1;
        private long qQ=-1;
        private string name="玩家昵称";
        private int upgrade_num=0;
        private int create_num=0;
        private string informations="个性签名";

        public long ID { get => iD; set => iD = value; }
        public string Name { get => name; set => name = value; }
        public int Upgrade_num { get => upgrade_num; set => upgrade_num = value; }
        public int Create_num { get => create_num; set => create_num = value; }
        public string Informations { get => informations; set => informations = value; }
        public long QQ { get => qQ; set => qQ = value; }
    }
}
