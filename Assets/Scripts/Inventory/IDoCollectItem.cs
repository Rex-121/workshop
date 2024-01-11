using System.Collections.Generic;
using UnityEngine;

namespace Tyrant
{
    public interface IDoCollectItem
    {
        // 收集物品
        public void CollectItem(IItem item);
    }


    public abstract class BaseDoCollect: IDoCollectItem
    {
        public virtual void CollectItem(IItem item)
        {
            Debug.Log($"#背包收集# {item}");
        }
    }
}