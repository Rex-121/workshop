using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Tyrant
{
    public class ForgeCraftRequireBoard: MonoBehaviour
    {


        [BoxGroup("slots_prefab")]
        [LabelText("插槽")]
        public GameObject slotPrefab;
        [BoxGroup("slots_prefab")]
        [LabelText("空插槽")]
        public GameObject emptySlotPrefab;
        
        public GridLayoutGroup panel;

        [LabelText("拖拽点")]
        public Transform anchor;
        
        public List<ForgeItemRequiresSlot> slots = new();

        public BehaviorSubject<bool> isMaterialEnough = new(false);
        
        private static BluePrint bluePrint => WorkBenchManager.main.bluePrint;
        private void Start()
        {
            
            var requires = bluePrint.boardLines;

            panel.constraintCount = requires.First().Count();
            
            for (int i = 0; i < requires.Count(); i++)
            {

                var line = requires.ElementAt(i);

                line.ForEach(v =>
                {
                    if (v == 0)
                    {
                        Instantiate(emptySlotPrefab, panel.transform);
                    }
                    else
                    {
                        var gb = Instantiate(slotPrefab, panel.transform).GetComponent<ForgeItemRequiresSlot>();
                        gb.itemDropped += ItemDropped;
                        slots.Add(gb);
                    }
                   

                });

                
            }
        }
        
        
        // 选择的材料
        public IMaterial[] selectedMaterials => slots
            .Where(v => v.hasValue)
            .Select(v => v.itemValue)
            .OfType<IMaterial>()
            .ToArray();

        private void ItemDropped(IItem item)
        {
            Check();
        }

        private void Check()
        {
            var isEnough = bluePrint.IsMaterialEnough(selectedMaterials);
            Debug.Log($"材料是否齐备 {isEnough}");
            isMaterialEnough.OnNext(isEnough);
        }
        
    }
}