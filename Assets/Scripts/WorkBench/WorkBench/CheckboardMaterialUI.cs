using Sirenix.OdinInspector;
using TMPro;

namespace Tyrant
{
    public class CheckboardMaterialUI: CheckerboardBasicUI
    {

        [ShowInInspector, DisableInEditorMode] public MaterialFeatureSO materialFeatureSO;


        public TextMeshProUGUI materialLabel;

        public override void SetSlot(WorkBenchSlot workBenchSlot)
        {
            base.SetSlot(workBenchSlot);

            materialFeatureSO = slot.materialFeature;
            
            ShowMaterial();
        }

        private void ShowMaterial()
        {
            if (materialFeatureSO == null) return;

            materialLabel.text = materialFeatureSO.featureUsage;

        }
    }
}