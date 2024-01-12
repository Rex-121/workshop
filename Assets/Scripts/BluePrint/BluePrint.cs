using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Tyrant.Items;
using UnityEngine;

namespace Tyrant
{
    public struct BluePrint
    {

        [ShowInInspector]
        public MaterialType[] rawMaterialsRequires;

        public Sprite icon;

        public string board;

        public int make, quality;
        
        public EquipmentSO equipmentSO;

        public BluePrint(IEnumerable<MaterialType> requires, Sprite icon, string board, int make, int quality, BluePrintSO bluePrintSO)
        {
            rawMaterialsRequires = requires.ToArray();
            this.icon = icon;
            this.board = board;
            this.make = make;
            this.quality = quality;
            this.equipmentSO = bluePrintSO.equipmentSO;
        }

        public IEnumerable<IEnumerable<int>> boardLines
        {
            get
            {
                var lines = board.Split(":");
                var all = new IEnumerable<int>[lines.Length];
                for (var i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    var array = new int[line.Length];
                    for (var j = 0; j < line.Length; j++)
                    {
                        array[j] = int.Parse(line.ElementAt(j).ToString());
                    }
                    all[i] = array;
                }

                return all;
            }
        }

        public static BluePrint FromSO(BluePrintSO so)
        {
            return new BluePrint(so.materialRequires, so.icon, so.board, so.makePoints, so.qualityPoints, so);
        }

        public bool IsMaterialEnough(IEnumerable<IMaterial> materials)
        {
            return true;
            // var codes = rawMaterialsRequires.Select(v => v.code);
            // var code = string.Join(":", codes);
            // var other = string.Join(":", materials.Select(v => v.code.Split("-").First()));
            // return code == other;
        }
        
    }
}