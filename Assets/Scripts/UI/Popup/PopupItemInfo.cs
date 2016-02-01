using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PopupItemInfo : MonoBehaviour {

  public Image image;

  public Text textName;
  public Text textDescription;

  public PopupStat stat;

  private Item item;

  public void SetItem(Item newItem) {
    item = newItem;

    image.sprite = GameEngine.instance.popupEngine.itemsSpriteSheet.Get( item.prefab );
    textName.text = item.name;
    textDescription.text = "Lorem ipsum...";

    stat.SetStats(item.GetStat("attack"), 0, item.GetStat("defense"), 0, item.GetStat("speed"), 0, item.GetStat("range"), 0);
  }

  public Item GetItem() {
    return item;
  }
}
