using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Transform target;

    public bool isFollow = false;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if (isFollow)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, 10 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isFollow = true;
        }
    }
}
