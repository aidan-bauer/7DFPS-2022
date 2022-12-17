using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public static GlobalConstants constants;

    private void Awake()
    {
        if (constants == null)
        {
            constants = Resources.Load<GlobalConstants>("Constants");
        }
    }
}
