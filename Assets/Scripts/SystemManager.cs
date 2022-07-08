using UnityEngine;
using Cinemachine;


public class SystemManager : MonoBehaviour
{
    public static SystemManager Instance;

    public bool isCameraLocked;
    public bool isMovementLocked;
    public bool isPaused;

    void Start()
    {
        Instance = this;
        CinemachineCore.GetInputAxis = HandleCamera;
    }

    float HandleCamera(string axisName)
    {
        if (axisName == "Mouse X")
        {
            if (!isCameraLocked) return Input.GetAxis("Mouse X");
            else return 0;
        }
        else if (axisName == "Mouse Y")
        {
            if (!isCameraLocked) return Input.GetAxis("Mouse Y");
            else return 0;
        }
        return Input.GetAxis(axisName);
    }
}
