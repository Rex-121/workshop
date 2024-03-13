using UnityEngine;

namespace Tyrant
{
    public class CheckerboardBasicUI: MonoBehaviour
    {

        protected WorkBenchSlot slot;

        public virtual void SetSlot(WorkBenchSlot workBenchSlot)
        {
            slot = workBenchSlot;
            
            if (slot == null || !CanSlotTypeExecuteEvent())
            {
                Destroy(this);
            }

            SlotPrepared();
        }

        public virtual void SlotPrepared()
        {
            
        }

        private bool CanSlotTypeExecuteEvent()
        {
            return slot.toolWrapper.type != WorkBench.SlotType.Empty;
        }
        
    }
}