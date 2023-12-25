using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Buff/Buff", fileName = "_BuffSO")]
    public class BuffDataSO : SerializedScriptableObject
    {

        public int id;

        public string buffName;

        public string description;


        public Sprite icon;

        public int priority;

        public int maxStack;


        public bool isForever;


        public BuffUpdateTime updateTimeType;
        public BuffRemoveStackUpdate stackRemoveType;



        public IBuffModule onCreate;
        public IBuffModule onRemove;
        public IBuffModule onTick;


        public IBuffModule onHit;
        public IBuffModule onBeHit;
        public IBuffModule onKill;
        public IBuffModule onBeKill;
    }
}
