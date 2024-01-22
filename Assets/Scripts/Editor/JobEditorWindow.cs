using System.Linq;
using Editor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Tyrant.Editor
{
    public class JobEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("开发/战斗职业编辑器")]
        private static void Open()
        {
            var window = GetWindow<JobEditorWindow>("战斗职业编辑器");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;
            
            // 初始卡牌列表
            tree.AddAllAssetsAtPath("职业", "Assets/SO/Job", typeof(JobSO));
            
            
            tree.AddAllAssetsAtPath("武器", "Assets/SO/Equipment/Weapons", typeof(WeaponSO));
            
            tree.AddAllAssetsAtPath("Buff", "Assets/SO/Buff", typeof(BuffDataSO));
            
            tree.EnumerateTree().Where(x => x.Value as WeaponSO || x.Value as BuffDataSO).ForEach(AddDragHandles);
            
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
                
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("新Job")))
                {
                    ScriptableObjectCreator.ShowDialog<JobSO>("Assets/SO/Job", obj =>
                    {
                        obj.jobName = obj.name;
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("新武器")))
                {
                    ScriptableObjectCreator.ShowDialog<WeaponSO>("Assets/SO/Equipment/Weapons", obj =>
                    {
                        obj.equipmentName = obj.name;
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
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
