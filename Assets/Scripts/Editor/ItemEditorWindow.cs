using System.Linq;
using Editor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Tyrant.Editor
{
    public class ItemEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("开发/材料编辑")]
        private static void Open()
        {
            var window = GetWindow<ItemEditorWindow>("材料编辑", true);
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 500);
        }
        
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;


            DungeonEditorWindow.UpdateOverviewList();
            
 
            // 所有材料
            tree.AddAllAssetsAtPath("材料", "Assets/SO/Materials", typeof(MaterialSO));
            
            
            // 所有材料
            tree.AddAllAssetsAtPath("特性", "Assets/SO/Materials", typeof(MaterialFeatureSO));
            
            return tree;
        }
        
        
        protected override void OnBeginDrawEditors()
        {
            if (this.MenuTree == null) return;
            
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }
                
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("新Item")))
                {
                    ScriptableObjectCreator.ShowDialog<MaterialSO>("Assets/SO/Materials", obj =>
                    {
                        obj.materialName = obj.name;
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
                
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("新特性")))
                {
                    ScriptableObjectCreator.ShowDialog<MaterialFeatureSO>("Assets/SO/Materials", obj =>
                    {
                        obj.featureName = obj.name;
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}
