using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GamePhase //遊戲階段
{
    Start,
    Player1Turn,
    Player2Turn,
    Result
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GamePhase currentPhase;
    public GameObject player1;
    public GameObject player2;
    public TMP_Text turnText;
    public float player1Score;
    public float player2Score;
    public bool canSlice = false;
    public float switchDelaySeconds = 3f; // 玩家切割後等待天平落下的延遲時間
    public float resultDelaySeconds = 3f; // 玩家2分數顯示後等待再結算
    public TMP_Text resultText;
    public TMP_Text scoreText;
    public TMP_Text resultNameText; // 左側玩家名稱
    public GameObject resultPanel;
    public GameObject backBtn; 
    public RectTransform resultNameAnchor; // 左側名稱定位
    public Transform resultModelAnchor;    // 右側模型定位
    public calcWeight leftPan;
    public calcWeight rightPan;
    void Awake()  
    {
        Instance = this;
    }
    void Start()  
    {
        turnText.gameObject.SetActive(false);
        if (resultText != null)
            resultText.gameObject.SetActive(false);
        if (resultNameText != null)
            resultNameText.gameObject.SetActive(false);
        if (resultPanel != null)
            resultPanel.SetActive(false);
        if (scoreText != null)
            scoreText.gameObject.SetActive(true); // 分數常駐左上顯示
        UpdateScoreUI(); // 初始化分數顯示

        EnsurePans(); // 開場抓取天平資料來源

        bool startWithPlayer2 = PlayerPrefs.GetInt("StartWithPlayer2", 0) == 1;

        if (startWithPlayer2)
        {
            player1Score = PlayerPrefs.GetFloat("P1Score", 0f);
            PlayerPrefs.DeleteKey("StartWithPlayer2");
            PlayerPrefs.DeleteKey("P1Score");
            UpdateScoreUI();

            StartCoroutine(StartTurn(GamePhase.Player2Turn, "Player 2 Turn"));
        }
        else
        {
            StartCoroutine(StartTurn(GamePhase.Player1Turn, "Player 1 Turn"));
        }
    }
    IEnumerator StartTurn(GamePhase phase, string turnLabel)  //通用回合開始流程
    {
        currentPhase = phase;
        UpdateTurnUI();

        canSlice = false;

        yield return StartCoroutine(ShowTurnText(turnLabel));
        yield return new WaitForSeconds(1f);

        turnText.gameObject.SetActive(false);
        canSlice = true;
    }

    void EnsurePans()
    {
        if (leftPan != null && rightPan != null) return;

        foreach (var pan in FindObjectsOfType<calcWeight>())
        {
            if (pan.whichPlat == 0) leftPan = pan;
            if (pan.whichPlat == 1) rightPan = pan;
        }
    }

    float CalculateBalanceScore()
    {
        EnsurePans();
        if (leftPan == null || rightPan == null) return 0f;

        float left = leftPan.weight;
        float right = rightPan.weight;
        float total = left + right;
        if (total <= 0.0001f) return 0f; // 沒有重量就沒有得分

        float diff = Mathf.Abs(left - right);
        float balance01 = Mathf.Clamp01(1f - diff / (total + Mathf.Epsilon));
        return balance01 * 100f; // 滿分 100，越平衡越接近 100
    }

    void UpdateScoreUI()
    {
        if (scoreText == null) return;

        string p1 = float.IsPositiveInfinity(player1Score) || player1Score == float.MaxValue ? "--" : player1Score.ToString("F2");
        string p2 = float.IsPositiveInfinity(player2Score) || player2Score == float.MaxValue ? "--" : player2Score.ToString("F2");

        scoreText.text = $"P1: {p1}   |   P2: {p2}";
    }

    void UpdateTurnUI()   //控制player1.2的出現
    {
        if (currentPhase == GamePhase.Player1Turn)
        {
            turnText.text = "Player 1 Turn";
            turnText.gameObject.SetActive(true);

            player1.SetActive(true);
            player2.SetActive(false);
            SetPlayerControl(player1, true);
            SetPlayerControl(player2, false);
        }
        else if (currentPhase == GamePhase.Player2Turn)
        {
            player1.SetActive(false);
            player2.SetActive(true);
            SetPlayerControl(player1, false);
            SetPlayerControl(player2, true);
        }
        else
        {
            
            // Result 階段
            player1.SetActive(true);
            player2.SetActive(true);
            SetPlayerControl(player1, false);
            SetPlayerControl(player2, false);
        }
    }


    public void OnSliceFinished(float score)  //切割完
    {
        if(!canSlice) return;
        canSlice = false;
        DisableActivePlayerControl();

        if (currentPhase == GamePhase.Player1Turn)
        {
            StartCoroutine(HandleEndOfPlayer1());
        }
        else if (currentPhase == GamePhase.Player2Turn)
        {
            StartCoroutine(HandleEndOfPlayer2());
        }
    }
    IEnumerator ShowTurnText(string text)  //顯示turntext
    {
        turnText.text = text;
        turnText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        turnText.gameObject.SetActive(false);
        canSlice = true;
    }

    IEnumerator HandleEndOfPlayer1()
    {
        yield return new WaitForSeconds(switchDelaySeconds); // 等待切下的物件落到天平

        player1Score = CalculateBalanceScore();
        UpdateScoreUI();

        PlayerPrefs.SetFloat("P1Score", player1Score);
        PlayerPrefs.SetInt("StartWithPlayer2", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  //重置場景後換到玩家2
    }

    IEnumerator HandleEndOfPlayer2()
    {
        yield return new WaitForSeconds(switchDelaySeconds); // 等待切下的物件落到天平

        player2Score = CalculateBalanceScore();
        UpdateScoreUI();

        // 先顯示玩家2分數，等待 resultDelaySeconds 再進入結算
        yield return new WaitForSeconds(resultDelaySeconds);

        currentPhase = GamePhase.Result;
        UpdateTurnUI();

        ShowResult();
    }

    void SetPlayerControl(GameObject player, bool enable)
    {
        if (player == null) return;
        foreach (var pc in player.GetComponentsInChildren<PlayerController>(true))
        {
            pc.enabled = enable;
        }
    }

    void DisableActivePlayerControl()
    {
        if (currentPhase == GamePhase.Player1Turn)
            SetPlayerControl(player1, false);
        else if (currentPhase == GamePhase.Player2Turn)
            SetPlayerControl(player2, false);
    }

    void ShowResult()
    {
        string message;
        GameObject winner = null;
        GameObject loser = null;

        if (player1Score > player2Score)
        {
            message = "Player 1 Wins!";
            winner = player1;
            loser = player2;
        }
        else if (player2Score > player1Score)
        {
            message = "Player 2 Wins!";
            winner = player2;
            loser = player1;
        }
        else
        {
            message = "Draw!";
        }

        Debug.Log(message);

        if (resultText != null)
        {
            resultText.text = message;
            resultText.alignment = TextAlignmentOptions.Center;
            resultText.fontSize = 80;
            // 確保文字框居中顯示
            RectTransform rt = resultText.rectTransform;
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = GetMiddleScreenPositionBetweenPlayers();
            resultText.gameObject.SetActive(true);
        }
        else
        {
            turnText.text = message;
            turnText.gameObject.SetActive(true);
        }

        if (resultNameText != null)
        {
            string winnerName = winner == player1 ? "PLAYER 1" : winner == player2 ? "PLAYER 2" : "DRAW";
            resultNameText.text = winnerName;
            resultNameText.alignment = TextAlignmentOptions.Center;
            resultNameText.fontSize = 90;

            RectTransform rt = resultNameText.rectTransform;
            rt.pivot = new Vector2(0.5f, 0.5f);

            if (resultNameAnchor != null)
            {
                rt.anchorMin = resultNameAnchor.anchorMin;
                rt.anchorMax = resultNameAnchor.anchorMax;
                rt.anchoredPosition = resultNameAnchor.anchoredPosition;
                rt.sizeDelta = resultNameAnchor.sizeDelta;
            }
            else
            {
                rt.anchorMin = new Vector2(0.25f, 0.5f);
                rt.anchorMax = new Vector2(0.25f, 0.5f);
                rt.anchoredPosition = Vector2.zero;
            }

            resultNameText.gameObject.SetActive(true);
        }

        // 顯示結果遮罩
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
            var img = resultPanel.GetComponent<UnityEngine.UI.Image>();
            if (img != null)
            {
                img.color = Color.white; // 白底
            }
        }

        // 結算時隱藏分數顯示
        if (scoreText != null)
            scoreText.gameObject.SetActive(false);

        // 結算後禁止再操作
        SetPlayerControl(player1, false);
        SetPlayerControl(player2, false);
        canSlice = false;

        // 顯示返回按鈕
        if (backBtn != null)
            backBtn.SetActive(true);


        // 只顯示獲勝玩家模型，平手則顯示兩個
        if (winner != null && loser != null)
        {
            winner.SetActive(true);
            loser.SetActive(false);
            PlaceWinnerModel(winner);
        }
        else
        {
            player1.SetActive(true);
            player2.SetActive(true);
        }
    }

    Vector2 GetMiddleScreenPositionBetweenPlayers()
    {
        if (player1 == null || player2 == null) return Vector2.zero;

        Vector3 midWorld = (player1.transform.position + player2.transform.position) * 0.5f;

        Canvas canvas = resultText != null ? resultText.canvas : null;
        Camera cam = Camera.main;

        if (canvas != null)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(cam, midWorld);

            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out localPoint))
            {
                return localPoint;
            }
        }

        // fallback 居中
        return Vector2.zero;
    }

    void PlaceWinnerModel(GameObject winner)
    {
        if (winner == null || resultModelAnchor == null) return;

        // 重置並移動到展示點
        winner.SetActive(true);
        winner.transform.SetParent(resultModelAnchor, worldPositionStays: false);
        winner.transform.localPosition = Vector3.zero;
        winner.transform.localRotation = Quaternion.identity;
        winner.transform.localScale = Vector3.one;

        foreach (var rb in winner.GetComponentsInChildren<Rigidbody>())
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        foreach (var r in winner.GetComponentsInChildren<Renderer>(true))
        {
            r.enabled = true; // 確保模型被顯示
        }
    }

    // 返回模式選擇畫面
    public void BackToModeSelect()
    {
        GameObject.Find("BGM")?.GetComponent<AudioSource>()?.Stop();
        SceneManager.LoadScene("StartScene"); 
    }
}
