using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Singleton/ItemGenesis", fileName = "ItemGenesis")]
    public class ItemGenesis: SingletonSO<ItemGenesis>
    {
        [ReadOnly]
        public MaterialSO[] materialSos;
        
        [ReadOnly]
        public MaterialFeatureSO[] materialFeatureSOs;
        
        public MaterialSO FindMaterialSOById(string id)
        {
            return materialSos.First(v => v.id.ToString() == id);
        }
        
        
        public MaterialFeatureSO FindMaterialFeatureSOByName(string name)
        {
            return materialFeatureSOs.First(v => v.name.ToString() == name);
        }
        
        
#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), PropertyOrder(-1)]
        public void UpdateMaterialSO()
        {
            materialSos = AssetDatabase.FindAssets("t:MaterialSO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<MaterialSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
            
            materialFeatureSOs = AssetDatabase.FindAssets("t:MaterialFeatureSO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<MaterialFeatureSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
        }
#endif
    }
}