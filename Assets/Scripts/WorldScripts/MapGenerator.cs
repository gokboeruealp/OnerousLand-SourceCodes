using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapGenerator : MonoBehaviour
{
	public EnemySpawner enemySpawner;

	public Tilemap tilemap;
	public TileBase tile;

	[Tooltip("The width of each layer of the stack")]
	public int width;
	[Tooltip("The height of each layer of the stack")]
	public int height;

	[SerializeField]
	public List<MapSettings> mapSettings = new List<MapSettings>();

	List<int[,]> mapList = new List<int[,]>();


	[ExecuteInEditMode]
	public void GenerateMap()
	{
		ClearMap();
		mapList = new List<int[,]>();
		for (int i = 0; i < mapSettings.Count; i++)
		{
			int[,] map = new int[width, height];
			float seed;
			if (mapSettings[i].randomSeed)
				seed = Time.time.GetHashCode();
			else
				seed = mapSettings[i].seed.GetHashCode();

			switch (mapSettings[i].algorithm)
			{
				case Algorithm.RandomWalkTopSmoothed:
					map = MapFunctions.GenerateArray(width, height, true);
					map = MapFunctions.RandomWalkTopSmoothed(map, seed, mapSettings[i].interval);
					break;
				case Algorithm.CellularAutomataVonNeuman:
					map = MapFunctions.GenerateCellularAutomata(width, height, seed, mapSettings[i].fillAmount, mapSettings[i].edgesAreWalls);
					map = MapFunctions.SmoothVNCellularAutomata(map, mapSettings[i].edgesAreWalls, mapSettings[i].smoothAmount);
					break;
				case Algorithm.CellularAutomataMoore:
					map = MapFunctions.GenerateCellularAutomata(width, height, seed, mapSettings[i].fillAmount, mapSettings[i].edgesAreWalls);
					map = MapFunctions.SmoothMooreCellularAutomata(map, mapSettings[i].edgesAreWalls, mapSettings[i].smoothAmount);
					break;
			}
			mapList.Add(map);
		}
		Vector2Int offset = new Vector2Int(-width / 2, (-height / 2) - 1);
		foreach (int[,] map in mapList)
		{
			MapFunctions.RenderMapWithOffset(map, tilemap, tile, offset);
			offset.y += -height + 1;
		}

		enemySpawner.StartFonc();
	}

	public void ClearMap()
	{
		tilemap.ClearAllTiles();
	}
}