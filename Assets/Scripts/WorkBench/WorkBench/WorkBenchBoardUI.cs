using UnityEngine;
using System.Linq;
using Tyrant.UI;
using UnityEngine.UI;

namespace Tyrant
{

    public class WorkBenchBoardUI : MonoBehaviour
    {

        private static WorkBench workBench => WorkBenchManager.main.workBench;


        public GameObject cellOnWorkBenchPrefab;


        public void D(BluePrint blueprint)
        {
        var list = workBench.Start();

        GetComponent<GridLayoutGroup>().constraintCount = blueprint.boardLines.First().Count();

        for (var i = 0; i < list.Count; i++)
        {
            var slot = list[i];
            var gb = Instantiate(cellOnWorkBenchPrefab, transform);
            var cell = gb.GetComponent<SlotOnWorkBench>();
            // cell.handler = this;
            cell.SetCellPosition(slot);
        }
        }

    }
}