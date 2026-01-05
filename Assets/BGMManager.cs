using UnityEngine;

public class GameBGM : MonoBehaviour
{
    private static GameBGM instance;

    void Awake()
    {
        // 檢查是否已經有另一個音樂播放器存在
        if (instance == null)
        {
            instance = this;
            // 關鍵：告訴 Unity 切換場景時不要刪除這個物件
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 如果場景中已經有一個了，就刪掉重複的，避免疊音
            Destroy(gameObject);
        }
    }
}