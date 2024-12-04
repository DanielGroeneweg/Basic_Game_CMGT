using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EnemyAimingAndShooting : MonoBehaviour
{
    #region aiming variables
    public GameObject tankTop;

    public float rotationSpeed;

    private GameManager _GameManager;
    #endregion

    #region shooting variables
    public float shootInterval;
    
    public float bulletSpeed;

    public BulletHandler bulletPrefab;

    public GameObject bulletSpawnPoint;

    private float shootCooldown;

    private bool canShoot = false;
    #endregion

    #region particle variables
    public GameObject particle;

    public AudioSource shootingSound;
    #endregion

    public void Start()
    {
        _GameManager = GameManager.instance;
    }
    public void Update()
    {
        if (!_GameManager.gamePaused)
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
        if (shootCooldown < shootInterval) shootCooldown += Time.deltaTime;

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
            BulletHandler bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            
            bullet.speed = bulletSpeed;
            
            canShoot = false;

            SpawnParticle();

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