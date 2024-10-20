using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EnemyAimingAndShooting : MonoBehaviour
{
    #region aiming variables
    // Publics
    public GameObject tankTop;
    public float rotationSpeed;

    // Privates
    private GameObject player;
    private GameObject playerTank;
    #endregion

    #region shooting variables
    // Publics
    public float shootInterval;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPoint;

    // Privates
    private float shootCooldown;
    private bool canShoot = true;
    #endregion

    #region particle variables
    public GameObject particle;
    public AudioSource shootingSound;
    private List<GameObject> particleList;
    #endregion

    public void Start()
    {
        player = GameObject.Find("PlayerItems");
        playerTank = GameObject.Find("PlayerTank");
    }
    public void Update()
    {
        Aim();

        if (canShoot) Shoot();
        else ShootCooldown();
    }

    private void Aim()
    {
        RaycastHit hit;
        var rayDirection = playerTank.transform.position - tankTop.transform.position;
        if (Physics.Raycast(tankTop.transform.position, rayDirection, out hit) && hit.transform == player.transform)
        {
            // Find the direction to the player in world space, but keep only the X and Z axes (ignore Y-axis)
            Vector3 directionToPlayer = new Vector3(
                playerTank.transform.position.x - tankTop.transform.position.x,
                0,
                playerTank.transform.position.z - tankTop.transform.position.z
            );

            // Create a rotation towards the player, keeping the Y-axis rotation only
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Smoothly rotate towards the target rotation, limiting to Y-axis
            tankTop.transform.rotation = Quaternion.Lerp(
                tankTop.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void ShootCooldown()
    {
        // Add time if our cooldown has not reached the set time before we can shoot
        if (shootCooldown < shootInterval) shootCooldown += Time.deltaTime;

        // reset our shootCooldown and allow us to shoot
        else
        {
            shootCooldown = 0;
            canShoot = true;
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.forward, out hit) && hit.transform == player.transform)
        {
            // Create a bullet at the bullet spawn point gameobject's transform
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            
            // Set our bullet speed
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
            
            // Set canShoot to false to make us wait for the cooldown to shoot again
            canShoot = false;

            // Spawn the smoke particle
            SpawnParticle();

            // Start the explosion sound
            shootingSound.Play();
        }
    }

    private void SpawnParticle()
    {
        GameObject myParticle = Instantiate(particle);
        myParticle.transform.position = new Vector3(bulletSpawnPoint.transform.position.x, myParticle.transform.position.y, bulletSpawnPoint.transform.position.z);
        myParticle.SetActive(true);
    }
}