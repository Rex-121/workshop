using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Buff/DiceBuff", fileName = "_DiceBuffSO")]
    public class DiceBuffDataSO : SerializedScriptableObject
    {

        [BoxGroup("Basic"), PreviewField(60), HideLabel, HorizontalGroup("Basic/Info", 60), PropertyOrder(-1)]
        public Sprite icon;

        [BoxGroup("Basic"), VerticalGroup("Basic/Info/Info"), LabelText("优先级"), SuffixLabel("数值越大越先触发", true)]
        public int priority;

        // 在tool里生成时调用，未实装
        [BoxGroup("回调"), Title("生命周期回调"), LabelText("当Buff生成")]
        public IDiceBuffModel onCreate;
        // 放置在棋盘格上生效
        [BoxGroup("回调"), LabelText("放入Slot时"), Title("机制回调"), Space(20)]
        public IDiceBuffMathModel onPin;
        [BoxGroup("回调"), LabelText("使用(计算buff点数时)")]
        public IDiceBuffMathModel onUse;
        
        [BoxGroup("回调"), LabelText("当Buff移除")]
        public IDiceBuffModel onRemove;

        [SerializeField]
        public IBuffEffectOn effectOnLocation;

        public interface IBuffEffectOn
        {
            public IEnumerable<WorkBenchSlot> AllEffect(Vector2Int pivot,
                Dictionary<WorkBench.ToolWrapper, WorkBenchSlot> all);
            
            public Vector2Int[] effectOnSlot { get; }
        }


        public IEnumerable<WorkBenchSlot> AllEffect(Vector2Int pivot,
            Dictionary<WorkBench.ToolWrapper, WorkBenchSlot> all)
        {
            return effectOnLocation == null ? all
                .Where(v => v.Value.toolWrapper.position == pivot)
                .Select(v => v.Value) : effectOnLocation.AllEffect(pivot, all);
        }
        
        
        [System.Serializable]
        public struct DefaultEffectOnLocation: IBuffEffectOn
        {
            [ShowInInspector] public Vector2Int[] effectOn;

            public Vector2Int[] effectOnSlot => effectOn;

            public IEnumerable<WorkBenchSlot> AllEffect(Vector2Int pivot, Dictionary<WorkBench.ToolWrapper, WorkBenchSlot> all)
            {
                var effects = effectOn.Select(v => pivot + v).ToArray();

                return all
                    .Where(v => effects.Contains(v.Key.position))
                    .Select(v => v.Value);
            }
        }

        public DiceBuffInfo ToBuff() => new DiceBuffInfo(ScriptableObject.Instantiate(this));
    }
}