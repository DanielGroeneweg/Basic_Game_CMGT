using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    private static DoNotDestroy dontDestroy;
    void Awake()
    {
        // Set this object to not destroy on load to keep it in all scenes
        DontDestroyOnLoad(this);

        if (dontDestroy == null)
        {
            dontDestroy = this;
        }

        // Destroy this object if the scene it was originally in gets loaded again to avoid duplicates
        else
        {
            Destroy(gameObject);
        }
    }
}
