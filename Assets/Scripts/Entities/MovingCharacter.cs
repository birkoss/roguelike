using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MovingCharacter: MonoBehaviour {

  protected Character character;

  public void SetCharacter(Character newCharacter) {
    character = newCharacter;
  }
  public Character GetCharacter() {
    return character;
  }

  public virtual void CheckStatus() {

  }
}
