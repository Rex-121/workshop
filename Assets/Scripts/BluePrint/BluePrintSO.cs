using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(fileName = "蓝图")]
    public class BluePrintSO : SerializedScriptableObject
    {

        [PreviewField(60), HideLabel, HorizontalGroup("basic", 60)]
        public Sprite icon;

        /*
         * 00010-
         * 12121-
         * 00010-
         */
        [HideLabel, HorizontalGroup("basic"), VerticalGroup("basic/info")]
        public string board;
        
        [HideLabel, HorizontalGroup("basic"), VerticalGroup("basic/info")]
        public EquipmentSO equipmentSO;

        private AnimationCurve _animationCurve;

        [BoxGroup("制作")]
        public int makePoints;
        [BoxGroup("制作")]
        public int qualityPoints;
        
        
        [LabelText("所需材料")]
        public MaterialRequiresGroup materialRequires;


        public void D()
        {
            // _animationCurve[]
        }
    }
}
