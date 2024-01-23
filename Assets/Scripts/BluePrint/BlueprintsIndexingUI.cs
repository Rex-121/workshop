using System;
using System.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Tyrant.UI
{
    public class BlueprintsIndexingUI: MonoBehaviour
    {

        public BluePrint[] allBluePrints;

        public BlueprintInspectorUI blueprintPrefab;
        public Transform panel;


        [LabelText("材料需求板")]
        public ForgeCraftRequireBoard requireBoardPrefab;

        private ForgeCraftRequireBoard _m;

        public Button forgeButton;
        
        void Start()
        {
            allBluePrints = BluePrintGenesis.main.allBlueprints;
            
            foreach (var allBluePrint in allBluePrints)
            {
                var gb = Instantiate(blueprintPrefab, panel);
                gb.NewBlueprint(allBluePrint);
                gb.selection = SelectedBp;
            }

            SelectedBp(allBluePrints.First());
        }


        public void DoForge()
        {
            WorkBenchManager.main.StartAWorkBench(_m.selectedMaterials);
            Destroy(gameObject);
        }

        private void SelectedBp(BluePrint bp)
        {
            if (_m != null)
            {
                Destroy(_m.gameObject);
            }
            
            WorkBenchManager.main.bluePrint = bp;
            
            _m = Instantiate(requireBoardPrefab, transform);

            _m.isMaterialEnough
                .Subscribe(v =>
                {
                    forgeButton.interactable = v;
                }).AddTo(_m);
        }
        
    }
}