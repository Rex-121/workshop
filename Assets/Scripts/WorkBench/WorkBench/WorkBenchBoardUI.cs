using System;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using Tyrant.UI;
using UnityEngine.UI;

namespace Tyrant
{
    
    /*
     * 用于生成铸造台棋盘
     */
    public class WorkBenchBoardUI : MonoBehaviour
    {
        
        public static WorkBenchBoardUI main;
        private void Awake()
        {
            if (main == null)
            {
                main = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private static WorkBench workBench => WorkBenchManager.main.workBench;


        [LabelText("棋盘格Prefab")]
        public CheckerboardUI checkerboardUIPrefab;


        public TextMeshProUGUI cardUsedLabel;
        /// <summary>
        /// 生成棋盘
        /// </summary>
        /// <param name="blueprint">蓝图</param>
        public void GenerateBoard(BluePrint blueprint)
        {
            var list = workBench.Start();

            // 确定棋盘大小
            DefineBoardSize(blueprint);

            for (var i = 0; i < list.Count; i++)
            {
                var slot = list[i];
                var checkerboard = Instantiate(checkerboardUIPrefab, transform);
                // var cell = gb.GetComponent<CheckerboardUI>();
                // cell.handler = this;
                checkerboard.SetSlot(slot);
            }
        }
        
        public void Forge()
        {
            WorkBenchManager.main.DidForgeThisTurn();
        }

        public void UpdateInformation()
        {
            cardUsedLabel.text = $"{WorkBenchManager.main.allOccupiedInThisTurn}/{WorkBenchManager.main.maxWorkBenchOccupied}";
        }

        // 确定棋盘大小
        private void DefineBoardSize(BluePrint blueprint)
        {
            GetComponent<GridLayoutGroup>().constraintCount = blueprint.boardLines.First().Count();
        }

    }
}