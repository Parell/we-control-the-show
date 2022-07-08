using UnityEngine;
using UnityEngine.UI;

public class VersionText : MonoBehaviour
{
    void Awake()
    {
        Text text = GetComponent<Text>();

        text.text = "v" + Application.version;
    }
}
