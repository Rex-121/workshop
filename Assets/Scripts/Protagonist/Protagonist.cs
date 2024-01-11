using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Singleton/Protagonist", fileName = "主角")]
    public class Protagonist: SingletonSO<Protagonist>
    {


        [LabelText("制作物品回合数")]
        public int stamina;


    }
}
