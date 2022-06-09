using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEventSO), editorForChildClasses: true)]
public class EventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        GameEventSO e = target as GameEventSO;
        if (GUILayout.Button("Raise"))
            e.Raise();
    }
}
