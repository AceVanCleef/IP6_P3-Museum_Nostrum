﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
