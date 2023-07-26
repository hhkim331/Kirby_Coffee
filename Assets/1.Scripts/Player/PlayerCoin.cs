using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCoin : MonoBehaviour
{
    //코인
    int coin;
    public int Coin
    {
        get { return coin; }
        set
        {
            coin = value;
            coinText.text = string.Format("{0:000}", coin);
        }
    }

    [SerializeField] Image coinImage;
    [SerializeField] TextMeshProUGUI coinText;

    //나중에 이펙트 추가
}
