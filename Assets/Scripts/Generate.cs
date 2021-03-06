﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Generate : MonoBehaviour {
    public Vector3[] vertices;
    public Vector2[] uvs;
    public int[] triangles;

    public int xSize;
    public int ySize;
    public int zSize;

    private Mesh mesh;

    void Start()
    {
        CreateCube();
    }


    public Vector3 RandVector()
    {
        return new Vector3(Rand(0, 1000), Rand(0, 1000), Rand(0, 1000));
    }

    private void CreateCube()
    {



        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
            }
        }
        mesh.vertices = vertices;

        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }
        mesh.triangles = triangles;
    }

    private float Rand(float min, float max)
    {
        return Random.Range(min, max);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
