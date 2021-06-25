using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float HP = 100;

    public Slider healtbar;
    public void takeDamage(float damage)
    {
        float Armor = Random.Range(0,11.5f);
        HP -= (damage - Armor);
        updateHealtbar();
        if (HP <= 0)
        {
            isDead();
        }
    }

    private void updateHealtbar()
    {
        healtbar.value = HP;
    }

    public void isDead()
    {
        Destroy(gameObject.transform.parent.gameObject);
        //particle
        //audios
    }
    public void Kamikaze(GameObject player)
    {
        Destroy(gameObject.transform.parent.gameObject);
        //particle
        //audios
        player.GetComponent<Player>().takeDamage(Random.Range(18, 25));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Kamikaze(collision.gameObject);
        }
    }
}
