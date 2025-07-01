using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 2, -4);
    public float mouseSensitivity = 100f;

    private float pitch = 0f;

    void LateUpdate()
    {
        if (!player) return;

        // Mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate player horizontally
        player.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -10f, 40f); // ✅ updated clamp here

        Quaternion cameraRotation = Quaternion.Euler(pitch, player.eulerAngles.y, 0);
        transform.position = player.position + cameraRotation * offset;

        transform.LookAt(player.position + Vector3.up * 1.6f);
    }
}
