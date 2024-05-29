using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolFestival_Raking_System.Sources
{
    internal class Data
    {
        //ランキング管理に必要なデータを処理します。

        public List<RankingData> Flying_Distance_Data = new List<RankingData>();
        public List<RankingData> Flying_Time_Data = new List<RankingData>();

        public class RankingData
        {
            public string Name;
            public int Score;
        }

        public void Sort_List(ref List<RankingData> datas)
        {
            datas.Sort((a, b) => {
                return (b.Score - a.Score);
                });
        }
    }
}
