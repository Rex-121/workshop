using System.Linq;
using Editor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Tyrant.Editor
{
    public class DungeonEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("开发/Dungeon编辑")]
        private static void Open()
        {
            var window = GetWindow<DungeonEditorWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        [MenuItem("开发/更新数据列表")]
        public static void UpdateOverviewList()
        {
            DungeonsOverview.Instance.UpdateCardsOverview();
            BuffOverview.Instance.UpdateBuffOverview();
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;


            UpdateOverviewList();
            
            // 初始卡牌列表
            tree.AddAllAssetsAtPath("Dungeons", "Assets/SO/Dungeon", typeof(DungeonSO));
            
            tree.Add("所有Dungeons", new DungeonsTable(DungeonsOverview.Instance.dungeonSos));
            //
            // tree.AddAllAssetsAtPath("所有卡牌", "Assets/SO/Cards", typeof(DungeonSO), true);
            //
            // tree.EnumerateTree().Where(x => x.Value as AllCardsLike).ForEach(AddDragHandles);

            tree.AddAllAssetsAtPath("Enemies", "Assets/SO/Enemy", typeof(EnemySO), true);

            
            tree.AddAllAssetsAtPath("Loot", "Assets/SO/Loot", typeof(LootSO), true);
            
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
                
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("新Dungeon")))
                {
                    ScriptableObjectCreator.ShowDialog<DungeonSO>("Assets/SO/Dungeon", obj =>
                    {
                        obj.dungeonName = obj.name;
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("新敌人")))
                {
                    ScriptableObjectCreator.ShowDialog<EnemySO>("Assets/SO/Enemy", obj =>
                    {
                        obj.enemyName = obj.name;
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("新Loot")))
                {
                    ScriptableObjectCreator.ShowDialog<LootSO>("Assets/SO/Loot", obj =>
                    {
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}
