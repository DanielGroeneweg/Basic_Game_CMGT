using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class PlayerAimingAndShooting : MonoBehaviour
{
    #region rotation variables
    [Header("Rotation")]
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
    [Header("Shooting")]
    public float shootInterval;

    public float bulletSpeed;

    public bool hasSuperBullet = false;

    public BulletHandler bulletPrefab;

    public BulletHandler superBulletPrefab;

    public GameObject bulletSpawnPoint;

    public LineRenderer laserSight;

    private float shootCooldown;

    private bool canShoot = true;

    private GameManager _GameManager;
    #endregion

    #region particle variables
    [Header("Bullet")]
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
            RotateTop();

            AimingDot();

            if (canShoot)
            {
                if (Input.GetMouseButtonDown(0)) Shoot();
            }
            else ShootCooldown();
        }
    }

    private void RotateTop()
    {
        Vector3 var = tankTop.transform.localEulerAngles;

        targetRotation += Input.GetAxis("Mouse X") * rotationSpeed;

        tankTop.transform.localEulerAngles = new Vector3(
            var.x,
            0 - tankBodyTransform.localEulerAngles.y + targetRotation,
            var.z
            );
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
        BulletHandler bullet;
        
        if (hasSuperBullet)
        {
            bullet = Instantiate(superBulletPrefab, bulletSpawnPoint.transform.position + bulletSpawnPoint.transform.forward * 0.5f, bulletSpawnPoint.transform.rotation);

            hasSuperBullet = false;
            _GameManager._CanvasManager.SwitchBulletImageVisibility();
        }

        else
        {
            bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position + bulletSpawnPoint.transform.forward * 0.5f, bulletSpawnPoint.transform.rotation);
        }

        bullet.speed = bulletSpeed;

        canShoot = false;

        SpawnParticle();

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
        RaycastHit hit;
        if (Physics.Raycast(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.forward, out hit))
        {
            Vector3[] points = { bulletSpawnPoint.transform.position, hit.point };
            laserSight.SetPositions(points);
        }
    }
}