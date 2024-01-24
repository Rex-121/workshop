using UnityEngine;
using UnityEngine.Rendering;

namespace Tyrant
{
    public class CardPlacementMono: MonoBehaviour
    {

        public Canvas canvas;


        public SortingGroup sortingGroup;

        public void SetIndex(int index)
        {

            sortingGroup.sortingOrder = 100 - index;

            canvas.sortingOrder = 100 - index;
        }

    }
}