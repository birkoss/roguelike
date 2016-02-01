using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Message : MonoBehaviour {

  public Text textMessage;

  public void SetText(string text) {
    textMessage.text = text;

    StartCoroutine(WaitingToHide());
  }

  public void HideMessage() {
    gameObject.GetComponent<Animator>().Play("FadeOut");
  }

  IEnumerator WaitingToHide() {
    yield return new WaitForSeconds(2);
    HideMessage();
    yield return null;
  }

}
