using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Splitter splitter;

    void Update()
    {
        //回合開始才能切
        if (GameManager.Instance.currentPhase != GamePhase.Player1Turn &&
            GameManager.Instance.currentPhase != GamePhase.Player2Turn)
            return;

        //滑鼠操控切割
        if (Input.GetMouseButtonDown(0))
        {
            splitter.DoSlice();
        }
    }
}

