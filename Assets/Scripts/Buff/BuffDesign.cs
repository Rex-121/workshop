using Sirenix.OdinInspector;

namespace Tyrant
{


    [System.Serializable]
    public enum BuffUpdateTime
    {
        [LabelText("叠加")]
        Add,
        [LabelText("替换")]
        Replace,
        [LabelText("保持")]
        Keep
    }


    [System.Serializable]
    public enum BuffRemoveStackUpdate
    {
        [LabelText("清空")]
        Clear,
        
        [LabelText("减少层数")]
        Reduce
    }
    
    
}