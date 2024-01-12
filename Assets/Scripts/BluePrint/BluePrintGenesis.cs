using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Tyrant
{    
    [CreateAssetMenu(menuName = "Singleton/BluePrintGenesis", fileName = "BluePrintGenesis")]
    public class BluePrintGenesis: SingletonSO<BluePrintGenesis>
    {
        [ShowInInspector]
        [ReadOnly]
        public BluePrintSO[] bluePrintSos { get; private set; }


        [ShowInInspector]
        public BluePrint[] allBlueprints => bluePrintSos
            .Select(BluePrint.FromSO)
            .ToArray();

        private void OnValidate()
        {
            UpdateBluePrintSO();
        }

#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), PropertyOrder(-1)]
        public void UpdateBluePrintSO()
        {
            bluePrintSos = AssetDatabase.FindAssets("t:BluePrintSO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<BluePrintSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
        }
#endif

    }
}