using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject Enemy1;

    public float enemy1PerSpawn;

    public Tilemap tileMap;
    public List<Vector3> availablePlaces = null;

    public GameObject enemies;
    public void StartFonc()
    {
        availablePlaces.Clear();
        for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++)
        {
            for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tileMap.transform.position.y));
                Vector3 place = tileMap.CellToWorld(localPlace);
                if (tileMap.HasTile(localPlace))
                {
                    availablePlaces.Add(place);
                }
            }
        }

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            DestroyImmediate(enemy);
        }

        if (Enemy1 != null)
        {
            enemySpawnerFonc();
        }
    }
    private void enemySpawnerFonc()
    {
        for (int i = 0; i < enemy1PerSpawn; i++)
        {
            GameObject enemyGO = Instantiate(Enemy1, availablePlaces[Random.Range(0, availablePlaces.Count)], Quaternion.identity, enemies.transform);

            Vector3Int v3Int = tileMap.WorldToCell(enemyGO.transform.position);
            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    tileMap.SetTile(new Vector3Int(v3Int.x + x, v3Int.y + y, 0), null);
                }
            }
            
        }
    }
}
