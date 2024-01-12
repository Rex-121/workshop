using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant.UI
{
    public class BlueprintsIndexingUI: MonoBehaviour
    {

        public BluePrint[] allBluePrints;

        public BlueprintInspectorUI blueprintPrefab;
        public Transform panel;


        public GameObject mat;

        private GameObject _m;
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
            WorkBenchManager.main.StartAWorkBench();
            Destroy(gameObject);
        }

        private void SelectedBp(BluePrint bp)
        {
            if (_m != null)
            {
                Destroy(_m);
            }
            
            WorkBenchManager.main.bluePrint = bp;
            
            _m = Instantiate(mat, transform);
        }
        
    }
}