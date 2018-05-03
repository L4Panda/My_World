using UnityEngine;
using System.Collections;

public static class NoiseSphere
{

    public enum NormalizeMode { Local, Global };

    public static float[,,] GenerateNoiseMap(int mapWidth, int mapHeight, int mapDepth, int seed, float scale, int octaves, float persistance, float lacunarity, Vector3 offset, NormalizeMode normalizeMode)
    {
        float[,,] noiseMap = new float[mapWidth, mapHeight, mapDepth];

        System.Random prng = new System.Random(seed);
        Vector3[] octaveOffsets = new Vector3[octaves];

        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) - offset.y;
            float offsetZ = prng.Next(-100000, 100000) - offset.x + offset.y;
            octaveOffsets[i] = new Vector3(offsetX, offsetY, offsetZ);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxLocalNoiseHeight = float.MinValue;
        float minLocalNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;
        float halfDepth = mapDepth / 2f;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; x < mapHeight; y++)
            {
                for (int z = 0; z < mapDepth; z++)
                {
                    amplitude = 1;
                    frequency = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
                        float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;
                        float sampleZ = (z - halfDepth + octaveOffsets[i].z) / scale * frequency;

                        float perlinValue = Perlin3D(sampleX, sampleY, sampleZ) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxLocalNoiseHeight)
                    {
                        maxLocalNoiseHeight = noiseHeight;
                    }
                    else if (noiseHeight < minLocalNoiseHeight)
                    {
                        minLocalNoiseHeight = noiseHeight;
                    }
                    noiseMap[x, y, z] = noiseHeight;
                }
            }
        }

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; x < mapHeight; y++)
            {
                for (int z = 0; z < mapDepth; z++)
                {
                    if (normalizeMode == NormalizeMode.Local)
                    {
                        noiseMap[x, y, z] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y, z]);
                    }
                    else
                    {
                        float normalizedHeight = (noiseMap[x, y, z] + 1) / (maxPossibleHeight / 0.9f);
                        noiseMap[x, y, z] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                    }
                }
            }
        }
        return noiseMap;
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
}
