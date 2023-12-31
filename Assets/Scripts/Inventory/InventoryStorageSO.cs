using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(fileName = "S")]
    public class InventoryStorageSO: SerializedScriptableObject
    {
        
        public BirthPackSO birthPackSos;

        public bool isAlreadyAddBirth = false;
        
        
        [Button(ButtonSizes.Large), PropertyOrder(9)]
        public void Clear()
        {
            isAlreadyAddBirth = false;
            items.Clear();
        }
        
        [Space]
        [PropertyOrder(10)]
        public List<IItem> items = new();


        public void AddBirth()
        {
            if (isAlreadyAddBirth) return;
            items.AddRange(birthPackSos.materials.Select(v => v.toRawMaterial.toMaterial));
            isAlreadyAddBirth = true;
        }
        
    }
}