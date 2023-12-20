using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace Tyrant
{
    public class InventoryManager : MonoBehaviour
    {
        #region 单例

        public static InventoryManager main;
        private void Awake()
        {
            if (main == null)
            {
                main = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }
        
        #endregion

        public BirthPackSO birthPackSO;

        public InventoryBag inventoryBag;
        
        
        // 所有的材料
        public IEnumerable<IItem> allMaterials => birthPackSO.materials.Select(v => v.toRawMaterial.toMaterial);

        public void AddItem(IItem item)
        {
            if (item is IEquipment equipment)
            {
                AddEquipment(equipment);
            }
            
            inventoryBag.AddItem(item);
        }

        private void AddEquipment(IEquipment equipment)
        {
            Debug.Log($"背包+{equipment.itemName}");
        }
        
    }
}
