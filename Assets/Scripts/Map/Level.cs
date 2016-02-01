using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level {
  public string name;
  public string theme;
  public string type;
  public int nbrLevel;

  public Level(string _name, string _theme, int _nbrLevel = 10) {
    name = _name;
    theme = _theme;
    type = "spreadable";
    nbrLevel = _nbrLevel;
  }

}
