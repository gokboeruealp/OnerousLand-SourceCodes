using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    public MapGenerator mapGenerator;

    private void Start()
    {
        mapGenerator.GenerateMap();
    }
}
