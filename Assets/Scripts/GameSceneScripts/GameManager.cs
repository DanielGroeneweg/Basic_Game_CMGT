using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region variables
    public bool gamePaused = false;

    public IntCount score;

    public IntCount playerHealth;

    public IntCount highscore;

    public SceneLoader _SceneLoader;

    public CanvasManager _CanvasManager;

    public PickUpSpawner _PickUpSpawner;

    public PlayerAimingAndShooting _PlayerAimingAndShooting;

    public PlayerMovement _PlayerMovement;

    public EnemySpawner _EnemySpawner;

    public SetCursorMode _SetCursorMode;

    public GameObject _PlayerTankTop;
    public GameObject healthPickUpPrefab;
    public GameObject _PlayerItems;
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
        _CanvasManager.ChangeHealthDisplay(playerHealth.value);
        playerHealth.ChangeValue(-1);

        if (playerHealth.value <= 0) GameOver();
    }
    private void GameOver()
    {
        _SetCursorMode.SetCursorLockState("None");
        _SceneLoader.LoadScene("EndScene");
    }
    public void HealPlayer()
    {
        if (playerHealth.value < 3)
        {
            playerHealth.ChangeValue(1);
            _CanvasManager.ChangeHealthDisplay(playerHealth.value);
        }
        
    }

    public void DamageEnemy(GameObject enemy)
    {
        EnemyHandler _EnemyHandler = enemy.GetComponent<EnemyHandler>();
        _EnemyHandler.DamageEnemy();

        if (_EnemyHandler.health <= 0)
        {
            var pickUpLocation = enemy.transform.position;
            pickUpLocation.y += 1;
            Instantiate(healthPickUpPrefab, pickUpLocation, Quaternion.identity);

            _EnemySpawner.enemiesInScene--;

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
        if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();
    }

    private void PauseGame()
    {
        gamePaused = true;
        pauseMenu.SetActive(true);
        _PlayerItems.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _SetCursorMode.SetCursorLockState("None");
    }

    public void ContinueGame()
    {
        gamePaused = false;
        pauseMenu.SetActive(false);
        _SetCursorMode.SetCursorLockState("Locked");
    }
}
