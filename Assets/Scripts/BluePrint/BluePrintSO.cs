using Sirenix.OdinInspector;
using Tyrant.Items;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(fileName = "蓝图")]
    public class BluePrintSO : ScriptableObject
    {

        [LabelText("所需材料")]
        public MaterialType[] materialRequires;

        [PreviewField(60)]
        public Sprite icon;

        

        /*
         * 00010-
         * 12121-
         * 00010-
         */
        public string board;
        
        public EquipmentSO equipmentSO;


        public int makePoints;
        public int qualityPoints;

    }
}
