using System.Linq;
using Editor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Tyrant.Editor
{
    public class BuffEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("开发/Buff编辑")]
        private static void Open()
        {
            var window = GetWindow<BuffEditorWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }
        
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;


            DungeonEditorWindow.UpdateOverviewList();
            
            
            tree.Add("所有Buff", new BuffTable(BuffOverview.Instance.buffDataSos));
            
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
                
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("新Buff")))
                {
                    ScriptableObjectCreator.ShowDialog<BuffDataSO>("Assets/SO/Buff", obj =>
                    {
                        obj.buffName = obj.name;
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}
