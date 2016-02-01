using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectableElement : MonoBehaviour {
  private bool isDragging;
  private bool isSelected;

  private float timeSelectedStart;
  private float timeSelectedThreshold = 0.15f;

  private GameObject gameObjectSelected;
  private GameObject tileSelected;

  private List<GameObject> squares;

  void Start() {
    isDragging = false;
    isSelected = false;

    squares = new List<GameObject>();
  }

  void Update() {
    if(  GameEngine.instance.IsActive() && GameEngine.instance.IsWaiting() ) {
      if( Input.GetMouseButton(0) ) {
        if( isSelected ) {
          if( isDragging ) {
            OnDrag();
          } else {
            if( timeSelectedStart + timeSelectedThreshold <= Time.time && gameObjectSelected == GameEngine.instance.turnEngine.CurrentCharacter().gameObject ) {
              isDragging = true;
              OnDragStart();
            }
          }
        } else {
          timeSelectedStart = Time.time;
          isSelected = true;

          Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
          Collider2D[] col = Physics2D.OverlapPointAll(v);
          if( col.Length > 0 ) {
            for(int i=col.Length-1; i>=0; i--) {
              if( col[i].transform.gameObject.GetComponent<SpriteRenderer>().sortingLayerName == "Item" ) {
                gameObjectSelected = col[i].transform.gameObject;
                SelectCharacter(gameObjectSelected, true);
                break;
              }
            }
          }
        }
      } else {
        if( isDragging ) {
          isDragging = false;
          OnDragEnd();
        } else if( isSelected ) {
          OnClick();
        }
        isSelected = false;
        gameObjectSelected = null;
      }
    }
  }

  public virtual void OnClick() {
    if( gameObjectSelected != null ) {
      Debug.Log("OnClick..." + gameObjectSelected.name);
      GameEngine.instance.ShowPopup(gameObjectSelected);
      SelectCharacter(gameObjectSelected, false);
      gameObjectSelected = null;
    }
  }

  public virtual void OnDragStart() {
    if( gameObjectSelected != null ) {
      Debug.Log("OnDragStart...");
    }
  }

  public virtual void OnDrag() {
    //  Debug.Log("OnDrag...");
    if( gameObjectSelected != null ) {
      Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      Collider2D[] col = Physics2D.OverlapPointAll(position);
      GameObject newTile = null;

      if( col.Length > 0 ) {
        for(int i=col.Length-1; i>=0; i--) {
          if( col[i].transform.gameObject.GetComponent<SpriteRenderer>().sortingLayerName == "Floor" ) {

            newTile = col[i].transform.gameObject;
            break;
          }
        }
      }

      if( newTile != null && newTile != tileSelected ) {
        // Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        position.z = 0;

        if( GameEngine.instance.mapEngine.IsEmpty((int)position.x, (int)position.y) || (position.x == gameObjectSelected.transform.position.x && position.y == gameObjectSelected.transform.position.y) ) {
          ClearSquares();

          if( (position.x != gameObjectSelected.transform.position.x || position.y != gameObjectSelected.transform.position.y) ) {
            GameObject go = (Instantiate(GameEngine.instance.mapEngine.tilesPreview[0], new Vector3(position.x, position.y, 0f), Quaternion.identity) as GameObject);
            squares.Add( go );

            Character c = GameEngine.instance.turnEngine.CurrentCharacter().Clone();
            c.x = (int)position.x;
            c.y = (int)position.y;

            Action action = GameEngine.instance.turnEngine.SimulateAction(c);
            Debug.Log("T:" + action.type);
            if( action.type == "attack" ) {
              Debug.Log("Condition.type: " + action.type);
              go = (Instantiate(GameEngine.instance.mapEngine.tilesPreview[1], action.gameObject.transform.position, Quaternion.identity) as GameObject);
              squares.Add( go );
            }


          }
          tileSelected = newTile;
        }
      }
    }
  }

  public virtual void OnDragEnd() {
    Debug.Log("OnDragEnd...");
    if( gameObjectSelected != null ) {
      if( squares.Count > 0 ) {
        GameEngine.instance.mapEngine.Move(gameObjectSelected, (int)squares[0].transform.position.x, (int)squares[0].transform.position.y );

      //  gameObjectSelected.transform.position = squares[0].transform.position;

        // gameObjectSelected.GetComponent<AttackingObject>().Attack( GameEngine.instance.mapEngine.ti)

        ClearSquares();

        GameEngine.instance.EndTurn();
      }
      SelectCharacter(gameObjectSelected, false);
      gameObjectSelected = null;
    }
  }

  private void ClearSquares() {
    for(int i=0; i<squares.Count; i++) {
      Destroy(squares[i]);
    }
    squares.Clear();
  }


  private void SelectCharacter(GameObject go, bool state) {
    Color c = go.GetComponent<SpriteRenderer>().material.color;
    c.a = (state ? 0.5f : 1.0f);
    go.GetComponent<SpriteRenderer>().material.color = c;
  }
}
