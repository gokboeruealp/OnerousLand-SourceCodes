using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyCount : MonoBehaviour
{
    public Text remainingEnemies;

    private void Update()
    {
        remainingEnemies.text = gameObject.transform.childCount.ToString();

        if (gameObject.transform.childCount <= 0)
        {
            SceneManager.LoadScene("EndGame");
        }
    }
}
