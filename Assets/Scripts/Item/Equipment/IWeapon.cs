using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    public interface IWeapon: IEquipment
    {

        // public Attribute attribute { get; }
        //
        // public AttackPower power { get; }
    }


    [System.Serializable, HideReferenceObjectPicker]
    public struct Weapon : IWeapon
    {
        
        [SerializeField, PreviewField(60, ObjectFieldAlignment.Left), HorizontalGroup("Basic", 60), HideLabel]
        public Sprite sprite { get; set; }

        
        [SerializeField, VerticalGroup("Basic/Basic"), HideLabel]
        public string itemName { get; set; }
        
       
        [SerializeField, VerticalGroup("Basic/Basic"), HideLabel]
        public Quality quality { get; set; }

        [SerializeField, PropertyOrder(100)] public Attribute attribute => _attribute.LiftByQuality(quality);

        [ShowInInspector, VerticalGroup("Basic/Basic"), HideLabel]
        public AttackPower power => _powerFromBluePrint.LiftByQuality(quality);
        private AttackPower _powerFromBluePrint;
        public Attribute _attribute;
        
        
        public Weapon(string name, Attribute attribute, Sprite sprite, AttackPower powerFromBluePrint, Quality qualities)
        {
            _attribute = attribute;

            this.sprite = sprite;

            _powerFromBluePrint = powerFromBluePrint;
            
            itemName = name;

            quality = qualities;
        }
        
        public static Weapon FromSwordSO(WeaponSO weaponSO)
        {
            return new Weapon(weaponSO.equipmentName, weaponSO.attribute, weaponSO.icon, new AttackPower(weaponSO.power.x, weaponSO.power.y), new Quality());
        }

        public IEquipment LiftByQuality(IQuality qualities)
        {
            return new Weapon(itemName, attribute, sprite, power, qualities.quality);
        }
    }
    
}