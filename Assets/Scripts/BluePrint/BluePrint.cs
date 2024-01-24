using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Tyrant.Items;
using UnityEngine;

namespace Tyrant
{
    public struct BluePrint
    {

        [ShowInInspector, LabelText("所需材料")]
        public MaterialRequiresGroup requires;

        public Sprite icon;

        public string board;

        public int make, quality;
        
        public EquipmentSO equipmentSO;

        public BluePrint(MaterialRequiresGroup requires, Sprite icon, string board, int make, int quality, BluePrintSO bluePrintSO)
        {
            this.requires = requires;
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

        public bool IsMaterialEnough(IMaterial[] materials)
        { 
            var count = materials.Count();
            // 如果少于最小数量 或者 大于最大材料数量
            if (count < requires.minRequiresCount || count > requires.maxCount) return false;


            var e = requires.ConditionsForMinRequires(materials.ToList());
            
            if (!e.Item1) return false;
            
            return true;
        }
        
    }


    [Serializable]
    public struct MaterialRequiresGroup
    {
        [SerializeField]
        public MaterialRequires[] rawMaterialsRequires;

        [ShowInInspector, LabelText("必需材料")]
        private MaterialRequires[] requires => rawMaterialsRequires
            .Where(v => v.require)
            .ToArray();
        
        
        [ShowInInspector, LabelText("可选材料")]
        private MaterialRequires[] options => rawMaterialsRequires
            .Where(v => !v.require)
            .ToArray();
        
        [ShowInInspector, LabelText("材料最小数量")]
        public int minRequiresCount => requires
            .Sum(v => v.min);
        
        [ShowInInspector, LabelText("材料最大数量")]
        public int maxCount => rawMaterialsRequires
            .Sum(v => v.max);



        // 满足最小的制作材料需求
        public Tuple<bool, List<IMaterial>> ConditionsForMinRequires(List<IMaterial> materials)
        {
            var k = materials;

            var veryRequire = requires;

            for (var x = 0; x < veryRequire.Count(); x++)
            {
                var require = veryRequire[x];

                var requireCount = 0;

                for (var i = k.Count() - 1; i >= 0; i--)
                {
                    if (!require.types.HasFlag(k[i].type)) continue;
                    requireCount++;
                    k.RemoveAt(i);
                    if (requireCount == require.min) break;
                }
                
                if (requireCount < require.min) return new Tuple<bool, List<IMaterial>>(false, k);
            }

            return new Tuple<bool, List<IMaterial>>(true, k);
        }
    }


    [Serializable]
    public struct MaterialRequires
    {
        
        [HorizontalGroup("A"), HideLabel, VerticalGroup("A/C")]
        public MaterialType types;
        
        
        [LabelText("是否必需"), HorizontalGroup("A"), VerticalGroup("A/C")]
        public bool require;

        [HorizontalGroup("A"), VerticalGroup("A/B"), LabelText("最小数量")]
        public int min;
        [HorizontalGroup("A"), VerticalGroup("A/B"), LabelText("最大数量")]
        public int max;
        

    }
}