using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    // The prefab used for the pipe on the bottom of the screen
    public GameObject pipePrefab;
    // The prefab used for the pipe on the top of the screen
    public GameObject upsideDownPipePrefab;
    // The gap in between the bottom and top pipe the player flies through
    public float pipeGap;
    // The Y Position of the pipe on the bottom of the screen
    public float pipeYPos;
    // The Y Position of the pipe on the top of the screen
    public float upsideDownPipeYPos;
    // The minimum a length a pipe needs to have
    public float minimumPipeLength;
    // The distance in between each set of pipes
    public float pipeInterval;
    // The difference between the pipe and spawner positition that the pipe needs to be to be destroyed
    public float destroyXDifference;
    // The prefab used for the grass
    public GameObject grassPrefab;

    // A float to keep track of the distance the spawner has traveled
    private float traveledDistance;
    // A float to track the spawner's last X position
    private float previousXPosition;
    private void Start()
    {
        // Save the current X Position of the spawner
        previousXPosition = transform.position.x;
    }
    void Update()
    {
        CheckDistance();
        DestroyPipes();
    }
    void CheckDistance()
    {
        // Update the distance traveled, then check if the distance traveled is bigger than the distance in between each pipe
        traveledDistance += transform.position.x - previousXPosition;
        if (traveledDistance >= pipeInterval)
        {
            //Call the pipe spawning and then reset the distance we traveled
            SpawnPipe();
            traveledDistance -= pipeInterval;
        }
        // Track our current X Position
        previousXPosition = transform.position.x;
    }
    void DestroyPipes()
    {
        // Get a list of each pipe in the game, then go through that list
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
        for (int i = pipes.Length - 1; i >= 0; i--)
        {
            // destroy pipes when the difference between them and the spawner is bigger than or equal to the distance required to destroy it
            if (pipes[i].transform.position.x <= transform.position.x - destroyXDifference) Destroy(pipes[i]);
        }
    }
    void SpawnPipe()
    {
        //Randomize the length of the pipe on the bottom of the screen
        float pipeLength = Random.Range(minimumPipeLength, upsideDownPipeYPos - minimumPipeLength - pipeGap);

        //Calculate the length of the pipe on the top of the screen using the randomized length
        float upsideDownPipeLength = (pipeYPos + pipeLength + pipeGap) * -1f;

        //Create the pipe on the bottom of the screen
        GameObject pipe = Instantiate(pipePrefab, new Vector3(transform.position.x, pipeYPos, 0), Quaternion.identity);
        //Stretch that pipe
        pipe.GetComponent<PipeStretcher>().StretchPipe(pipeLength, false);

        //Create the pipe on the top of the screen
        GameObject upsideDownPipe = Instantiate(upsideDownPipePrefab, new Vector3(transform.position.x, upsideDownPipeYPos, 0), Quaternion.identity);
        //Stretch that pipe
        upsideDownPipe.GetComponent<PipeStretcher>().StretchPipe(upsideDownPipeLength, true);

        //Create the grass
        Instantiate(grassPrefab, new Vector3(transform.position.x + 3f, -5.1f, 0), Quaternion.identity);

        GameObject[] grasses = GameObject.FindGameObjectsWithTag("Grass");
        float smallestX = transform.position.x;
        GameObject furthestGrass = null;
        for (int i = grasses.Length - 1; i >= 0; i--)
        {
            if (grasses[i].transform.position.x < smallestX)
            {
                smallestX = grasses[i].transform.position.x;
                furthestGrass = grasses[i];
            }
        }
        if (furthestGrass != null) Destroy(furthestGrass);
    }
}
