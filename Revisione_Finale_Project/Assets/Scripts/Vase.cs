using UnityEngine;

public class Vase : MonoBehaviour
{
    public new Rigidbody rigidbody;
    public float power;
    public GameObject noiseSource;

    private bool _activated;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!_activated && other.CompareTag("Player"))
        {
            var direction = other.transform.position - transform.position;
            direction.y = 0;
            rigidbody.AddForce(-direction * power, ForceMode.VelocityChange);
            _activated = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            noiseSource.SetActive(true);
        }
    }
}
