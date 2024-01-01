using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tyrant
{
    public class EquipmentInventorySlot: MonoBehaviour
    {


        
        public EquipmentDragging equipmentDraggingPrefab;

        public void Refresh(IEquipment e, Canvas canvas, Transform pointToDrag)
        {
            var slot = Instantiate(equipmentDraggingPrefab, transform);
            slot.canvas = canvas;
            slot.equipment = e;
            slot.pointToDrag = pointToDrag;

        }

    }
}