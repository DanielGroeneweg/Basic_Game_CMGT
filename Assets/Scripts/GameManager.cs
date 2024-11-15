using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region variables
    // An enum to control if the game is paused or not
    public enum gameStates { Playing, Paused };
    public gameStates gameState = gameStates.Playing;

    // A reference to the score scriptable object
    public IntCount score;

    // A reference to the playerhealth scriptable object
    public IntCount playerHealth;

    // A reference to the highscore scriptable object
    public IntCount highscore;

    // A reference to the scene loader script
    public SceneLoader _SceneLoader;

    // A reference to the canvas manager script to control the HUD
    public CanvasManager _CanvasManager;

    // A reference to the pick-up spawner script
    public PickUpSpawner _PickUpSpawner;

    // A reference to the player script that manages aiming and shooting
    public PlayerAimingAndShooting _PlayerAimingAndShooting;

    // A reference to the player script that manages movement
    public PlayerMovement _PlayerMovement;

    // A reference to the enemySpawner script
    public EnemySpawner _EnemySpawner;

    // A reference to the set cursor mode script
    public SetCursorMode _SetCursorMode;

    // A reference to the top part of the player tank
    public GameObject _PlayerTankTop;

    // A reference to the health pick-up dropped by killed enemies
    public GameObject healthPickUpPrefab;

    // A reference to the PlayerItems gameobject
    public GameObject _PlayerItems;

    // A reference to the pauseMenu shown when the game is paused
    public GameObject pauseMenu;

    // The room transforms used for the enemy movement AI
    public Transform _CenterRoom;
    public Transform _TopLeftRoom;
    public Transform _TopRightRoom;
    public Transform _BottomLeftRoom;
    public Transform _BottomRightRoom;

    public static GameManager instance;
    #endregion
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        Cursor.lockState = CursorLockMode.Locked;
    }
    public void IncreaseScore()
    {
        score.ChangeValue(1);
        _CanvasManager.ChangeScoreText(score.value);

        // Save the highscore if the player's score is higher than the highscore (we do it here to also save the score if the game crashes or is closed before the player loses)
        if (score.value > highscore.value) highscore.SetValue(score.value);
    }

    public void DamagePlayer()
    {
        // Damage player and display its health
        _CanvasManager.ChangeHealthDisplay(playerHealth.value);
        playerHealth.ChangeValue(-1);

        // If the player has 0 health, the game ends
        if (playerHealth.value <= 0) GameOver();
    }
    private void GameOver()
    {
        // Enable the cursor and load the end scene
        _SetCursorMode.SetCursorLockState("None");
        _SceneLoader.LoadScene("EndScene");
    }
    public void HealPlayer()
    {
        // Cap our max health at 3
        if (playerHealth.value < 3)
        {
            // increase health by 1, then add a heart from the HUD
            playerHealth.ChangeValue(1);
            _CanvasManager.ChangeHealthDisplay(playerHealth.value);
        }
        
    }

    public void DamageEnemy(GameObject enemy)
    {
        EnemyHandler _EnemyHandler = enemy.GetComponent<EnemyHandler>();
        _EnemyHandler.DamageEnemy();

        // increase the player's score and destroy this enemy if it has 0 or less HP
        if (_EnemyHandler.health <= 0)
        {
            // Get the position of the enemy and then float it above the ground
            var pickUpLocation = enemy.transform.position;
            pickUpLocation.y += 1;

            // Create a health pick up
            Instantiate(healthPickUpPrefab, pickUpLocation, Quaternion.identity);

            // Decrease the amount of enemiesInScene int
            _EnemySpawner.enemiesInScene--;

            // Increase score and destroy this enemy
            IncreaseScore();
            Destroy(enemy);
        }
    }

    public void SuperBulletPickedUp()
    {
        _CanvasManager.SwitchBulletImageVisibility();
        _PlayerAimingAndShooting.hasSuperBullet = true;
        _PickUpSpawner.hasSpawned = false;
    }

    private void Update()
    {
        // Pause the game if the player presses escape
        if (Input.GetKeyDown(KeyCode.Escape)) ShowPauseMenu();
    }

    private void ShowPauseMenu()
    {
        gameState = gameStates.Paused;
        pauseMenu.SetActive(true);
        _PlayerItems.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _SetCursorMode.SetCursorLockState("None");
    }

    public void ContinueGame()
    {
        gameState = gameStates.Playing;
        pauseMenu.SetActive(false);
        _SetCursorMode.SetCursorLockState("Locked");
    }
}
