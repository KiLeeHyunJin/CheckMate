using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultReward : MonoBehaviour
{
    [SerializeField] InGameManager inGame;
    [SerializeField] GameManager gameManager;

    [SerializeField] TextMeshProUGUI getCoin;

    private void Awake()
    {
        if (inGame == null)
            inGame = InGameManager.instance;
        if (gameManager == null)
            gameManager = GameManager.instance;

    }
}
