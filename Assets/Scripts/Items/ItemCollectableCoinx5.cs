using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectableCoinx5 : ItemCollectableBase
{
    protected override void OnCollect()
    {
        base.OnCollect();
        ItemManager.Instance.AddCoins(5);
    }
}
