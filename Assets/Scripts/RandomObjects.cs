using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjects : MonoBehaviour {

    public GameObject[] collectables;

    public RaycastHit hit; // Used to get information about Raycast (detects collisions and colliders)
    public GameObject parent;
    private Vector3 randomPoint; // (x,y,z)
    private Mesh mesh;
    private Vector3[] verts; // Vector3 array of the mesh vertices
    private Vector3[] norms; // Vectpr3 array of the mesh normales
    private int[] tris; // Integer array of the triangle indices of the mesh

    //Texture2D TextureMap;
    GameObject collectable;

    //------------------------------------------------------------------
    void Start()
    {
        // Objects to spawn
        for (int i = 0; i < collectables.Length; i++)
        {
            collectable = collectables[i];
            
            var pfn = "P" + i;
            Gen(pfn);
            
        }
    }
    //------------------------------------------------------------------
    private void Gen(string name)
    {
        /// This function generates the prefab in a random postion on a mesh
        /// using the GetRandomPoint() function
        var randPos = GetRandomPoint(); // Generated random position

        if(collectable.tag == "Beach")
            {
                if(randPos.y >= 1)
                {
                randPos = GetRandomPoint();
                }
            }


        var go = Instantiate(collectable, randPos, Quaternion.identity); // Generate the prefab using the random position and world position
        var v = transform.eulerAngles;
        v.y = Rand(0, 360);
        go.transform.eulerAngles = v;
        go.transform.parent = parent.transform;
        go.name = name; // Name the prefab
    }
    //------------------------------------------------------------------
    private float Rand(float min, float max)
    {
        return Random.Range(min, max); // Return a random value
    }
    //------------------------------------------------------------------
    public Vector3 GetRandomPoint()
    {
        /// This function calculates a random point on a mesh 
        /// using the barycentric coordinates of a triangle on the mesh to calculate the weighted average
        /// which is within the bounds of the triangle

        /* Mesh Info */
        mesh = GetComponent<MeshFilter>().mesh; // Get the mesh of the main object to spawn things on (terrain mesh)
        verts = mesh.vertices; // Vertices of the mesh
        tris = mesh.triangles; // Triangles of the mesh
        norms = mesh.normals; // Normals of the mesh

        /* Generate random barycentric coordinates */
        Vector3 BC;
        BC.x = Rand(0, 1);
        BC.y = Rand(0, 1 - BC.x);
        BC.z = Rand(0, 1 - BC.x - BC.y);
        hit.barycentricCoordinate = BC;

        /* Determine weighted average using triangle vertices */
        int triIndex = (int)Rand(0, tris.Length); // Index of triangle

        // Find vertices of triangle
        Vector3 v1 = verts[tris[triIndex]];
        Vector3 v2 = verts[tris[triIndex + 1]];
        Vector3 v3 = verts[tris[triIndex + 2]];

        // Calculate weighted average
        hit.point = transform.TransformPoint(v1 * BC.x + v2 * BC.y + v3 * BC.z) / (BC.x + BC.y + BC.z);

        /* Calculate normals */
        Vector3 n1 = norms[tris[triIndex]];
        Vector3 n2 = norms[tris[triIndex + 1]];
        Vector3 n3 = norms[tris[triIndex + 2]];

        hit.normal = n1 * BC.x + n2 * BC.y + n3 * BC.z;

        return hit.point; // Random point on mesh

    }
    //------------------------------------------------------------------

}