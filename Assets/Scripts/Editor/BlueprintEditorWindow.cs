using System.Linq;
using Editor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Tyrant.Editor
{
    public class BlueprintEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("开发/蓝图编辑器")]
        private static void Open()
        {
            var window = GetWindow<BlueprintEditorWindow>("蓝图编辑器");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }
        
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;


            DungeonEditorWindow.UpdateOverviewList();

            tree.AddAllAssetsAtPath("所有蓝图", "Assets/SO/BluePrints", typeof(BluePrintSO));
            
            tree.AddAllAssetsAtPath("武器", "Assets/SO/Equipment/Weapons", typeof(WeaponSO));

            tree.EnumerateTree().Where(x => x.Value as EquipmentSO).ForEach(AddDragHandles);
            
            return tree;
        }
        
        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }
        
        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }
                
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("新蓝图"))) 
                {
                    ScriptableObjectCreator.ShowDialog<BluePrintSO>("Assets/SO/BluePrints", obj =>
                    {
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}
