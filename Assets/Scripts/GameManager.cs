using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // A reference to the score scriptable object
    public IntCount score;

    // A reference to the playerhealth scriptable object
    public IntCount playerHealth;

    // A reference to the scene loader script
    public SceneLoader _SceneLoader;

    // A reference to the canvas manager script to control the HUD
    public CanvasManager _CanvasManager;

    // A reference to the pick-up spawner script
    public PickUpSpawner _PickUpSpawner;

    // A reference to the player script that manages aiming and shooting
    public PlayerAimingAndShooting _PlayerAimingAndShooting;

    // A reference to the PlayerItems gameobject
    public GameObject _PlayerItems;

    // A reference to the player script that manages movement
    public PlayerMovement _PlayerMovement;

    // A reference to the top part of the player tank
    public GameObject _PlayerTankTop;

    // The room transforms used for the enemy movement AI
    public Transform _CenterRoom;
    public Transform _TopLeftRoom;
    public Transform _TopRightRoom;
    public Transform _BottomLeftRoom;
    public Transform _BottomRightRoom;
    private void Awake()
    {
        // Make it so the player cursor is invisible and locked to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void IncreaseScore()
    {
        // Increase score
        score.ChangeValue(1);

        // Display the new Score
        _CanvasManager.ChangeScoreText(score.value);
    }

    public void DamagePlayer()
    {
        // Display our health
        _CanvasManager.ChangeHealthDisplay(playerHealth.value);

        // Decrease health by 1, then remove a heart from the HUD
        playerHealth.ChangeValue(-1);

        // Load end screen if player died
        if (playerHealth.value <= 0) _SceneLoader.LoadScene("EndScene");
    }

    public void HealPlayer()
    {
        // Cap our max health at 3
        if (playerHealth.value < 3)
        {
            // increase health by 1, then add a heart from the HUD
            playerHealth.ChangeValue(1);

            // Display our health
            _CanvasManager.ChangeHealthDisplay(playerHealth.value);
        }
        
    }

    public void DamageEnemy(GameObject enemy)
    {
        // Damage the Enemy
        enemy.GetComponent<EnemyHandler>().DamageEnemy();
    }

    public void SuperBulletPickedUp()
    {
        // Display that we have a super bullet
        _CanvasManager.SwitchBulletImageVisibility();

        // Enable the super bullet for the player shooting script
        _PlayerAimingAndShooting.hasSuperBullet = true;

        // Tell our pick-up spawner the pick-up has been picked up
        _PickUpSpawner.hasSpawned = false;
    }
}
