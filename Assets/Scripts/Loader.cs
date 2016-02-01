using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

  public GameObject gameEngine;

	void Awake () {
    if( GameEngine.instance == null ) {
      Instantiate(gameEngine);
    }
	}
  
}
