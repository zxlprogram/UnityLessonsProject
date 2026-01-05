using UnityEngine;
using TMPro; // 如果你使用的是 TextMeshPro，若是一般 Text 請改用 UnityEngine.UI

public class LevelClearDisplay : MonoBehaviour
{
    [Header("顯示比例的文字組件")]
    public TextMeshProUGUI upperText; // 拖入顯示上塊比例的文字
    public TextMeshProUGUI lowerText; // 拖入顯示下塊比例的文字

    void Start()
    {
        // 當場景載入時，從靜態類別 GameData 讀取存好的比例
        if (upperText != null)
        {
            upperText.text = $"上塊比例: {GameData.UpperPercent:F1}%";
        }

        if (lowerText != null)
        {
            lowerText.text = $"下塊比例: {GameData.LowerPercent:F1}%";
        }
    }
}