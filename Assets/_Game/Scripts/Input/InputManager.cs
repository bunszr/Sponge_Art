using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputData inputData;

    private void Awake()
    {
        inputData.Init();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            inputData.pressedPosition = Utility.PlaneRaycast(Vector3.up, Vector3.zero);
            inputData.HasDrag.Value = true;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            inputData.HasDrag.Value = false;
        }

        if (inputData.HasDrag.Value)
        {
            inputData.position = Utility.PlaneRaycast(Vector3.up, Vector3.zero);
        }
    }
}