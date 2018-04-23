using UnityEditor;
using UnityEngine;

// ---------------------------------------------------------------------------------------------------------------------------
// Part of ProceduralTree [this class is optional - to improve the ProceduralTree inspector, and add the create menu option]

// ---------------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(TreeGenerator))]
public class TreeGeneratorEditor : Editor
{
    GUIStyle iconGUIStyle;
    GUIContent iconGUIContent;

    static readonly int[] NumSides = new[] { 3, 4, 5, 6, 8, 12, 16, 24, 32 };

    // Add an option to generate a new tree under GameObject / Create Procedural menu

    [MenuItem("GameObject/Create Procedural/Procedural Tree")]
    static void CreateProceduralTree()
    {
        var procTree = new GameObject(string.Format("Tree_{0:X4}", Random.Range(0, 65536))).AddComponent<TreeGenerator>();
        procTree.Seed = Random.Range(0, 65536);
        procTree.Update();
        GetSampleMaterial(procTree);
    }
        public override void OnInspectorGUI()
        {

            var tree = (TreeGenerator)target;

            var pt = PrefabUtility.GetPrefabType(tree);
            if (pt != PrefabType.None && pt != PrefabType.DisconnectedPrefabInstance) // Prefabs are not dynamic
            {
                EditorGUILayout.HelpBox("Prefabs are static snapshots of a Procedural Tree. To edit tree parameters, select GameObject > Break Prefab Instance.", MessageType.Info);
                GUI.enabled = false;
                DrawDefaultInspector();
                return;
            }


            DrawDefaultInspector();

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Rand Seed")) // Randomize tree seed
                {
           
                    Undo.RecordObject(tree, "Random seed " + tree.name);
                    tree.Seed = Random.Range(0, 65536);
                    tree.Update();
                }
                if (GUILayout.Button("Rand Tree")) // Randomize all tree parameters
                {
             
                    Undo.RecordObject(tree, "Random tree " + tree.name);
                    Undo.RecordObject(tree.Renderer, "Random tree material " + tree.name);
                    tree.Seed = Random.Range(0, 65536);
                    tree.MaxNumVertices = 1024 << Random.Range(1, 6);
                    tree.NumberOfSides = NumSides[Random.Range(0, NumSides.Length)];
                    tree.BaseRadius = Random.Range(0.25f, 4f);
                    tree.RadiusStep = Random.Range(0.875f, 0.95f);
                    tree.MinimumRadius = Random.Range(0.01f, 0.2f);
                    tree.BranchRoundness = Random.value;
                    tree.SegmentLength = Random.Range(0.25f, 0.75f);
                    tree.Twisting = Random.Range(0f, 40f);
                    tree.BranchProbability = Random.Range(0f, 0.25f);
                    tree.Update();

                    // Randomize material only when tree has no material, or it's using one of the sample ones
                    if (tree.Renderer.sharedMaterial != null)
                    {
                        if (AssetDatabase.GetAssetPath(tree.Renderer.sharedMaterial).StartsWith(GetSampleMaterialsPath(tree)))
                            GetSampleMaterial(tree);
                    }
                    else GetSampleMaterial(tree);

                } 
        
            }
            GUILayout.EndHorizontal();
        }

        Texture2D IconContent(string name)
        {
            System.Reflection.MethodInfo mi = typeof(EditorGUIUtility).GetMethod("IconContent", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, null, new System.Type[] { typeof(string) }, null);
            if (mi == null) mi = typeof(EditorGUIUtility).GetMethod("IconContent", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic, null, new System.Type[] { typeof(string) }, null);
            return (Texture2D)((GUIContent)mi.Invoke(null, new object[] { name })).image;
        }

        static string pathSampleMaterials;

        static string GetSampleMaterialsPath(TreeGenerator tree)
        {
            if (string.IsNullOrEmpty(pathSampleMaterials))
            {
                // Get materials folder
                pathSampleMaterials = AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(tree));
                pathSampleMaterials = System.IO.Path.GetDirectoryName(pathSampleMaterials) + "/SampleMaterials/";
            }
            return pathSampleMaterials;
        }

    static void GetSampleMaterial(TreeGenerator tree)
        {
            // Get sample materials
            string[] sampleMaterials = System.IO.Directory.GetFiles(GetSampleMaterialsPath(tree), "*.mat", System.IO.SearchOption.AllDirectories);
            // Return if none found
            if (sampleMaterials.Length < 1) return; 
            // Assign a random sample material
            tree.Renderer.sharedMaterial = AssetDatabase.LoadAssetAtPath(sampleMaterials[Random.Range(0, sampleMaterials.Length)], typeof(Material)) as Material;
        }
}
