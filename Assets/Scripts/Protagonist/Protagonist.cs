using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Singleton/Protagonist", fileName = "主角")]
    public class Protagonist: SingletonSO<Protagonist>
    {


        [LabelText("制作物品回合数")]
        public int stamina;


        [LabelText("卡组容量"), BoxGroup("卡牌")]
        public int maxCardDeckCapacity = 10;

        [LabelText("初始手牌数量"), BoxGroup("卡牌")]
        public int genesisCardAmount = 5;

        [LabelText("每回合抽牌数量"), BoxGroup("卡牌")] public int drawPerTurn = 1;
    }
}
