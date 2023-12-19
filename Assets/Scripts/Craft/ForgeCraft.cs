using UnityEngine;

namespace Tyrant
{
    public class ForgeCraft 
    {



        public IEquipment Craft(int makes, int quality)
        {
            return M();
        }



        IEquipment M()
        {
            return new Equipment("Weapon");
        }



        public static IEquipment DoCraft(int makes, int quality)
        {
            return new ForgeCraft().Craft(makes, quality);
        }
        
    }
}
