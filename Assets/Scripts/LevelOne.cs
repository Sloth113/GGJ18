using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Enemies{
    Tank,
    Speedy,
    Normal
}
[System.Serializable]
public struct Wave
{
    public List<Enemies> spawnOrder;
    public float spawnTimer;
}

public class LevelOne : MonoBehaviour {
    public List<Wave> waveInfo;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

//[UnityEditor.CustomEditor(typeof(LevelOne))]
//public class InspectorCustomizer : UnityEditor.Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        serializedObject.Update();
//        UnityEditor.EditorGUILayout.PropertyField(serializedObject.FindProperty("waveInfo"), true);
//    }

//}
