using UnityEngine;
using UnityEngine.EventSystems;

namespace Tyrant
{
    public class EquipmentDoEquipment: MonoBehaviour, IDropHandler
    {

        public HeroEquipments equipments;
        
        public void OnDrop(PointerEventData eventData)
        {
            var gb = eventData.pointerDrag;


            if (gb.TryGetComponent(out EquipmentDragging equipmentDragging))
            {
                equipmentDragging.previousParent = transform;
                equipments.ChangeEquipment(equipmentDragging.equipment);
            }
        }
    }
}