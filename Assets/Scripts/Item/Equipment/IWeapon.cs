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
        
        [ReadOnly, HideLabel]
        [VerticalGroup("Basic/VBasic")]
        public string id;
        
        [ShowInInspector]
        [HideLabel]
        [PreviewField(60, ObjectFieldAlignment.Left)]
        [HorizontalGroup("Basic", 60)]
        public Sprite sprite => _weaponSO.icon;
        
        [SerializeField]
        [HideLabel]
        [VerticalGroup("Basic/VBasic")]
        public string itemName { get; set; }
        
       
        [SerializeField, VerticalGroup("Basic/VBasic"), HideLabel]
        public Quality quality { get; set; }

        [PropertyOrder(100)] public Attribute attribute => _attribute.LiftByQuality(quality);

        // [ShowInInspector, VerticalGroup("Basic/Basic"), HideLabel]
        public AttackPower power => _powerFromBluePrint.LiftByQuality(quality);

        private WeaponSO _weaponSO => EquipmentGenesis.main.FindWeaponSOById(id);
        
        [SerializeField]
        private AttackPower _powerFromBluePrint;
        
        [SerializeField]
        private Attribute _attribute;
        
        
        public Weapon(string name, Attribute attribute, string id, AttackPower powerFromBluePrint, Quality qualities)
        {
            _attribute = attribute;

            // this.sprite = sprite;
            this.id = id;

            _powerFromBluePrint = powerFromBluePrint;
            
            itemName = name;

            quality = qualities;
        }
        
        public static Weapon FromSwordSO(WeaponSO weaponSO)
        {
            return new Weapon(weaponSO.equipmentName, weaponSO.attribute, weaponSO.id.ToString(), new AttackPower(weaponSO.power.x, weaponSO.power.y), new Quality());
        }

        public IEquipment LiftByQuality(IQuality qualities)
        {
            return new Weapon(itemName, attribute, id, power, qualities.quality);
        }
    }
    
}