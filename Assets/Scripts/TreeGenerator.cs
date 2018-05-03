using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class TreeGenerator : MonoBehaviour
{
    public const int CurrentVersion = 102;


    public int Seed;
    [Range(1024, 65000)]
    public int MaxNumVertices = 65000;
    [Range(3, 32)]
    public int NumberOfSides = 16;
    [Range(0.25f, 4f)]
    public float BaseRadius = 2f;
    [Range(0.75f, 0.95f)]
    public float RadiusStep = 0.9f;
    [Range(0.01f, 0.2f)]
    public float MinimumRadius = 0.02f;
    [Range(0f, 1f)]
    public float BranchRoundness = 0.8f;
    [Range(0.1f, 2f)]
    public float SegmentLength = 0.5f;
    [Range(0f, 40f)]
    public float Twisting = 20f;
    [Range(0f, 0.25f)]
    public float BranchProbability = 0.1f;

    // ---------------------------------------------------------------------------------------------------------------------------

    float checksum;
    [SerializeField, HideInInspector]
    float checksumSerialized;

    List<Vector3> vertexList;
    List<Vector2> uvList;
    List<int> triangleList;

    float[] ringShape;

    [HideInInspector, System.NonSerialized]
    public MeshRenderer Renderer;
    MeshFilter filter;

    void OnEnable()
    {
        if (filter != null && Renderer != null) return;

        gameObject.isStatic = true;

        filter = gameObject.GetComponent<MeshFilter>();
        if (filter == null) filter = gameObject.AddComponent<MeshFilter>();
        if (filter.sharedMesh != null) checksum = checksumSerialized;
        Renderer = gameObject.GetComponent<MeshRenderer>();
        if (Renderer == null) Renderer = gameObject.AddComponent<MeshRenderer>();
    }

    public void GenerateTree()
    {
        gameObject.isStatic = false;

        var originalRotation = transform.localRotation;
        var originalSeed = Random.seed;

        if (vertexList == null)
        {
            vertexList = new List<Vector3>();
            uvList = new List<Vector2>();
            triangleList = new List<int>();
        }
        else
        {
            vertexList.Clear();
            uvList.Clear();
            triangleList.Clear();
        }

        SetTreeRingShape();

        Random.seed = Seed;


        Branch(new Quaternion(), Vector3.zero, -1, BaseRadius, 0f);

        Random.seed = originalSeed;

        transform.localRotation = originalRotation;

        SetTreeMesh();
    }


    private void SetTreeMesh()
    {

        var mesh = filter.sharedMesh;
        if (mesh == null)
            mesh = filter.sharedMesh = new Mesh();
        else
            mesh.Clear();

        mesh.vertices = vertexList.ToArray();
        mesh.uv = uvList.ToArray();
        mesh.triangles = triangleList.ToArray();


        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        ;
    }

//------------------------------------------------------------------------------------------------------------------

        void Branch(Quaternion quaternion, Vector3 position, int lastRingVertexIndex, float radius, float texCoordV)
        {
            var offset = Vector3.zero;
            var texCoord = new Vector2(0f, texCoordV);
            var textureStepU = 1f / NumberOfSides;
            var angInc = 2f * Mathf.PI * textureStepU;
            var ang = 0f;

            // Add ring vertices
            for (var n = 0; n <= NumberOfSides; n++, ang += angInc)
            {
                var r = ringShape[n] * radius;
                offset.x = r * Mathf.Cos(ang); // Get X, Z vertex offsets
                offset.z = r * Mathf.Sin(ang);
                vertexList.Add(position + quaternion * offset); // Add Vertex position
                uvList.Add(texCoord); // Add UV coord
                texCoord.x += textureStepU;
            }

            if (lastRingVertexIndex >= 0) // After first base ring is added ...
            {
                // Create new branch segment quads, between last two vertex rings
                for (var currentRingVertexIndex = vertexList.Count - NumberOfSides - 1; currentRingVertexIndex < vertexList.Count - 1; currentRingVertexIndex++, lastRingVertexIndex++)
                {
                    triangleList.Add(lastRingVertexIndex + 1); // Triangle A
                    triangleList.Add(lastRingVertexIndex);
                    triangleList.Add(currentRingVertexIndex);
                    triangleList.Add(currentRingVertexIndex); // Triangle B
                    triangleList.Add(currentRingVertexIndex + 1);
                    triangleList.Add(lastRingVertexIndex + 1);
                }
            }

            
            radius *= RadiusStep;
            if (radius < MinimumRadius || vertexList.Count + NumberOfSides >= MaxNumVertices) // End branch if reached minimum radius, or ran out of vertices
            {
               
                vertexList.Add(position); 
                uvList.Add(texCoord + Vector2.one); 
                for (var n = vertexList.Count - NumberOfSides - 2; n < vertexList.Count - 2; n++)
                {
                    triangleList.Add(n);
                    triangleList.Add(vertexList.Count - 1);
                    triangleList.Add(n + 1);
                }
                return;
            }

     
            texCoordV += 0.0625f * (SegmentLength + SegmentLength / radius);
            position += quaternion * new Vector3(0f, SegmentLength, 0f);
            transform.rotation = quaternion;
            var x = (Random.value - 0.5f) * Twisting;
            var z = (Random.value - 0.5f) * Twisting;
            transform.Rotate(x, 0f, z);
            lastRingVertexIndex = vertexList.Count - NumberOfSides - 1;
            Branch(transform.rotation, position, lastRingVertexIndex, radius, texCoordV); // Next segment

        
            if (vertexList.Count + NumberOfSides >= MaxNumVertices || Random.value > BranchProbability) return;

           
            transform.rotation = quaternion;
            x = Random.value * 70f - 35f;
            x += x > 0 ? 10f : -10f;
            z = Random.value * 70f - 35f;
            z += z > 0 ? 10f : -10f;
            transform.Rotate(x, 0f, z);
            Branch(transform.rotation, position, lastRingVertexIndex, radius, texCoordV);
        }

        // ---------------------------------------------------------------------------------------------------------------------------
        // Try to get shared mesh for new prefab instances
        // ---------------------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        bool CanGetPrefabMesh()
        {
            // Return false if we are not instancing a new procedural tree prefab
            if (PrefabUtility.GetPrefabType(this) != PrefabType.PrefabInstance) return false;
            if (filter.sharedMesh != null) return true;

            // Try to get mesh from an existing instance
            var parentPrefab = PrefabUtility.GetPrefabParent(this);
            var list = (TreeGenerator[])FindObjectsOfType(typeof(TreeGenerator));
            foreach (var go in list)
                if (go != this && PrefabUtility.GetPrefabParent(go) == parentPrefab)
                {
                    filter.sharedMesh = go.filter.sharedMesh;
                    return true;
                }
            return false;
        }
#endif

        // ---------------------------------------------------------------------------------------------------------------------------
        // Set tree shape, by computing a random offset for every ring vertex
        // ---------------------------------------------------------------------------------------------------------------------------

        void SetTreeRingShape()
        {
            ringShape = new float[NumberOfSides + 1];
            var k = (1f - BranchRoundness) * 0.5f;
            // Randomize the vertex offsets, according to BranchRoundness
            Random.seed = Seed;
            for (var n = 0; n < NumberOfSides; n++) ringShape[n] = 1f - (Random.value - 0.5f) * k;
            ringShape[NumberOfSides] = ringShape[0];
        }

        // ---------------------------------------------------------------------------------------------------------------------------
        // Update function will return, unless the tree parameters have changed
        // ---------------------------------------------------------------------------------------------------------------------------

        public void Update()
        {
            // Tree parameter checksum (add any new parameters here!)
            var newChecksum = (Seed & 0xFFFF) + NumberOfSides + SegmentLength + BaseRadius + MaxNumVertices +
                RadiusStep + MinimumRadius + Twisting + BranchProbability + BranchRoundness;

            // Return (do nothing) unless tree parameters change
            if (newChecksum == checksum && filter.sharedMesh != null) return;

            checksumSerialized = checksum = newChecksum;

#if UNITY_EDITOR
            if (!CanGetPrefabMesh())
#endif
                GenerateTree(); // Update tree mesh
        }

        // ---------------------------------------------------------------------------------------------------------------------------
        // Destroy procedural mesh when object is deleted
        // ---------------------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        void OnDisable()
        {
            if (filter.sharedMesh == null) return; // If tree has a mesh
            if (PrefabUtility.GetPrefabType(this) == PrefabType.PrefabInstance) // If it's a prefab instance, look for siblings
            {
                var parentPrefab = PrefabUtility.GetPrefabParent(this);
                var list = (TreeGenerator[])FindObjectsOfType(typeof(TreeGenerator));
                foreach (var go in list)
                    if (go != this && PrefabUtility.GetPrefabParent(go) == parentPrefab)
                        return; // Return if there's another prefab instance still using the mesh
            }
            DestroyImmediate(filter.sharedMesh, true); // Delete procedural mesh
        }
#endif
    }

