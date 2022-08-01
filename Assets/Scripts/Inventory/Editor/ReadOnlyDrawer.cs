using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var previousGUIState = GUI.enabled; // Saves previous enabled value

        GUI.enabled = false; // Disables editing for property

        EditorGUI.PropertyField(position, property, label); // Draws property

        GUI.enabled = previousGUIState; // Sets previous enabled value
    }
}
