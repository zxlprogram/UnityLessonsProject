using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public int level;
   
    public void OpenScene()
    {
        SceneManager.LoadScene("Level " + level.ToString());
    }
}
