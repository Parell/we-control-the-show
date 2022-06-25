using UnityEngine;

public class UIHudManager : MonoBehaviour
{
    [SerializeField] private GameObject hud;

    public void ShowHud()
    {
        hud.SetActive(true);
    }

    public void HideHud()
    {
        hud.SetActive(false);
    }
}
