using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TurnEngine : MonoBehaviour {

  private List<Character> turns;
  private int currentTurn = -1;

  private float turnDelay = 0.5f;//0.2f

  public bool isWaiting = false;
  public bool isActive = false;

  private GameObject currentCharacterIndicator;

  // Unity
  // --------------------------------------------------------------------------
	void Awake () {
    turns = new List<Character>();
	}


  // Turn Engine
  // --------------------------------------------------------------------------
  public void Init(List<Character> characters) {
    turns.Clear();
    for(int speed=1; speed <=100; speed++) {
      for(int c=0; c<characters.Count; c++) {
        if( speed % characters[c].speed == 0 ) {
          turns.Add( characters[c] );
        }
      }
    }
    currentTurn = -1;
    isActive = true;
  }



  public Character CurrentCharacter() {
    return turns[currentTurn];
  }

  private void AI() {
    Debug.Log("AI...");

    // TODO: Better AI :P
    Character c = CurrentCharacter();

    for(int i=0; i<c.actions.Count; i++) {
      if( c.actions[i].type == "attack" ) {
        List<Condition> conditions = new List<Condition>();
        conditions.Add( new Condition("type", "!=", ((int)c.type).ToString()) );
        conditions.Add( new Condition("health", ">", "0"));
        List<Character> characters = GameEngine.instance.mapEngine.GetCharacters(conditions);

        for(int j=0; j<characters.Count; j++) {

          Vector2 position = new Vector2(-1, -1);
          Debug.Log("C:" + characters[j].x + "x" + characters[j].y );
          for(int y=characters[j].y-c.range; y<=characters[j].y+c.range; y++) {
            for(int x=characters[j].x-c.range; x<=characters[j].x+c.range; x++) {
              if( x == characters[j].x || y == characters[j].y ) {
                if( position.x == -1 && GameEngine.instance.mapEngine.IsEmpty(x, y) ) {
                  position = new Vector2(x, y);
                  break;
                }
              }
            }
          }
          if( position.x != -1 ) {
            GameEngine.instance.mapEngine.Move(c.gameObject, (int)position.x, (int)position.y);
            break;
          }

        }
      }
    }

    EndTurn();
  }

  public Action SimulateAction(Character c) {
    Debug.Log("Simulate Action...");
    Debug.Log("C: " + c.type + " / " + c.range + " at " + c.x + "x" + c.y);

    Action action = new Action("nothing");

    //Character c = CurrentCharacter();

    // bool nothingToDo = true;
    for(int i=0; i<c.actions.Count; i++) {
      if( c.actions[i].type == "attack" ) {
        List<Character> characters = GameEngine.instance.FindCharactersNear(c);
        if( characters.Count > 0 ) {
          action.type = "attack";
          action.gameObject = characters[0].gameObject;
          break;
        }
      }
    }

    Debug.Log("Type: " + action.type);

    return action;
  }

  private void ExecuteAction() {
    Debug.Log("Execute Action...");
    Character c = CurrentCharacter();
    Action action = SimulateAction( c );
    if( action.type == "attack" ) {
      c.gameObject.GetComponent<AttackingCharacter>().Attack( action.gameObject );
    } else {
      NextTurn();
    }
  }

  // Turn
  // --------------------------------------------------------------------------
  public void StartTurn() {
    if( !isActive ) {
      return ;
    }

    for(int i=0; i<turns.Count; i++) {
      currentTurn++;
      if( currentTurn >= turns.Count ) {
        currentTurn = 0;
      }
      if( turns[currentTurn].gameObject.GetComponent<LivingCharacter>().IsAlive() ) {
        break;
      }
    }

  //  if( squares.Count == 0 ) {
  if( currentCharacterIndicator == null || currentCharacterIndicator.transform.position != CurrentCharacter().gameObject.transform.position ) {
    if( currentCharacterIndicator != null ) {
    Debug.Log("Deleting...");
    Destroy( currentCharacterIndicator );
  }
      currentCharacterIndicator = (Instantiate(GameEngine.instance.mapEngine.tilesPreview[2], new Vector3(CurrentCharacter().gameObject.transform.position.x, CurrentCharacter().gameObject.transform.position.y, 0f), Quaternion.identity) as GameObject);
      currentCharacterIndicator.transform.SetParent( CurrentCharacter().gameObject.transform );
    }
//      squares.Add( go );
    //}

    isWaiting = ( turns[currentTurn].type == Entity.Type.Player );
    if( !isWaiting ) {
      AI();
    }
  }

  public void EndTurn() {
    Debug.Log("End Turn...");
    ExecuteAction();
  }

  public void NextTurn() {
    GameEngine.instance.mapEngine.SaveState();
    StartCoroutine(EndOfTurnDelay());
  }

  IEnumerator EndOfTurnDelay() {
    yield return new WaitForSeconds(turnDelay);

    StartTurn();
  }

}
