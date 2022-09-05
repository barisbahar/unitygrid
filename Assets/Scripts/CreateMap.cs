using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName ="New Map", menuName ="Map")]
public class CreateMap : ScriptableObject
{
    [HideInInspector]
    public bool obstacle;
    [HideInInspector]
    public bool obstaclerotate;
    [HideInInspector]
    public bool obstaclemove;
    public int height;
    public int width;
    public int levelComplete;
    public Transform ground;
    public Transform player;
    public Transform obstacleprefab;
    [HideInInspector]
    public int x,z;
    [HideInInspector]
    public int levelcount;
}
