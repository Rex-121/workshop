using System;
using System.Collections.Generic;
using System.Linq;
using Item.Material;
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
                equipments = new Inventory(
                    Storage.main.LoadAllItems("EQUIPMENTS"),
                    "EQUIPMENTS",
                    item => item is IEquipment
                    );
                items = new Inventory(
                    Storage.main.LoadAllItems("ITEMS"),
                    "ITEMS", 
                    item => item is IMaterial
                    );

                // 获取初始物品
                GetBirthPackIfNeeded();
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

        private IEnumerable<Inventory> allInventories => new[] {items, equipments};


        [Button]
        public void GetBirthPackIfNeeded()
        {
            if (SaveManager.main.normalSettings.birthPackDelivered) return;
            birthPackSO.materials
                .Select(v => v.ToMaterial(Quality.Fine, new AtLeastOneFeatureGenerate()))
                .ForEach(AddItem);
            SaveManager.main.DoDeliverBirthPack();
        }
        
        
        public Inventory InventoryBy(Inventory.Type by)
        {
            return by switch
            {
                Inventory.Type.Equipment => equipments,
                Inventory.Type.Item => items,
                _ => items
            };
        }
        
        // private Dictionary<int, Inventory.Slot> allItems => items.slots;
        public void AddSlot(Inventory.Slot slot) => allInventories.AddSlot(slot);

        public void AddItem(IItem item) => allInventories.CollectItem(item);

        public void Remove(Inventory.Slot slot) => allInventories.RemoveSlot(slot);
        
        public void SwapSlot(Inventory.Slot a, Inventory.Slot? b, int toIndex) => allInventories.SwapSlot(a, b, toIndex);


        public void Replace(Inventory.Slot slot, IItem item) => allInventories.Replace(slot, item);

    }
}
