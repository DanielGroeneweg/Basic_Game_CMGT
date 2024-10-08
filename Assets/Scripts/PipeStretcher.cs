using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeStretcher : MonoBehaviour
{
    // The part of the pipe that will be stretched
    public GameObject pipeBase;
    // The part of the pipe at the end of it
    public GameObject pipeEnd;
    public void StretchPipe(float length, bool isOnTopOfScreen)
    {
        // Stretch the base of the pipe
        pipeBase.transform.localScale = new Vector3(pipeBase.transform.localScale.x, length, pipeBase.transform.localScale.z);
        // Move the end of the pipe to the end of the pipe base {
        if (isOnTopOfScreen)
        {
            pipeEnd.transform.localPosition = new Vector3(
                pipeEnd.transform.localPosition.x,
                pipeEnd.transform.localPosition.y - length * 2f,
                pipeEnd.transform.localPosition.z);
        }
        else
        {
            pipeEnd.transform.localPosition = new Vector3(
                pipeEnd.transform.localPosition.x,
                pipeEnd.transform.localPosition.y + length * 2f,
                pipeEnd.transform.localPosition.z);
        }
    }
}