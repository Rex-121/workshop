using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Buff/Buff", fileName = "_BuffSO")]
    public class BuffDataSO : SerializedScriptableObject
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

        // [BoxGroup("Basic")]
        // public bool isForever;

        [BoxGroup("更新机制"), LabelText("叠加机制")]
        public BuffUpdateTime updateTimeType;
        [BoxGroup("更新机制"), LabelText("移除机制")]
        public BuffRemoveStackUpdate stackRemoveType;



        [BoxGroup("回调"), Title("生命周期回调"), LabelText("当Buff生成")]
        public IBuffModule onCreate;
        
        [BoxGroup("回调"), LabelText("当Buff移除")]
        public IBuffModule onRemove;
        // public IBuffModule onTick;

        [BoxGroup("回调"), LabelText("攻击时"), Title("机制回调"), Space(20)]
        public IBuffModule onHit;
        [BoxGroup("回调"), LabelText("被攻击时")]
        public IBuffModule onBeHit;
        [BoxGroup("回调"), LabelText("击杀时")]
        public IBuffModule onKill;
        [BoxGroup("回调"), LabelText("被击杀时")]
        public IBuffModule onBeKill;


        public BuffInfo ToBuff() => new BuffInfo(ScriptableObject.Instantiate(this));
    }
}
