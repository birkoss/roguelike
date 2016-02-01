using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PopupButtons : MonoBehaviour {

  public Button[] buttons;
  public int currentButton;

  public delegate void OnClosePanelRequest();
	public event OnClosePanelRequest OnClosePanel;

  public void Awake() {
    for(int i=currentButton; i<buttons.GetLength(0); i++) {
      buttons[i].gameObject.SetActive( false );
    }
  }

  public void Add(string label, UnityAction action = null) {
    if( currentButton < buttons.GetLength(0) ) {
      buttons[currentButton].gameObject.transform.GetChild(0).GetComponent<Text>().text = label;
      buttons[currentButton].gameObject.SetActive( true );
      buttons[currentButton].onClick.AddListener( ClosePanel );
      if( action != null ) {
        buttons[currentButton].onClick.AddListener( action );
      }
      currentButton++;
    }
  }

  public void ClosePanel() {
    OnClosePanel();
  }
}
