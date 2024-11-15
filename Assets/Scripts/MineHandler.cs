using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class MineHandler : MonoBehaviour
{
    public GameObject explosionPrefab;
    private GameManager _GameManager;
    private void Start()
    {
        _GameManager = GameManager.instance;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _GameManager.DamagePlayer();
            SpawnParticle();
            Destroy(gameObject);
        }
    }

    private void SpawnParticle()
    {
        GameObject myParticle = Instantiate(explosionPrefab);

        myParticle.transform.position = new Vector3(transform.position.x, myParticle.transform.position.y, transform.position.z);
    }
}
