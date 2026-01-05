using UnityEngine;
using TMPro;

public class RankDisplay : MonoBehaviour
{
    [Header("標語顯示組件")]
    public TextMeshProUGUI rankText; // 拖入你的標語文字物件

    void Start()
    {
        if (rankText == null) return;

        // 從 GameData 讀取剛才存好的比例
        float upper = GameData.UpperPercent;
        float lower = GameData.LowerPercent;

        // 計算相差值 (絕對值)
        float diff = Mathf.Abs(upper - lower);

        // 根據相差值決定標語文字與顏色
        if (diff <= 3f)
        {
            rankText.text = "神之切割";
            rankText.color = Color.yellow;
        }
        else if (diff <= 12f)
        {
            rankText.text = "相當精準";
            rankText.color = Color.green;
        }
        else if (diff <= 25f)
        {
            rankText.text = "還算及格";
            rankText.color = Color.white;
        }
        else
        {
            rankText.text = "偏心嚴重";
            rankText.color = Color.red;
        }
    }
}