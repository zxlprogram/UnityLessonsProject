using UnityEngine;

public class QuitHandler : MonoBehaviour
{
    public void QuitGame()
    {
        // 輸出測試訊息（在編輯器模式下不會真的關閉，所以要看 Console）
        Debug.Log("遊戲正在關閉...");

        // 如果是執行檔，則關閉程式
        Application.Quit();

        // 如果你在 Unity 編輯器內測試，這行可以讓它停止播放模式
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}