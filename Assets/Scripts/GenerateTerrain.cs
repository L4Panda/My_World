using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour {

    public int width = 20;
    public int height = 20;
    public int depth = 20;

    public float scale = .9f;

    //public float offsetX = 100f;
    //public float offsetY = 100f;

	void Start () {
       
        //offsetX = Rand(0f, 99999f);
        //offsetY = Rand(0f, 99999f);  
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                for(int z = 0; z < depth; z++)
                {
                    GenerateTexture();

                        
                }
            }
        }

	}

		
	void Update () {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
	}

    private float Rand(float min, float max)
    {
        return Random.Range(min, max); // Return a random value
    }

    public static float Perlin3D(float x, float y, float z)
    {
        float AB = Mathf.PerlinNoise(x, y);
        float BC = Mathf.PerlinNoise(y, z);
        float AC = Mathf.PerlinNoise(x, z);

        float BA = Mathf.PerlinNoise(y, x);
        float CB = Mathf.PerlinNoise(z, y);
        float CA = Mathf.PerlinNoise(z, x);

        float ABC = AB + BC + AC + BA + CB + CA;
        return ABC / 6f;
    }

    Texture3D GenerateTexture()
    {
        Color[] colorArray = new Color[width * height * depth];
        Texture3D texture = new Texture3D(width, height, depth, TextureFormat.RGBA32, true);
        texture.filterMode = FilterMode.Point;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for(int z = 0; z < depth; z++)
                {
                    Color c = CalculateColor(x, y, z);
                    colorArray[x + (y * height) + (z * width * depth)] = c;
                }
               
            }
        }
        texture.SetPixels(colorArray);
        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y, int z)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale;
        float zCoord = (float)z / depth * scale;

        float sample = Perlin3D(xCoord, yCoord, zCoord);
        return new Color(sample, sample, sample);
    }
}
