using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TestFactorManager))]
public class InspectorButtonTestFactorManager : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TestFactorManager testFactorManager = (TestFactorManager)target;
        if (GUILayout.Button("Apply"))
        {
            testFactorManager.Apply();
        }
    }
}
