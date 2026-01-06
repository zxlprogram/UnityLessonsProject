using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonTo : MonoBehaviour
{
    // 在 Inspector 面板填入目標場景名稱（例如：TutorialScene）
    public string targetScene;
    public void StartMenu()
    {
        Debug.Log("go to scene: " + targetScene);
        SceneManager.LoadScene(targetScene);
    }
}