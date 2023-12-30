using System.Linq;
using Editor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Tyrant.Editor
{
    public class ToolEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("开发/Tool编辑")]
        private static void Open()
        {
            var window = GetWindow<ToolEditorWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }
        
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;


            DungeonEditorWindow.UpdateOverviewList();
            
            
            tree.AddAllAssetsAtPath("所有Tools", "Assets/SO/Tool", typeof(ToolSO));

            tree.AddAllAssetsAtPath("所有ToolBuffs", "Assets/SO/DiceBuff", typeof(DiceBuffDataSO));

            tree.EnumerateTree().Where(x => x.Value as DiceBuffDataSO).ForEach(AddDragHandles);
            
            return tree;
        }
        
        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
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
                
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("新DiceBuff")))
                {
                    ScriptableObjectCreator.ShowDialog<DiceBuffDataSO>("Assets/SO/DiceBuff", obj =>
                    {
                        // obj.buffName = obj.name;
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
                
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("新Tool")))
                {
                    ScriptableObjectCreator.ShowDialog<ToolSO>("Assets/SO/Tool", obj =>
                    {
                        obj.toolName = obj.name;
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
            }
            
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}
