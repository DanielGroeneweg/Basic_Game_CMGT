using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleShooter : MonoBehaviour
{
    public float particleAmount;

    public Vector3 particleDirection;

    public float particleDirectionOffset;

    public GameObject particlePrefab;

    public float forceMin;

    public float forceMax;
    private void Start()
    {
        ShootParticles();
    }

    private void ShootParticles()
    {
        for (int i = 1; i <= particleAmount; i++) CreateParticle();
    }

    private void CreateParticle()
    {
        GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);

        Rigidbody rb = particle.GetComponent<Rigidbody>();
        rb.velocity = CreateDirection();

        Renderer renderer = particle.GetComponent<Renderer>();
        float value = Random.Range(0.1f, 1);
        renderer.material.color = Color.HSVToRGB(value, 1, 1);
    }
    private Vector3 CreateDirection()
    {
        float xOffset = Random.Range(-particleDirectionOffset, particleDirectionOffset);
        float yOffset = Random.Range(-particleDirectionOffset, particleDirectionOffset);

        float force = Random.Range(forceMin, forceMax);

        // Create a vector3 using the random offsets for the direction
        Vector3 direction = new Vector3(particleDirection.x + xOffset, particleDirection.y + yOffset, particleDirection.z).normalized;

        Vector3 vec = direction * force;
        return vec;
    }
}