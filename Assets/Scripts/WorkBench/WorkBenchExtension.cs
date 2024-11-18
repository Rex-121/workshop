using Sirenix.Utilities;

namespace WorkBench
{
    public static class WorkBenchExtension
    {

        /// <summary>
        /// 清理所有棋盘格
        /// </summary>
        /// <param name="workBench">棋盘</param>
        public static void ClearSlots(this Tyrant.WorkBench workBench)
        {
            workBench.allSlots.ForEach(v => v.Value.DidForgeThisTurn());
        }
        
    }
}