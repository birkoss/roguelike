using UnityEngine;
using System.Collections;

public class LivingCharacter : MovingCharacter {

  public Sprite deadSprite;

  public bool IsAlive() {
    return character.IsAlive();
  }

  public void SetHealth(int newHealth) {
    character.health = newHealth;
    CheckStatus();
  }

  public void TakeDamage(int damage) {
    character.health = Mathf.Max(character.health-damage, 0);
    CheckStatus();
  }

  public void heal(int point) {
    character.health += point;

    if( tag == "Player" ) {
      // GameEngine.instance.RefreshPanel();
    }
  }

  public override void CheckStatus() {
    if( !IsAlive() ) {
      GetComponent<Animator>().enabled = false;
      GetComponent<SpriteRenderer>().sprite = deadSprite;
      // Still want us to collide with it, and leave a trace there....
      // GetComponent<BoxCollider2D>().enabled = false;
      // GetComponent<SpriteRenderer>().sortingOrder = 0;

      if( tag == "Player" ) {
        GameEngine.instance.GameOver();
      }
    } else if( tag == "Player" ) {
      // GameEngine.instance.RefreshPanel();
      // GameEngine.instance.GetComponent<TurnEngine>().ClearActions();
    }
  }
}
