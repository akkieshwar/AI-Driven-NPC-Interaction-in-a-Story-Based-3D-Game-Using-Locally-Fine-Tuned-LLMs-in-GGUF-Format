using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Optional: Destroy on hit
        Destroy(gameObject);
    }
}
