using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(fileName = "材料")]
    public class MaterialSO : ScriptableObject
    {


        public string name;


        public Sprite icon;

    
        public IMaterial toMaterial => new Tyrant.Material(name, icon);

    }
}
