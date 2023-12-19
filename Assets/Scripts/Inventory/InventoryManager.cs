using System;
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

        private void Start()
        {

            var materials = birthPackSO.materials.Select(v => v.toRawMaterial.toMaterial);

            materials.ForEach(AddItem);
        }

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
