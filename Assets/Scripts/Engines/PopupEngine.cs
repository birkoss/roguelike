using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PopupEngine : MonoBehaviour {

  public GameObject backgroundPrefab;

  public GameObject[] popupPrefabs;

  public SpriteSheet itemsSpriteSheet;

  private GameObject background;
  private List<Popup> popups;

  private GameObject container;

	void Awake () {
    popups = new List<Popup>();

    container = GameObject.Find("Canvas");
	}

  public Popup Create(string name) {
    if( background == null ) {
      background = (Instantiate(backgroundPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject);
      background.transform.SetParent( container.transform, false );
    }

    //  anim.GetNextAnimatorStateInfo(0).nameHash
    for(int i=0; i<popupPrefabs.Length; i++) {
      if( popupPrefabs[i].name == name ) {
        GameObject popupGameObject = (Instantiate(popupPrefabs[i], new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject);
        Popup popup = popupGameObject.GetComponent<Popup>();

        popupGameObject.transform.SetParent( container.transform, false );

        popups.Add( popup );
        return popup;
      }
    }
    return null;
  }

  public void FixedUpdate() {
    if( background != null ) {
      if( background.GetComponent<Animator>().GetNextAnimatorStateInfo(0).IsName("End") ) {
        Destroy(background);
      }
    }
    for(int i=popups.Count-1; i>=0; i--) {
      if( popups[i] != null ) {
        if( popups[i].GetComponent<Animator>().GetNextAnimatorStateInfo(0).IsName("End") ) {
          Destroy(popups[i].gameObject);
          popups.RemoveAt(i);
        }
      }
    }
    if( popups.Count == 0 && background != null ) {
      background.GetComponent<Animator>().Play("FadeOut");
    }
  }

  public Popup GetCurrentPopup() {
    return popups[ popups.Count - 1 ];
  }

  public bool IsActive() {
    return (popups.Count > 0);
  }
}
