using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButtonHandler : MonoBehaviour
{
    // 在按鈕的 OnClick 事件中關聯這個方法
    public void ResetCurrentLevel()
    {
        // 取得當前場景的名稱並重新載入
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}