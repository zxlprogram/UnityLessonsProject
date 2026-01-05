using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMM : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
