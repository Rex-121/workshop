using System.Linq;
using Editor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;


namespace Editor
{
    public class SingletonEditorWindow: OdinMenuEditorWindow
    {
        [MenuItem("开发/单例")]
        private static void Open()
        {
            var window = GetWindow<SingletonEditorWindow>("单例查看");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }


        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;
            tree.AddAllAssetsAtPath("", "Assets/Resources/Singleton");
            return tree;
        }
    }
}