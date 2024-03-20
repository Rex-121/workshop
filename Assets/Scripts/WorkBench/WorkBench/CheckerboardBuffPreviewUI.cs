using TMPro;
using UniRx;

namespace Tyrant
{
    /// <summary>
    /// 用于棋盘格显示buff预览
    /// </summary>
    public class CheckerboardBuffPreviewUI: CheckerboardBasicUI
    {
        public TextMeshProUGUI buffLabel;
        
        public override void SlotPrepared()
        {
            base.SlotPrepared();
            
            slot.previewBuffHandler.previewScore
                .Subscribe(Display)
                .AddTo(this);
        }

        
        private void Display(int value)
        {
            buffLabel.text = $"+ {value}";
            buffLabel.enabled = value != 0;
        }
    }
}