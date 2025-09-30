using System;
using UnityEngine;

public class HealthCollectible : Interactable
{
    public float healAmount = 25f;
    public Vector3 rotationSpeed = new Vector3(0f, 50f, 0f);
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.RestoreHealth(healAmount);
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
        //Mathf.PingPong(rotationSpeed.magnitude * Time.deltaTime, 1f);
    }
}
