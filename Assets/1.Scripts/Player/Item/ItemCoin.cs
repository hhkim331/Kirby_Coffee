﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCoin : Item
{
    enum CoinType
    {
        Coin1,
        Coin5,
        Coin10,
    }
    [SerializeField] CoinType coinType;

    public override void GetItem()
    {
        base.GetItem();
        switch (coinType)
        {
            case CoinType.Coin1:
                PlayerManager.Instance.PCoin.Coin += 1;
                break;
            case CoinType.Coin5:
                PlayerManager.Instance.PCoin.Coin += 5;
                break;
            case CoinType.Coin10:
                PlayerManager.Instance.PCoin.Coin += 10;
                break;
        }
        Destroy(gameObject);
    }
}
