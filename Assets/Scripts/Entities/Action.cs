using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Action  {
  public string type;
  public GameObject gameObject;

  public Action(string _type = "", GameObject _gameObject = null) {
    type = _type;
    gameObject = _gameObject;
  }
}
