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
    public GameObject tankTop;
    public Transform tankBodyTransform;
    public float rotationSpeed;

    // Privates
    private float targetRotation;
    #endregion

    #region shooting variables
    // Publics
    public float shootInterval;
    public float bulletSpeed;
    public bool hasSuperBullet = false;
    public GameObject bulletPrefab;
    public GameObject superBulletPrefab;
    public GameObject bulletSpawnPoint;
    public CanvasManager canvasManager;

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
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Update()
    {
        RotateTop();

        if (canShoot && Input.GetMouseButtonDown(0)) Shoot();
        else ShootCooldown();
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
            bullet = Instantiate(superBulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            
            hasSuperBullet = false;
            canvasManager.SwitchBulletImageVisibility();
        }

        else
        {
            bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        }

        // Set our bullet speed
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

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
}