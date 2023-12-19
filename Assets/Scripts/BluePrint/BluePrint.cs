using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    public struct BluePrint
    {

        [ShowInInspector]
        public RawMaterial[] rawMaterialsRequires;

        public Sprite icon;

        public BluePrint(IEnumerable<RawMaterial> requires, Sprite icon)
        {
            rawMaterialsRequires = requires.ToArray();
            this.icon = icon;
        }

        public static BluePrint FromSO(BluePrintSO so)
        {
            return new BluePrint(so.materialSos.Select(v => v.toRawMaterial), so.icon);
        }

        public bool IsMaterialEnough(IEnumerable<IMaterial> materials)
        {
            var codes = rawMaterialsRequires.Select(v => v.code);
            var code = string.Join(":", codes);
            var other = string.Join(":", materials.Select(v => v.code.Split("-").First()));
            return code == other;
        }
        
    }
}