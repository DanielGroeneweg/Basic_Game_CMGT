using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EnemyAimingAndShooting : MonoBehaviour
{
    #region aiming variables
    // Publics
    // A reference to this enemy's top part of the tank
    public GameObject tankTop;

    // The rate at which the top part of the tank rotates
    public float rotationSpeed;

    // Privates
    // A reference to the GameManager script
    private GameManager _GameManager;
    #endregion

    #region shooting variables
    // Publics
    // The time in between each shot
    public float shootInterval;
    
    // The speed of the bullets fired
    public float bulletSpeed;

    // A reference to the bullet prefab
    public GameObject bulletPrefab;

    // A reference to the bulletSpawnPoint gameobject
    public GameObject bulletSpawnPoint;

    // Privates
    // A float to keep track of the shoot cooldown
    private float shootCooldown;

    // A bool to keep track if the enemy can shoot
    private bool canShoot = true;
    #endregion

    #region particle variables
    // A reference to the particle gameobject
    public GameObject particle;

    // A reference to the sound used for shooting
    public AudioSource shootingSound;
    #endregion

    public void Start()
    {
        _GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void Update()
    {
        if (_GameManager.gameState == GameManager.gameStates.Playing)
        {
            Aim();

            if (canShoot) Shoot();
            else ShootCooldown();
        }
    }

    private void Aim()
    {
        // Use a raycast to check if there is no object in between the enemy and player, then rotate the enemy's top part of the tank towards the player
        RaycastHit hit;
        Vector3 rayDirection = _GameManager._PlayerTankTop.transform.position - tankTop.transform.position;
        if (Physics.Raycast(tankTop.transform.position, rayDirection, out hit) && hit.transform == _GameManager._PlayerItems.transform)
        {
            // Find the direction to the player in world space, but keep only the X and Z axes (ignore Y-axis)
            Vector3 directionToPlayer = new Vector3(
                _GameManager._PlayerTankTop.transform.position.x - tankTop.transform.position.x,
                0,
                _GameManager._PlayerTankTop.transform.position.z - tankTop.transform.position.z
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
        // Use a raycast to check if the enemy tank's barrel is directly aimed at the player, then shoot
        RaycastHit hit;
        if (Physics.Raycast(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.forward, out hit) && hit.transform == _GameManager._PlayerItems.transform)
        {
            // Create a bullet at the bullet spawn point gameobject's transform
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            
            // Set our bullet speed
            bullet.GetComponent<BulletHandler>().speed = bulletSpeed;
            
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