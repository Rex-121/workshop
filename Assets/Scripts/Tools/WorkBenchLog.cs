using System;
using Tyrant;
using Unity.VisualScripting;

namespace WorkBench
{
    public struct WorkBenchLog: ILog
    {

        public WorkBenchLog(WorkBenchStatus s)
        {
            _status= s;
        }
        private readonly WorkBenchStatus _status;

        public string title => "WorkBench";
        public string message => _status.Description();
    }


    public static class WorkBenchStatusExt
    {
        public static string Description(this WorkBenchStatus o)
        {
            return o switch
            {
                WorkBenchStatus.Begin => "(1)开始锻造",
                WorkBenchStatus.AddMaterial => "(2)加入材料",
                WorkBenchStatus.AddAddons => "(3)加入配件",
                WorkBenchStatus.RoundBegin => "(4)回合开始",
                WorkBenchStatus.Score => "(5)开始计算分数",
                WorkBenchStatus.RoundEnd => "(6)回合结束",
                WorkBenchStatus.Item => "(7)产出物品",
                WorkBenchStatus.End => "(8)结束锻造",
                _ => "ERROR"
            };
        }

        public static void Log(this WorkBenchStatus o)
        {
            TLog.Log(new WorkBenchLog(o));
        }
    }
    
    /**
     * 	1. 开始锻造
        2. 加入材料
        3. 加入配件
        4. 回合开始
        5. 开始计算分数
        6. 回合结束
        7. 产出物品
        8. 结束锻造
     */
    public enum WorkBenchStatus
    {
        Begin,
        AddMaterial,
        AddAddons,
        RoundBegin,
        Score,
        RoundEnd,
        Item,
        End
    }
    
    
}