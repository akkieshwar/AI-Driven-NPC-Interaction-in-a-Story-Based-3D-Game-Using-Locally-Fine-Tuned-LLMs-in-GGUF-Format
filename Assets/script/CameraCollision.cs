using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform player; // Target (usually the player head)
    public float minDistance = 1f;
    public float maxDistance = 4f;
    public float smoothSpeed = 10f;
    public LayerMask collisionMask;

    private Vector3 dollyDir;
    private float currentDistance;

    void Start()
    {
        dollyDir = transform.localPosition.normalized;
        currentDistance = transform.localPosition.magnitude;
    }

    void LateUpdate()
    {
        Vector3 desiredCameraPos = player.position - player.forward * maxDistance;
        RaycastHit hit;

        if (Physics.SphereCast(player.position, 0.3f, -player.forward, out hit, maxDistance, collisionMask))
        {
            currentDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            currentDistance = maxDistance;
        }

        Vector3 finalPosition = player.position + dollyDir * -currentDistance;
        transform.position = Vector3.Lerp(transform.position, finalPosition, Time.deltaTime * smoothSpeed);
    }
}
