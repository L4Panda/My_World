using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnSpace : MonoBehaviour {

    public GameObject[] prefabs;

    public RaycastHit hit; // Used to get information about Raycast (detects collisions and colliders)
    public GameObject parent;
    private Vector3 randomPoint; // (x,y,z)
    private Mesh mesh;
    private Vector3[] verts; // Vector3 array of the mesh vertices
    private Vector3[] norms; // Vectpr3 array of the mesh normales
    private int[] tris; // Integer array of the triangle indices of the mesh
    GameObject prefab;
    Texture2D TextureMap;
    private Vector3 randPos;

    //------------------------------------------------------------------
    void Start()
    {
        int numSpawns = (int)Rand(300, 500); // Number of objects to spawn on the mesh
        
        // What kind of objects will spawn?
        for (int i = 0; i < numSpawns; i++)
        {
            prefab = prefabs[(int)Rand(0, prefabs.Length)];
            randPos = GetRandomPoint(); // Generated random position
            var pfn = "P" + i;
            Gen(pfn, randPos);

        }
    }

    //------------------------------------------------------------------
    private void Gen(string name, Vector3 pos)
    {
        /// This function generates the prefab in a random postion on a mesh
        /// using the GetRandomPoint() function

        var go = Instantiate(prefab, randPos, Quaternion.identity); // Generate the prefab using the random position and world position
        var v = transform.eulerAngles;
        v.y = Rand(0, 360);
        go.transform.eulerAngles = v;
        go.transform.parent = parent.transform;
        go.transform.localScale = new Vector3(1f, 1f, 1f);
        go.name = name; // Name the prefab
    }
    //------------------------------------------------------------------
    private float Rand(float min, float max)
    {
        return Random.Range(min, max); // Return a random value using the Range function in the Random class
    }
    //------------------------------------------------------------------
    public Vector3 GetRandomPoint()
    {
        return new Vector3(Rand(-5000,5000),Rand(-5000,5000),Rand(-5000, 5000));
    }
    //------------------------------------------------------------------

}