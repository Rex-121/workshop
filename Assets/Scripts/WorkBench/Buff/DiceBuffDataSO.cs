using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Buff/DiceBuff", fileName = "_DiceBuffSO")]
    public class DiceBuffDataSO : SerializedScriptableObject
    {

        [BoxGroup("Basic"), HorizontalGroup("Basic/Info/Info/Info"), SuffixLabel("技能ID", true), HideLabel]
        public int id;

        [BoxGroup("Basic"), HorizontalGroup("Basic/Info/Info/Info"), SuffixLabel("技能名称",true), HideLabel]
        public string buffName;

        [BoxGroup("Basic")]
        public string description;

        [BoxGroup("Basic"), PreviewField(60), HideLabel, HorizontalGroup("Basic/Info", 60), PropertyOrder(-1)]
        public Sprite icon;

        [BoxGroup("Basic"), VerticalGroup("Basic/Info/Info"), LabelText("优先级"), SuffixLabel("数值越大越先触发", true)]
        public int priority;

        [BoxGroup("Basic"), VerticalGroup("Basic/Info/Info"), LabelText("最大堆叠层数")]
        public int maxStack;

        [BoxGroup("Basic"), VerticalGroup("Basic/Info/Info"), LabelText("起始层数")]
        public int startWithStack;

        [BoxGroup("更新机制"), LabelText("叠加机制")]
        public BuffUpdateTime updateTimeType;
        [BoxGroup("更新机制"), LabelText("移除机制")]
        public BuffRemoveStackUpdate stackRemoveType;



        [BoxGroup("回调"), Title("生命周期回调"), LabelText("当Buff生成")]
        public IDiceBuffModel onCreate;
        [BoxGroup("回调"), LabelText("放入Slot时"), Title("机制回调"), Space(20)]
        public IDiceBuffModel onPin;
        [BoxGroup("回调"), LabelText("使用")]
        public IDiceBuffMathModel onUse;
        // [BoxGroup("回调"), LabelText("使用")]
        // public IDiceBuffMathModel onUse;
        [BoxGroup("回调"), LabelText("当Buff移除")]
        public IDiceBuffModel onRemove;
        // [BoxGroup("回调"), LabelText("当Buff移除")]
        // public IDiceBuffMathModel onDiceFaceChanged;
        // [BoxGroup("回调"), LabelText("被击杀时")]
        // public IDiceBuffModel onBeKill;

        [SerializeField]
        public IBuffEffectOn effectOnLocation;// = new EffectOnLocation();

        public interface IBuffEffectOn
        {
            public IEnumerable<WorkBenchSlot> AllEffect(Vector2Int pivot,
                Dictionary<WorkBench.ToolWrapper, WorkBenchSlot> all);
        } 
        
        [System.Serializable]
        public struct DefaultEffectOnLocation: IBuffEffectOn
        {
            public Vector2Int[] effectOn;

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