using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MessageEngine : MonoBehaviour {

  public GameObject messagePrefab;

  private List<Message> messages;

  private GameObject container;

	void Awake () {
    messages = new List<Message>();

    container = GameObject.Find("Canvas");
	}

  public Message Create(string text) {
    GameObject messageGameObject = (Instantiate(messagePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject);
    Message message = messageGameObject.GetComponent<Message>();
    message.SetText(text);

    messageGameObject.transform.SetParent( container.transform, false );

    messages.Add( message );

    return message;
  }

  public void FixedUpdate() {
    // if( background != null ) {
    //   if( background.GetComponent<Animator>().GetNextAnimatorStateInfo(0).IsName("End") ) {
    //     Destroy(background);
    //   }
    // }
    // for(int i=messages.Count-1; i>=0; i--) {
    //   if( messages[i] != null ) {
    //     if( messages[i].GetComponent<Animator>().GetNextAnimatorStateInfo(0).IsName("End") ) {
    //       Destroy(messages[i].gameObject);
    //       messages.RemoveAt(i);
    //     }
    //   }
    // }
    // if( messages.Count == 0 && background != null ) {
    //   background.GetComponent<Animator>().Play("FadeOut");
    // }
  }
}
