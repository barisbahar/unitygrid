using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CreateMap))]
public class ScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CreateMap script = (CreateMap)target;
        script.levelcount = EditorGUILayout.IntSlider("Level Count", script.levelcount, 0, ((script.width * script.height) - 5));
        script.obstacle = EditorGUILayout.Toggle("Obstacle", script.obstacle);
        if (script.obstacle)
        {
            script.obstaclerotate = EditorGUILayout.Toggle("Obstacle Rotate", script.obstaclerotate);
            script.obstaclemove = EditorGUILayout.Toggle("Obstacle Move", script.obstaclemove);
        }
        script.x = EditorGUILayout.IntSlider("Obstacle x", script.x, 0, script.height);
        script.z = EditorGUILayout.IntSlider("Obstacle z", script.z, 0, script.width);

    }
}