using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PopupTitle : MonoBehaviour {

  public Text textTitle;
  public Image icon;

  public void SetTitle(string title, Sprite newIcon = null ) {
    textTitle.text = title;

    if( newIcon == null ) {
      icon.gameObject.transform.parent.gameObject.SetActive( false );
    } else {
      icon.sprite = newIcon;
    }
  }

}
