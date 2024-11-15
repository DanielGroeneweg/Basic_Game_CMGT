using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class PlayerAimingAndShooting : MonoBehaviour
{
    #region rotation variables
    // Publics
    // A reference to the top part of the player tank
    public GameObject tankTop;

    // A reference to the transform attached to the body of the player tank
    public Transform tankBodyTransform;

    // The rate at which the top part of the tank rotates
    public float rotationSpeed;

    // Privates
    // A float to keep track of what the value of the player's tank top rotation should be
    private float targetRotation;
    #endregion

    #region shooting variables
    // Publics
    // The time in between each shot
    public float shootInterval;

    // The speed at which bullets fired by the player travel
    public float bulletSpeed;

    // a bool to keep track of if the player has a super bullet
    public bool hasSuperBullet = false;

    // a reference to the normal player bullet prefab
    public GameObject bulletPrefab;

    // A reference to the player super bullet prefab
    public GameObject superBulletPrefab;

    // A reference to the BulletSpawnPoint gameobject so we can spawn bullets at its location
    public GameObject bulletSpawnPoint;

    public LineRenderer laserSight;

    // Privates
    // A float to keep track of the cooldown timer
    private float shootCooldown;

    // A bool to check if the player can shoot
    private bool canShoot = true;

    private GameManager _GameManager;
    #endregion

    #region particle variables
    // A reference to the particle spawned when shooting a bullet
    public GameObject particle;

    // A reference to the sound player when shooting a bullet
    public AudioSource shootingSound;
    #endregion

    private void Start()
    {
        _GameManager = GameManager.instance;
    }
    public void Update()
    {
        if (!_GameManager.gamePaused)
        {
            // Rotate the top part of the player tank using the mouse input
            RotateTop();

            // Set the aiming dot to the object the player is aiming at
            AimingDot();

            // Check if the player pressed left mouse button if the player can shoot
            // Don't use && check, otherwise the cooldown timer starts running already, causing the shoot interval to vary
            if (canShoot)
            {
                if (Input.GetMouseButtonDown(0)) Shoot();
            }
            else ShootCooldown();
        }
    }

    private void RotateTop()
    {
        // Store our rotation in a variable
        Vector3 var = tankTop.transform.localEulerAngles;

        // Increase and Decrease our rotation by moving our mouse left and right
        targetRotation += Input.GetAxis("Mouse X") * rotationSpeed;

        // Set our Y rotation to always go to the world forward, then add the rotation from the mouse movement
        tankTop.transform.localEulerAngles = new Vector3(
            var.x,
            0 - tankBodyTransform.localEulerAngles.y + targetRotation,
            var.z
            );
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
        // Create a bullet at the bullet spawn point gameobject's transform
        GameObject bullet = null;
        
        // Change to Super Bullet if the player has one
        if (hasSuperBullet)
        {
            bullet = Instantiate(superBulletPrefab, bulletSpawnPoint.transform.position + bulletSpawnPoint.transform.forward * 0.5f, bulletSpawnPoint.transform.rotation);
            bullet.GetComponent<BulletHandler>().speed = bulletSpeed;

            hasSuperBullet = false;
            _GameManager._CanvasManager.SwitchBulletImageVisibility();
        }

        else
        {
            bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position + bulletSpawnPoint.transform.forward * 0.5f, bulletSpawnPoint.transform.rotation);
            bullet.GetComponent<BulletHandler>().speed = bulletSpeed;
        }

        // Set our bullet speed
        

        // Set canShoot to false to make us wait for the cooldown to shoot again
        canShoot = false;

        // Spawn a particle for the smoke
        SpawnParticle();

        // Start the shooting sound
        shootingSound.Play();
    }

    private void SpawnParticle()
    {
        GameObject myParticle = Instantiate(particle);
        myParticle.transform.position = new Vector3(bulletSpawnPoint.transform.position.x, myParticle.transform.position.y, bulletSpawnPoint.transform.position.z);
        myParticle.SetActive(true);
    }

    private void AimingDot()
    {
        // Set the line renderer points to render a line in between the bullet spawnpoint and the first hitbox hit using a raycast
        // going forwards from the buleltspawnpoint
        RaycastHit hit;
        if (Physics.Raycast(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.forward, out hit))
        {
            Vector3[] points = { bulletSpawnPoint.transform.position, hit.point };
            laserSight.SetPositions(points);
        }
    }
}