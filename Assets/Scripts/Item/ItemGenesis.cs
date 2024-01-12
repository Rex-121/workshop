using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Singleton/ItemGenesis", fileName = "ItemGenesis")]
    public class ItemGenesis: SingletonSO<ItemGenesis>
    {
        [Sirenix.OdinInspector.ReadOnly]
        public MaterialSO[] materialSos;
        
        
        public MaterialSO FindMaterialSOById(string id)
        {
            return materialSos.First(v => v.id.ToString() == id);
        }
        
#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), PropertyOrder(-1)]
        public void UpdateMaterialSO()
        {
            materialSos = AssetDatabase.FindAssets("t:MaterialSO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<MaterialSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
        }
#endif
    }
}