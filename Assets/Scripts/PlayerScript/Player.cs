using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float HP = 250;
    float armor = 3;

    public Slider healtbar;
    public void takeDamage(float damage)
    {
        HP -= (damage - armor);
        updateHealtbar();
        if (HP <= 0)
        {
            Dead();
        }
    }
    private void updateHealtbar()
    {
        healtbar.value = HP;
    }

    public void Dead()
    {
        SceneManager.LoadScene("GameOver");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeadZone")
        {
            Dead();
        }
    }
}
