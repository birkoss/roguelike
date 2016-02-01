using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Condition  {
  public string type;
  public string operation;
  public string value;

  public Condition(string _type = "", string _operation = "", string _value = "") {
    type = _type;
    operation = _operation;
    value = _value;
  }
}
