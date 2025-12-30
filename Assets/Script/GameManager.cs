using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    void Awake()  
    {
        Instance = this;
    }
    void Start()  
    {
        turnText.gameObject.SetActive(false);
        StartCoroutine(StartGameFlow());
    }
    IEnumerator StartGameFlow()  //遊戲開始顯示Player1 Turn
    {
        currentPhase = GamePhase.Player1Turn;
        UpdateTurnUI();

        canSlice = false;

        yield return StartCoroutine(ShowTurnText("Player 1 Turn"));
        yield return new WaitForSeconds(1f);

        turnText.gameObject.SetActive(false);
        canSlice = true;
    }

    void UpdateTurnUI()   //控制player1.2的出現
    {
        if (currentPhase == GamePhase.Player1Turn)
        {
            turnText.text = "Player 1 Turn";
            turnText.gameObject.SetActive(true);

            player1.SetActive(true);
            player2.SetActive(false);
        }
        else if (currentPhase == GamePhase.Player2Turn)
        {
            
            player1.SetActive(false);
            player2.SetActive(true);
        }
        else
        {
            
            // Result 階段
            player1.SetActive(true);
            player2.SetActive(true);
        }
    }


    public void OnSliceFinished(float score)  //切割完
    {
        if(!canSlice) return;
        canSlice = false;

        if (currentPhase == GamePhase.Player1Turn)
        {
            player1Score = score;
            currentPhase = GamePhase.Player2Turn;
            StartCoroutine(ShowTurnText("Player 2 Turn"));
            UpdateTurnUI();
        }
        else if (currentPhase == GamePhase.Player2Turn)
        {
            player2Score = score;

            currentPhase = GamePhase.Result;
            UpdateTurnUI();

            ShowResult();
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

    IEnumerator SwitchToPlayer2Turn()  //切換到player2回合
    {
        yield return StartCoroutine(ShowTurnText("Player 2 Turn"));

        currentPhase = GamePhase.Player2Turn;
        UpdateTurnUI();

        canSlice = true;
    }

    void ShowResult()
    {
        if (player1Score < player2Score)
            Debug.Log("Player 1 Wins!");
        else if (player2Score < player1Score)
            Debug.Log("Player 2 Wins!");
        else
            Debug.Log("Draw!");
    }

}
