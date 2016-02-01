using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PopupItems : MonoBehaviour {

  public delegate void OnItemClickEvent(Item item);
	public event OnItemClickEvent OnItemClick;

  public Button btnBack;
  public Button btnNext;

  public Toggle[] toggleItems;

  private List<Item> items;
  private int currentPage;
  private Item currentItem;

  public void Awake() {
    btnBack.onClick.AddListener(delegate { ChangePage(-1); });
    btnNext.onClick.AddListener(delegate { ChangePage(1); });
  }

  public void ShowItem(Item item) {
    Debug.Log(item);
    Debug.Log( OnItemClick );

    if( OnItemClick != null ) {
      OnItemClick(item);
    }

    if( item == null ) {
      // textRight.text = "";
    } else {
      // Character character = currentCharacter.Clone();
      // character.Equip("weapon", item);
      // textRight.text = GetStats(character);
    }
    currentItem = item;
  }

  public void SetItems(List<Item> newItems) {
    items = newItems;

    for(int i=0; i<toggleItems.Length; i++) {
      int currentIndex = i;

      EventTrigger.Entry entry = new EventTrigger.Entry();
      entry.eventID = EventTriggerType.PointerClick;
      entry.callback.AddListener( delegate { ToggleClicked(currentIndex); } );
      toggleItems[i].GetComponent<EventTrigger>().triggers.Add(entry);
    }

    btnBack.gameObject.SetActive( items.Count > toggleItems.Length );
    btnNext.gameObject.SetActive( items.Count > toggleItems.Length );

    ShowPage(0);
  }

  private void ShowPage(int page) {
    currentPage = page;

    for(int i = 0; i<toggleItems.Length; i++) {
      ChangeToggle(toggleItems[i], true);

      int itemIndex = i + (currentPage * toggleItems.Length);


      if( itemIndex < items.Count ) {
       toggleItems[i].gameObject.transform.GetChild(1).GetComponent<Image>().sprite = GameEngine.instance.popupEngine.itemsSpriteSheet.Get( items[itemIndex].prefab );
     }
      if( itemIndex < items.Count && items[itemIndex] == currentItem ) {
        toggleItems[i].isOn = true;
      }
    }

    int min = 0;
    int max = Mathf.Min(toggleItems.Length, items.Count - (currentPage * toggleItems.Length));
    for(int i = max; i<toggleItems.Length; i++) {
      ChangeToggle(toggleItems[i], false);
    }

    btnBack.interactable = ( page > 0 );
    btnNext.interactable = ( page < Mathf.Floor(items.Count / toggleItems.Length) );
  }

  public void ToggleClicked(int buttonIndex = 0) {
    if( currentItem == items[ (currentPage * toggleItems.Length) + buttonIndex] ) {
      // buttons[0].gameObject.SetActive( false );
      ShowItem(null);
    } else {
      // buttons[0].gameObject.SetActive( true );
      ShowItem(items[ (currentPage * toggleItems.Length) + buttonIndex]);
    }
  }

  private void ChangePage(int newPage) {
    ShowPage( currentPage + newPage );
  }

  private void ChangeToggle(Toggle t, bool state) {
    if( state ) {
      t.isOn = false;
    }
    t.interactable = state;
    Color c = t.gameObject.transform.GetChild(1).GetComponent<Image>().color;
    c.a = (state ? 1 : 0);
    t.gameObject.transform.GetChild(1).GetComponent<Image>().color = c;
  }

}
