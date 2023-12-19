using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(fileName = "蓝图")]
    public class BluePrintSO : ScriptableObject
    {

        [LabelText("所需材料")]
        public MaterialSO[] materialSos;

        public Sprite icon;
        
    }
}
