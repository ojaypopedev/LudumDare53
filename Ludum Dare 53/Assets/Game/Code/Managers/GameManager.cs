using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static System.Action onReset;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Reset();
    }

    public static void Reset()
    {

    }
}
