#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using Sirenix.OdinInspector.Editor;
    using System.Linq;
    using UnityEngine;
    using Sirenix.Utilities.Editor;
    using Sirenix.Serialization;
    using UnityEditor;
    using Sirenix.Utilities;
    using UnityEngine.SceneManagement;

    public class TreeEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/Tree Editor Window")]
        private static void OpenWindow()
        {
            var window = GetWindow<TreeEditorWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        private CreateDialogueData createDialogueData;
        private TestButton testButton;
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if(createDialogueData != null)
                DestroyImmediate(createDialogueData.dialogueData);
        }

        protected override OdinMenuTree BuildMenuTree()
        {


            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: false)
            //{
            //    { "Home",                           this,                           EditorIcons.House                       }, // Draws the this.someData field in this case.
            //    { "Odin Settings",                  null,                           SdfIconType.GearFill                    },
            //    { "Odin Settings/Color Palettes",   ColorPaletteManager.Instance,   SdfIconType.PaletteFill                 },
            //    { "Odin Settings/AOT Generation",   AOTGenerationConfig.Instance,   EditorIcons.SmartPhone                  },
            //    { "Player Settings",                Resources.FindObjectsOfTypeAll<PlayerSettings>().FirstOrDefault()       },

            //};
            ;
            // tree.AddAllAssetsAtPath("Odin Settings/More Odin Settings", "Plugins/Sirenix", typeof(ScriptableObject), true).AddThumbnailIcons();
            // tree.AddAssetAtPath("Odin Getting Started", "Plugins/Sirenix/Getting Started With Odin.asset");
            //tree.MenuItems.Insert(2, new OdinMenuItem(tree, "Menu Style", tree.DefaultMenuStyle));
            //tree.Add("Menu/Items/Are/Created/As/Needed", new GUIContent());
            //tree.Add("Menu/Items/Are/Created", new GUIContent("And can be overridden"));

          
            tree.AddAllAssetsAtPath("DialogueData", "Dialogues",typeof(DialogueData));
            var item = tree.MenuItems[0];
            for(int i = 0; i < item.ChildMenuItems.Count; i++)
            {
                var item1 = item.ChildMenuItems[i];

                if(item1.Value is DialogueData)
                {
                    DialogueData dd = item1.Value as DialogueData;
                    int num = 1;
                    for(int j = 0; j < dd.phrases.Length; j++)
                    {
                        string slash = "/";
                        if(dd.phrases[j].AddSelection || j > 0 && dd.phrases[j - 1].AddSelection)
                        {
                            slash = "/выбор "+ num +" / ";
                            if(!dd.phrases[j].AddSelection) num++;
                        }
 
                        tree.Add(item1.GetFullPath()+ slash + dd.phrases[j].name, dd.phrases[j]);
                    }
                }
            }
            //---------------------------------------------------------------------------
            tree.SortMenuItemsByName();

            createDialogueData = new CreateDialogueData();
            tree.Add("Добавить диалог", createDialogueData);


            testButton = new(this);

            tree.Add("Распечатка", testButton);

            return tree;
        }
        public void Refresh() {
            BuildMenuTree();
        }
        protected override void OnBeginDrawEditors()
        {
            OdinMenuTreeSelection selected = this.MenuTree.Selection;
            if(selected.SelectedValue is DialogueData)
            {
                SirenixEditorGUI.BeginHorizontalToolbar();
                {
                    GUILayout.FlexibleSpace();
                    if(SirenixEditorGUI.ToolbarButton("Удалить диалог"))
                    {
                        DialogueData asset = selected.SelectedValue as DialogueData;
                        string path = AssetDatabase.GetAssetPath(asset);
                        AssetDatabase.DeleteAsset(path);
                        AssetDatabase.SaveAssets();
                    }

                }
                SirenixEditorGUI.EndHorizontalToolbar();
            }
            if(selected.SelectedValue is PhraseData)
            {
                SirenixEditorGUI.BeginHorizontalToolbar();
                {
                    GUILayout.FlexibleSpace();
                    if(SirenixEditorGUI.ToolbarButton("Дублировать фразу"))
                    {
                        PhraseData asset = selected.SelectedValue as PhraseData;
                        string path = AssetDatabase.GetAssetPath(asset);
                        AssetDatabase.CopyAsset(path, "Assets/Dialogues/" + asset + "- 1.asset");
                        AssetDatabase.SaveAssets();
                    }
                    GUILayout.FlexibleSpace();
                    if(SirenixEditorGUI.ToolbarButton("Удалить фразу"))
                    {
                        PhraseData asset = selected.SelectedValue as PhraseData;
                        string path = AssetDatabase.GetAssetPath(asset);
                        AssetDatabase.DeleteAsset(path);
                        AssetDatabase.SaveAssets();
                    }
                }
                SirenixEditorGUI.EndHorizontalToolbar();


            }

        }

        public class CreateDialogueData
        {
            public CreateDialogueData()
            {

                dialogueData = ScriptableObject.CreateInstance<DialogueData>();
                _dialogueName = "New Dialogue Data";

            }


            public string _dialogueName;
            public string _dalogsPath = "Assets/Dialogues/";
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public DialogueData dialogueData;

            [Button("Добавить новый диалог")]
            private void CreateNewData()
            {
                AssetDatabase.CreateAsset(dialogueData, "Assets/Dialogues/" + _dialogueName + ".asset");
                AssetDatabase.SaveAssets();
                dialogueData = ScriptableObject.CreateInstance<DialogueData>();
            }
        }
        public class TestButton
        {
            TreeEditorWindow _tree;
            public TestButton(TreeEditorWindow tree)
            {
                _tree = tree;


            }
            //[InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            //public DialogueData dialogueData;

            [Button("Обновить")]
            private void PrintMenuItems()
            {
                _tree.Refresh();
                Debug.Log("UpdateMenuTree");

            }

           
        }



    }
}
#endif

