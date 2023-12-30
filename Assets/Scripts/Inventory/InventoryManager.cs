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

        public InventoryStorageSO storageSO;
        
        // 所有的材料
        public IEnumerable<IItem> allMaterials => storageSO.items;//birthPackSO.materials.Select(v => v.toRawMaterial.toMaterial);


        private void Start()
        {
            storageSO.AddBirth();
        }

        public void AddItem(IItem item)
        {
            if (item is IEquipment equipment)
            {
                AddEquipment(equipment);
            }
            
            inventoryBag.AddItem(item);
            
            storageSO.items.Add(item);
        }

        private void AddEquipment(IEquipment equipment)
        {
            Debug.Log($"背包+{equipment.itemName}");
        }
        
    }
}
