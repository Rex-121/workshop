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
        
        public List<IItem> items = new();


        public void AddBirth()
        {
            if (isAlreadyAddBirth) return;
            items.AddRange(birthPackSos.materials.Select(v => v.toRawMaterial.toMaterial));
            isAlreadyAddBirth = true;
        }
        
    }
}