using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MassEvent/Item Shop Event")]
public class ItemShopEvent : TileEvent
{
    public List<Item> Items;
    [TextArea(3, 15)] public string Message;
    [TextArea(3, 15)] public string AskBuyMessage;
    [TextArea(3, 15)] public string BuyMessage;
    [TextArea(3, 15)] public string MoneyNotEnoughMessage;
    [TextArea(3, 15)] public string ItemCountOverMessage;
    [TextArea(3, 15)] public string CloseMessage;

    public override void Exec(RPGSceneManager manager)
    {
        manager.ItemShopWindow.Open(this);
    }
}