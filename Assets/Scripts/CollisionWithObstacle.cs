using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionWithObstacle : MonoBehaviour
{
    public AudioSource hitAudio;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            GameObject player = GameObject.Find("PlayerItems");
            // Make the player not able to move
            player.GetComponent<PlayerMovement>().isAlive = false;
            hitAudio.Play();
        }
    }
}
