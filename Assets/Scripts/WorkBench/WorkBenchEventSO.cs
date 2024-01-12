using System;
using Sirenix.OdinInspector;
using Tyrant;
using UnityEngine;
using UnityEngine.Events;

namespace WorkBench
{
    
    /*
     * 1. 准备蓝图
     * 2. PrepareNewRound
     * 3. NewTurn
     * 4. DidForgeThisTurn
     * 5. 计算分数 ♾️️
     */
    [CreateAssetMenu(menuName = "Events/骰子熔铸台", fileName = "熔铸台事件")]
    public class WorkBenchEventSO: SerializedScriptableObject
    {
        // 1. 准备蓝图
        public UnityAction<BluePrint> blueprintDidSelected;
        // 2. PrepareNewRound
        public UnityAction<int> prepareNewRound;
        // 3. NewTurn
        public UnityAction<int> newTurnDidStarted;
        // 4. DidForgeThisTurn
        public UnityAction<int> turnDidEnded;
        // 计算分数
        public UnityAction<int, int> scoreDidChange;
        // 4. DidForgeThisTurn
        public UnityAction<int> roundDidEnded;

        public void BlueprintDidSelected(BluePrint bluePrint) => blueprintDidSelected?.Invoke(bluePrint);
        public void PrepareNewRound() => prepareNewRound?.Invoke(0);
        public void NewTurnDidStarted() => newTurnDidStarted?.Invoke(0);
        public void TurnDidEnded() => turnDidEnded?.Invoke(0);
        public void ScoreDidChange(int make, int quality) => scoreDidChange?.Invoke(make, quality);
        public void RoundDidEnd() => roundDidEnded?.Invoke(0);

    }
}