using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] public Camera _camera;
    private float xRotation;
    public float xSensitivity = 30f; 
    public float ySensitivity = 30f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        //Camera rotation for looking Up and Down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        _camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //Applying to camera transform
        //Camera rotation for looking Left and Right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity); 
    }
}
