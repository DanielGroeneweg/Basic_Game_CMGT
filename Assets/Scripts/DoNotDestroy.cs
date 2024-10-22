using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    private static DoNotDestroy dontDestroy;
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (dontDestroy == null)
        {
            dontDestroy = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
