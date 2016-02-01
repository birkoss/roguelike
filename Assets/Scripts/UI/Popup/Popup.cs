using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Popup : MonoBehaviour {

  public PopupTitle title;
  public PopupCharacterInfo characterInfo;
  public PopupItemInfo itemInfo;
  public PopupItems items;
  public PopupButtons buttons;

  public GameObject targetGameObject;

  public void Awake() {
    title.gameObject.SetActive( false );
    characterInfo.gameObject.SetActive( false );
    itemInfo.gameObject.SetActive( false );
    items.gameObject.SetActive( false );
    buttons.gameObject.SetActive( false );

    buttons.OnClosePanel += ClosePanel;
  }

  public void TryItem(string position, Item item) {
    characterInfo.TryItem(position, item);
  }

  public void SetTitle(string newTitle, Sprite icon = null) {
    title.SetTitle(newTitle, icon);
    title.gameObject.SetActive( true );
  }

  public void SetCharacter(Character character) {
    characterInfo.SetCharacter(character);
    characterInfo.gameObject.SetActive( true );
  }

  public void SetItem(Item item) {
    itemInfo.SetItem(item);
    itemInfo.gameObject.SetActive( true );
  }
  public Item GetItem() {
    return itemInfo.GetItem();
  }

  public PopupItems SetItems(List<Item> newItems) {
    items.SetItems(newItems);
    items.gameObject.SetActive( true );
    return items;
  }

  public void AddButton(string label, UnityAction action = null) {
    buttons.Add(label, action);
    buttons.gameObject.SetActive( true );
  }

  public void ClosePanel() {
    gameObject.GetComponent<Animator>().Play("SlideOut");
  }

}
