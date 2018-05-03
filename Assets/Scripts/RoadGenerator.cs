using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadGenerator : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> verts;
    List<int> tris;
    List<Vector3> norms;
    Vector3[] points;
    RandomSpawn point;
    Vector3 start;
    Vector3 end;
    Vector3 startDirection;
    Vector3 endDirection;



    private void Start()
    {
        mesh = new Mesh();
        verts = new List<Vector3>();
        tris = new List<int>();
        norms = new List<Vector3>();

        int numPoints = Random.Range(100, 200);
        points = new Vector3[numPoints];
        point = GetComponent<RandomSpawn>();
    
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = point.GetRandomPoint();
        }

        start = GetPoint(0f);
        startDirection = GetDirection(0f);
        
        Quaternion rotation = GetRotation(0);
        Vector3 left = rotation * Vector3.left;
        Vector3 right = rotation * Vector3.right;
        Vector3 up = rotation * Vector3.up;
        verts.Add(start + right);
        verts.Add(start + left);
        norms.Add(up);
        norms.Add(up);
        int triIndex = 0;

        int size = 10; 
        for (int i = 0; i <= size; i++)
        {
            float t = (float)i / (float)size;
            end = GetPoint(t);
            endDirection = GetDirection(t);
            rotation = GetRotation(t);

            left = rotation * Vector3.left;
            right = rotation * Vector3.right;
            up = rotation * Vector3.up;

            verts.Add(end + right);
            verts.Add(end + left);
            norms.Add(up);
            norms.Add(up);

            tris.Add(triIndex);
            tris.Add(triIndex + 1);
            tris.Add(triIndex + 2);

            tris.Add(triIndex + 2);
            tris.Add(triIndex + 1);
            tris.Add(triIndex + 3);

            triIndex += 2;

            start = end;
        }

        mesh.SetVertices(verts);
        mesh.SetNormals(norms);
        mesh.SetTriangles(tris, 0);
        GetComponent<MeshFilter>().mesh = mesh;

    }

    public int CurveCount
    {
        get
        {
            return (points.Length - 1) / 3;
        }
    }

    private Quaternion GetRotation(float t)
    {
        return Quaternion.LookRotation(GetDirection(t), Vector3.up);
    }

    public Vector3 GetPoint(float t)
    {
        return Bezier.GetPoint(start, start + startDirection, end - endDirection, end, t);
    }

    public Vector3 GetVelocity(float t)
    {
        return Bezier.GetFirstDerivative(start, start + startDirection, end - endDirection, end, t);
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }
    

}