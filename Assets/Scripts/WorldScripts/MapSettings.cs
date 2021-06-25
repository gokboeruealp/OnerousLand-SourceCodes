using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif
public enum Algorithm
{
    RandomWalkTopSmoothed, CellularAutomataVonNeuman, CellularAutomataMoore
}

[System.Serializable]
[CreateAssetMenu(fileName = "NewMapSettings", menuName = "Map Settings", order = 0)]
public class MapSettings : ScriptableObject
{
    public Algorithm algorithm;
    public bool randomSeed;
    public float seed;
    public int fillAmount;
    public int smoothAmount;
    public int clearAmount;
    public int interval;
    public int minPathWidth, maxPathWidth, maxPathChange, roughness, windyness;
    public bool edgesAreWalls;
    public float modifier;
}

