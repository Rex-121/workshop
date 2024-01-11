using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(fileName = "蓝图")]
    public class BluePrintSO : ScriptableObject
    {

        [LabelText("所需材料")]
        public MaterialSO[] materialSos;

        [PreviewField]
        public Sprite icon;

        public string board;

        /*
         * 00010-
         * 12121-
         * 00010-
         */

        public EquipmentSO equipmentSO;


        public int makePoints;
        public int qualityPoints;

    }
}
