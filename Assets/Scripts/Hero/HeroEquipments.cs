using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    public class HeroEquipments
    {
        [ShowInInspector, InlineProperty, HideLabel, Title("武器")]
        public IEquipment weapon;
        
        public HeroEquipments()
        {
            
        }

        public Attribute attribute
        {
            get
            {
                var d = new[] {weapon}.FirstOrDefault(v => v != null);
                return d?.attribute ?? new Attribute(0,0,0);
            }
        }

        public AttackPower PowerCombine(int attributeLift)
        {
            if (weapon == null) return new AttackPower(attributeLift, attributeLift);
            return weapon.power + attributeLift;
        }

        public HeroEquipments ChangeEquipment(IEquipment equipment)
        {

            if (equipment is Weapon)
            {
                weapon = equipment;
                Debug.Log($"#装备# 更换装备{weapon}");
            }

            return this;
        }
        
    }
}