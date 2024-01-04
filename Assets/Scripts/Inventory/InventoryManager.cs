using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
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
                equipments = new Inventory(Storage.main.LoadAllItems("EQUIPMENTS"), "EQUIPMENTS");
                items = new Inventory(Storage.main.LoadAllItems("ITEMS"), "ITEMS");
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }
        
        #endregion


        [ShowInInspector]
        public Inventory equipments;
        [ShowInInspector]
        public Inventory items;

        public BirthPackSO birthPackSO;

        public InventoryBag inventoryBag;

        // public List<Inventory.Slot> all;
        
        // 所有的材料
        public IEnumerable<Inventory.Slot> allMaterials => items.slots;

        public IEnumerable<Inventory.Slot> allEquipments => equipments.slots;
        
        private void Start()
        {
            // storageSO.AddBirth();
        }

        public void AddSlot(Inventory.Slot slot)
        {
            if (slot.item is IMaterial)
            {
                items.slots.Add(slot);
                items.Save();
            }

            if (slot.item is IEquipment)
            {
                equipments.slots.Add(slot);
                equipments.Save();
            }
        }

        public void AddItem(IItem item)
        {
            if (item is IEquipment equipment)
            {
                AddEquipment(equipment);
            }

            if (item is IMaterial)
            {
                items.CollectItem(item);
                
                items.Save();
            }

            if (item is IEquipment)
            {
                equipments.CollectItem(item);
                
                equipments.Save();
            }
            // all.Add(item);
            
            // Storage.main.Save(all.ToArray());
            // inventoryBag.AddItem(item);
            
            // storageSO.items.Add(item);
        }

        public void Remove(Inventory.Slot slot)
        {
            if (slot.item is IMaterial)
            {
                items.Remove(slot);
                items.Save();
            }
            
            if (slot.item is IEquipment)
            {
                equipments.Remove(slot);
                equipments.Save();
            }
        }

        private void AddEquipment(IEquipment equipment)
        {
            Debug.Log($"#背包# 背包+{equipment.itemName}-{equipment.quality.tier}");
        }
        
    }
}
