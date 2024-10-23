using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleShooter : MonoBehaviour
{
    // The amounts of particles this object spawns
    public float particleAmount;

    // The general direction of the particles
    public Vector3 particleDirection;

    // The offset of the direction of the particles
    public float particleDirectionOffset;

    // A reference to the particle prefab
    public GameObject particlePrefab;

    // The minimum force a particle should have
    public float forceMin;

    // The maximum force a particle should have
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
        // Create a copy of the particle prefab
        GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);

        // Get a reference to the particle's RigidBody component
        Rigidbody rb = particle.GetComponent<Rigidbody>();

        // Give the particle a random direction and force
        rb.velocity = CreateDirection();

        // Get a reference to the particle's material
        Renderer renderer = particle.GetComponent<Renderer>();

        // set the particle's material color to a random color
        renderer.material.color = Random.ColorHSV(0.5f, 1f);
    }
    private Vector3 CreateDirection()
    {
        // Create a random X and Y offset for the direction
        float xOffset = Random.Range(-particleDirectionOffset, particleDirectionOffset);
        float yOffset = Random.Range(-particleDirectionOffset, particleDirectionOffset);

        // Create a random force
        float force = Random.Range(forceMin, forceMax);
        
        // Create a vector3 using the random offsets for the direction
        Vector3 direction = new Vector3(particleDirection.x + xOffset, particleDirection.y + yOffset, particleDirection.z);

        // normalize the vector3, then multiply it by the random generated force
        direction = direction.normalized * force;

        // Return the direction to use it as velocity
        return direction;
    }
}
