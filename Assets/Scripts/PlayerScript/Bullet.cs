using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    public Tilemap tilemap;

    private void Start()
    {
        tilemap = GameObject.FindGameObjectWithTag("BreakableTile").GetComponent<Tilemap>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger");
        if (collision.tag == "BreakableTile")
        {
            Debug.Log("Terrain");
            Vector3Int pos = tilemap.WorldToCell(gameObject.transform.position);

            tilemap.SetTile(pos, null);

            Destroy(gameObject);
        }
        else if (collision.tag == "EnemyTrigger")
        {
            collision.GetComponent<Enemy>().takeDamage(Random.Range(20, 50));
        }
    }
}