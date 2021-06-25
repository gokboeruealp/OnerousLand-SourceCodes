using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapFunctions
{
    public static int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (empty)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = 1;
                }
            }
        }
        return map;
    }

    public static void RenderMap(int[,] map, Tilemap tilemap, TileBase tile)
    {
        tilemap.ClearAllTiles();
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }
    }

    public static void RenderMapWithOffset(int[,] map, Tilemap tilemap, TileBase tile, Vector2Int offset)
    {
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x + offset.x, y + offset.y, 0), tile);
                }
            }
        }
    }

    public static IEnumerator RenderMapWithDelay(int[,] map, Tilemap tilemap, TileBase tile)
    {
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    yield return null;
                }
            }
        }
    }

    public static void UpdateMap(int[,] map, Tilemap tilemap)
    {
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (map[x, y] == 0)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
        }
    }

    public static int[,] PerlinNoise(int[,] map, float seed)
    {
        int newPoint;
        float reduction = 0.5f;
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            newPoint = Mathf.FloorToInt((Mathf.PerlinNoise(x, seed) - reduction) * map.GetUpperBound(1));

            newPoint += (map.GetUpperBound(1) / 2);
            for (int y = newPoint; y >= 0; y--)
            {
                map[x, y] = 1;
            }
        }
        return map;
    }

    public static int[,] PerlinNoiseSmooth(int[,] map, float seed, int interval)
    {
        if (interval > 1)
        {
            int newPoint, points;
            float reduction = 0.5f;

            Vector2Int currentPos, lastPos;
            List<int> noiseX = new List<int>();
            List<int> noiseY = new List<int>();

            for (int x = 0; x < map.GetUpperBound(0); x += interval)
            {
                newPoint = Mathf.FloorToInt((Mathf.PerlinNoise(x, (seed * reduction))) * map.GetUpperBound(1));
                noiseY.Add(newPoint);
                noiseX.Add(x);
            }

            points = noiseY.Count;

            for (int i = 1; i < points; i++)
            {
                currentPos = new Vector2Int(noiseX[i], noiseY[i]);
                lastPos = new Vector2Int(noiseX[i - 1], noiseY[i - 1]);

                Vector2 diff = currentPos - lastPos;

                float heightChange = diff.y / interval;
                float currHeight = lastPos.y;

                for (int x = lastPos.x; x < currentPos.x; x++)
                {
                    for (int y = Mathf.FloorToInt(currHeight); y > 0; y--)
                    {
                        map[x, y] = 1;
                    }
                    currHeight += heightChange;
                }
            }
        }
        else
        {
            map = PerlinNoise(map, seed);
        }

        return map;
    }
    public static int[,] PerlinNoiseCave(int[,] map, float modifier, bool edgesAreWalls)
    {
        int newPoint;
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {

                if (edgesAreWalls && (x == 0 || y == 0 || x == map.GetUpperBound(0) - 1 || y == map.GetUpperBound(1) - 1))
                {
                    map[x, y] = 1;
                }
                else
                {
                    newPoint = Mathf.RoundToInt(Mathf.PerlinNoise(x * modifier, y * modifier));
                    map[x, y] = newPoint;
                }
            }
        }
        return map;
    }

    public static int[,] RandomWalkTop(int[,] map, float seed)
    {
        System.Random rand = new System.Random(seed.GetHashCode());

        int lastHeight = Random.Range(0, map.GetUpperBound(1));

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            int nextMove = rand.Next(2);

            if (nextMove == 0 && lastHeight > 2)
            {
                lastHeight--;
            }
            else if (nextMove == 1 && lastHeight < map.GetUpperBound(1) - 2)
            {
                lastHeight++;
            }

            for (int y = lastHeight; y >= 0; y--)
            {
                map[x, y] = 1;
            }
        }
        return map;
    }
    public static int[,] RandomWalkTopSmoothed(int[,] map, float seed, int minSectionWidth)
    {
        System.Random rand = new System.Random(seed.GetHashCode());

        int lastHeight = Random.Range(0, map.GetUpperBound(1));

        int nextMove = 0;
        int sectionWidth = 0;

        for (int x = 0; x <= map.GetUpperBound(0); x++)
        {
            nextMove = rand.Next(2);

            if (nextMove == 0 && lastHeight > 0 && sectionWidth > minSectionWidth)
            {
                lastHeight--;
                sectionWidth = 0;
            }
            else if (nextMove == 1 && lastHeight < map.GetUpperBound(1) && sectionWidth > minSectionWidth)
            {
                lastHeight++;
                sectionWidth = 0;
            }
            sectionWidth++;

            for (int y = lastHeight; y >= 0; y--)
            {
                map[x, y] = 1;
            }
        }

        return map;
    }

    public static int[,] RandomWalkCave(int[,] map, float seed, int requiredFloorPercent)
    {
        System.Random rand = new System.Random(seed.GetHashCode());

        int floorX = rand.Next(1, map.GetUpperBound(0) - 1);
        int floorY = rand.Next(1, map.GetUpperBound(1) - 1);
        int reqFloorAmount = ((map.GetUpperBound(1) * map.GetUpperBound(0)) * requiredFloorPercent) / 100;
        int floorCount = 0;

        map[floorX, floorY] = 0;
        floorCount++;

        while (floorCount < reqFloorAmount)
        {
            int randDir = rand.Next(4);

            switch (randDir)
            {
                case 0:
                    if ((floorY + 1) < map.GetUpperBound(1) - 1)
                    {
                        floorY++;

                        if (map[floorX, floorY] == 1)
                        {
                            map[floorX, floorY] = 0;
                            floorCount++;
                        }
                    }
                    break;
                case 1:
                    if ((floorY - 1) > 1)
                    {
                        floorY--;
                        if (map[floorX, floorY] == 1)
                        {
                            map[floorX, floorY] = 0;
                            floorCount++;
                        }
                    }
                    break;
                case 2:
                    if ((floorX + 1) < map.GetUpperBound(0) - 1)
                    {
                        floorX++;
                        if (map[floorX, floorY] == 1)
                        {
                            map[floorX, floorY] = 0;
                            floorCount++;
                        }
                    }
                    break;
                case 3:
                    if ((floorX - 1) > 1)
                    {
                        floorX--;
                        if (map[floorX, floorY] == 1)
                        {
                            map[floorX, floorY] = 0;
                            floorCount++;
                        }
                    }
                    break;
            }
        }
        return map;
    }

    public static int[,] RandomWalkCaveCustom(int[,] map, float seed, int requiredFloorPercent)
    {
        System.Random rand = new System.Random(seed.GetHashCode());

        int floorX = Random.Range(1, map.GetUpperBound(0) - 1);
        int floorY = Random.Range(1, map.GetUpperBound(1) - 1);
        int reqFloorAmount = ((map.GetUpperBound(1) * map.GetUpperBound(0)) * requiredFloorPercent) / 100;
        int floorCount = 0;

        map[floorX, floorY] = 0;
        floorCount++;

        while (floorCount < reqFloorAmount)
        {
            int randDir = rand.Next(8);

            switch (randDir)
            {
                case 0:
                    if ((floorY + 1) < map.GetUpperBound(1) && (floorX - 1) > 0)
                    {
                        floorY++;
                        floorX--;

                        if (map[floorX, floorY] == 1)
                        {
                            map[floorX, floorY] = 0;
                            floorCount++;
                        }
                    }
                    break;
                case 1:
                    if ((floorY + 1) < map.GetUpperBound(1))
                    {
                        floorY++;

                        if (map[floorX, floorY] == 1)
                        {
                            map[floorX, floorY] = 0;
                            floorCount++;
                        }
                    }
                    break;
                case 2:
                    if ((floorY + 1) < map.GetUpperBound(1) && (floorX + 1) < map.GetUpperBound(0))
                    {
                        floorY++;
                        floorX++;

                        if (map[floorX, floorY] == 1)
                        {
                            map[floorX, floorY] = 0;
                            floorCount++;
                        }
                    }
                    break;
                case 3:
                    if ((floorX + 1) < map.GetUpperBound(0))
                    {
                        floorX++;

                        if (map[floorX, floorY] == 1)
                        {
                            map[floorX, floorY] = 0;
                            floorCount++;
                        }
                    }
                    break;
                case 4:
                    if ((floorY - 1) > 0 && (floorX + 1) < map.GetUpperBound(0))
                    {
                        floorY--;
                        floorX++;

                        if (map[floorX, floorY] == 1)
                        {
                            map[floorX, floorY] = 0;
                            floorCount++;
                        }
                    }
                    break;
                case 5:
                    if ((floorY - 1) > 0)
                    {
                        floorY--;

                        if (map[floorX, floorY] == 1)
                        {
                            map[floorX, floorY] = 0;
                            floorCount++;
                        }
                    }
                    break;
                case 6: 
                    if ((floorY - 1) > 0 && (floorX - 1) > 0)
                    {
                        floorY--;
                        floorX--;

                        if (map[floorX, floorY] == 1)
                        {
                            map[floorX, floorY] = 0;
                            floorCount++;
                        }
                    }
                    break;
                case 7: 
                    if ((floorX - 1) > 0)
                    {
                        floorX--;

                        if (map[floorX, floorY] == 1)
                        {
                            map[floorX, floorY] = 0;
                            floorCount++;
                        }
                    }
                    break;
            }
        }

        return map;
    }
    public static int[,] DirectionalTunnel(int[,] map, int minPathWidth, int maxPathWidth, int maxPathChange, int roughness, int windyness)
    {
        int tunnelWidth = 1;

        int x = map.GetUpperBound(0) / 2;

        System.Random rand = new System.Random(Time.time.GetHashCode());

        for (int i = -tunnelWidth; i <= tunnelWidth; i++)
        {
            map[x + i, 0] = 0;
        }

        for (int y = 1; y < map.GetUpperBound(1); y++)
        {
            if (rand.Next(0, 100) > roughness)
            {

                int widthChange = Random.Range(-maxPathWidth, maxPathWidth);
                tunnelWidth += widthChange;

                if (tunnelWidth < minPathWidth)
                {
                    tunnelWidth = minPathWidth;
                }

                if (tunnelWidth > maxPathWidth)
                {
                    tunnelWidth = maxPathWidth;
                }
            }

            if (rand.Next(0, 100) > windyness)
            {
                int xChange = Random.Range(-maxPathChange, maxPathChange);
                x += xChange;

                if (x < maxPathWidth)
                {
                    x = maxPathWidth;
                }
                if (x > (map.GetUpperBound(0) - maxPathWidth))
                {
                    x = map.GetUpperBound(0) - maxPathWidth;
                }

            }

            for (int i = -tunnelWidth; i <= tunnelWidth; i++)
            {
                map[x + i, y] = 0;
            }
        }
        return map;
    }
    public static int[,] GenerateCellularAutomata(int width, int height, float seed, int fillPercent, bool edgesAreWalls)
    {
        System.Random rand = new System.Random(seed.GetHashCode());

        int[,] map = new int[width, height];

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (edgesAreWalls && (x == 0 || x == map.GetUpperBound(0) - 1 || y == 0 || y == map.GetUpperBound(1) - 1))
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (rand.Next(0, 100) < fillPercent) ? 1 : 0;
                }
            }
        }
        return map;
    }

    public static int[,] SmoothVNCellularAutomata(int[,] map, bool edgesAreWalls, int smoothCount)
    {
        for (int i = 0; i < smoothCount; i++)
        {
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {
                    int surroundingTiles = GetVNSurroundingTiles(map, x, y, edgesAreWalls);

                    if (edgesAreWalls && (x == 0 || x == map.GetUpperBound(0) - 1 || y == 0 || y == map.GetUpperBound(1)))
                    {
                        map[x, y] = 1;
                    }
                    else if (surroundingTiles > 2)
                    {
                        map[x, y] = 1;
                    }
                    else if (surroundingTiles < 2)
                    {
                        map[x, y] = 0;
                    }
                }
            }
        }
        return map;
    }

	static int GetVNSurroundingTiles(int[,] map, int x, int y, bool edgesAreWalls)
    {
        int tileCount = 0;

        if (x - 1 > 0)
        {
            tileCount += map[x - 1, y];
        }
        else if (edgesAreWalls)
        {
            tileCount++;
        }

        if (y - 1 > 0)
        {
            tileCount += map[x, y - 1];
        }
        else if (edgesAreWalls)
        {
            tileCount++;
        }

        if (x + 1 < map.GetUpperBound(0))
        {
            tileCount += map[x + 1, y];
        }
        else if (edgesAreWalls)
        {
            tileCount++;
        }

        if (y + 1 < map.GetUpperBound(1))
        {
            tileCount += map[x, y + 1];
        }
        else if (edgesAreWalls)
        {
            tileCount++;
        }

        return tileCount;
    }
    public static int[,] SmoothMooreCellularAutomata(int[,] map, bool edgesAreWalls, int smoothCount)
    {
        for (int i = 0; i < smoothCount; i++)
        {
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {
                    int surroundingTiles = GetMooreSurroundingTiles(map, x, y, edgesAreWalls);

                    if (edgesAreWalls && (x == 0 || x == (map.GetUpperBound(0) - 1) || y == 0 || y == (map.GetUpperBound(1) - 1)))
                    {
                        map[x, y] = 1;
                    }
                    else if (surroundingTiles > 4)
                    {
                        map[x, y] = 1;
                    }
                    else if (surroundingTiles < 4)
                    {
                        map[x, y] = 0;
                    }

                }
            }
        }
        return map;
    }

    static int GetMooreSurroundingTiles(int[,] map, int x, int y, bool edgesAreWalls)
    {

        int tileCount = 0;

        for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
        {
            for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < map.GetUpperBound(0) && neighbourY >= 0 && neighbourY < map.GetUpperBound(1))
                {
                    if (neighbourX != x || neighbourY != y)
                    {
                        tileCount += map[neighbourX, neighbourY];
                    }
                }
            }
        }
        return tileCount;
    }

}