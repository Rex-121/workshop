using UnityEngine;
using UnityEngine.EventSystems;

namespace Tyrant
{
    public class EquipmentDoEquipment: MonoBehaviour, IDropHandler
    {

        private HeroEquipments _equipments;

        public HeroEquipments equipments
        {
            get => _equipments;
            set
            {
                _equipments = value;
                EquipWeapon(equipments.weapon);
            }
        }

        public EquipmentBriefDisplay weaponBriefDisplay;
        
        public void OnDrop(PointerEventData eventData)
        {
            var gb = eventData.pointerDrag;
            
            if (gb.TryGetComponent(out EquipmentDragging equipmentDragging))
            {
                // equipmentDragging.previousParent = transform;

                var slot = equipmentDragging.equipment;
                
                // 与背包物品更换
                InventoryManager.main.Replace(slot, equipments.weapon);
                
                // 装备新物品
                equipments.ChangeEquipment(slot.item as IEquipment);
                
                Destroy(equipmentDragging.gameObject);
                
            }
            
            EquipWeapon(equipments.weapon);
        }


        private void EquipWeapon(IEquipment weapon)
        {
            if (!ReferenceEquals(weaponBriefDisplay, null))
            {
                weaponBriefDisplay.Equipment(weapon);
            }

        }
        
    }
}