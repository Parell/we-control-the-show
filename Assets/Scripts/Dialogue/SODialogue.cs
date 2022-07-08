using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject / Dialogue", fileName = "Dialogue")]
public class SODialogue : ScriptableObject
{
    public Dialogue[] setences;
}

[Serializable]
public class Dialogue
{
    [TextArea(3, 10)]
    public string setences;
    public string names;
}
