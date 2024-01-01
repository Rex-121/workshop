using System.Linq;
using Sirenix.OdinInspector;

namespace Tyrant
{
    public class HeroEquipments
    {
        [ShowInInspector, InlineProperty, HideLabel, Title("武器")]
        public IEquipment weapon;
        
        public HeroEquipments(IEquipment weapon = null)
        {
            // this.weapon = weapon;
        }

        public Attribute attribute
        {
            get
            {
                var d = new[] {weapon}.FirstOrDefault(v => v != null);
                return d?.attribute ?? new Attribute(0,0,0);
                // if (weapon == null) return new Attribute(0, 0, 0);
                // return weapon.attribute;
            }
        }

        public AttackPower PowerCombine(int attributeLift)
        {
            if (weapon == null) return new AttackPower(attributeLift, attributeLift);
            return weapon.power + attributeLift;
        }

        public void ChangeEquipment(IEquipment equipment)
        {

            if (equipment is Weapon)
            {
                weapon = equipment;
            }
            
        }
        
    }
}