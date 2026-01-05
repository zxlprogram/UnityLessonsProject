using UnityEngine;
using TMPro;

public class SliceUIController : MonoBehaviour
{
    public TMP_Text leftText;
    public TMP_Text rightText;
    public TMP_Text resultText;

    public void UpdateUI(float leftWeight, float rightWeight)
    {
        leftText.text = $"左邊重量：{leftWeight:F2}";
        rightText.text = $"右邊重量：{rightWeight:F2}";

        if (Mathf.Abs(leftWeight - rightWeight) < 0.01f)
            resultText.text = "結果：平衡";
        else if (leftWeight > rightWeight)
            resultText.text = "結果：左邊較重";
        else
            resultText.text = "結果：右邊較重";
    }
}
