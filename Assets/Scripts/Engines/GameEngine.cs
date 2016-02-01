using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
 using System.IO;

public class GameEngine : MonoBehaviour {

  public MapEngine mapEngine;
  public TurnEngine turnEngine;
  public PopupEngine popupEngine;
  public MessageEngine messageEngine;

  public static GameEngine instance = null;

  public int size = 48;
  private Vector2 maxDimension;

  // Unity
  // --------------------------------------------------------------------------
	private void Awake () {
    if( instance == null ) {
      instance = this;
    } else  if( instance != this ) {
      Destroy(gameObject);
    }

    DontDestroyOnLoad(gameObject);

    // Find the correct camera size depending on the size of the screen
    // ------------------------------------------------------------------------
    Vector2 targetViewport = new Vector2(320, 480);

    float percentageX = Screen.width/targetViewport.x;
    float percentageY = Screen.height/targetViewport.y;
    float targetSize = 0.0f;
    if( percentageX > percentageY ) {
      targetSize = percentageY;
    } else {
      targetSize = percentageX;
    }
    int floored = Mathf.FloorToInt(targetSize);
    if(floored < 1) {
      floored = 1;
    }

    // Set the camera size and position and find the maxDimension of the map
    // ------------------------------------------------------------------------
    Camera.main.orthographicSize = ((Screen.height/2)/floored)/size;

    maxDimension = new Vector2(((2f * Camera.main.orthographicSize) * Camera.main.aspect), (2f * Camera.main.orthographicSize));
    maxDimension.x = Mathf.Ceil(maxDimension.x);
    maxDimension.y = Mathf.Ceil(maxDimension.y);

    Camera.main.transform.position = new Vector3( (maxDimension.x/2)-0.5f, (maxDimension.y/2)-0.5f, -10 );

    // Load externals data from XML
    // ------------------------------------------------------------------------
    TableMaster.Instance().Load("loots");
    EnemyMaster.Instance().Load("enemies");
    ItemMaster.Instance().Load("items");

    // Gentleman, starts your engine
    // ------------------------------------------------------------------------
    mapEngine = GetComponent<MapEngine>();
    turnEngine = GetComponent<TurnEngine>();
    popupEngine = GetComponent<PopupEngine>();
    messageEngine = GetComponent<MessageEngine>();
	}

  private void Start() {
    mapEngine.Init(maxDimension, new Level("Dark Forest", "Forest", 5));

    PrepareRoom();

    GameObject.Find("ButtonMap").GetComponent<Button>().onClick.AddListener( TestPopup );
  }


  public void TestPopup() {


    // List<Item> items = new List<Item>();
    // items.Add( ItemMaster.Instance().Get("Wooden Sword") );
    // items.Add( ItemMaster.Instance().Get("Wooden Spear") );
    //
    // // items.Add( new Item("Bouba") );
    // // items.Add( new Item("Bouba2") );
    // // items.Add( new Item("Bouba2") );
    // // items.Add( new Item("Bouba2") );
    // // items.Add( new Item("Bouba2") );
    // // items.Add( new Item("Bouba2") );
    // // items.Add( new Item("Bouba2") );
    // // items.Add( new Item("Bouba2") );
    // //
    // // items.Add( new Item("Bouba2") );
    //
    // Popup popup = popupEngine.Create("Popup");
    // popup.SetTitle("test 123");
    // // popup.SetCharacter(mapEngine.GetCharacters()[0]);
    // popup.SetItem( ItemMaster.Instance().Get("Wooden Sword") );
    // PopupItems pi = popup.SetItems( items );
    // pi.OnItemClick += OnItemClick;
    //
    // popup.AddButton("ok");
    //
    // Debug.Log("...");

    // popup.GetComponent<ModalPanelEquip>().SetCharacter( mapEngine.GetCharacters()[0] );
    //
    // popup.GetComponent<ModalPanelEquip>().AddButton("Equip");
    // popup.GetComponent<ModalPanelEquip>().AddButton("Cancel");
    //
    // popup.GetComponent<ModalPanelEquip>().SetItems( items );

    messageEngine.Create("This is a text...");

  }

  public void OnItemClick(Item i) {
    popupEngine.GetCurrentPopup().SetItem( i );
  }

  // Character
  // --------------------------------------------------------------------------
  public List<Character> FindCharactersNear(Character c) {
    List<Character> characters = new List<Character>();

    List<Condition> conditions = new List<Condition>();
    conditions.Add( new Condition("type", "!=", ((int)c.type).ToString() ) );
    conditions.Add( new Condition("health", ">", "0"));
    List<Character> allCharacters = mapEngine.GetCharacters(conditions);
    for(int i=0; i<allCharacters.Count; i++) {
      int distanceX = (int)Mathf.Abs(allCharacters[i].x - c.x);
      int distanceY = (int)Mathf.Abs(allCharacters[i].y - c.y);
      if( distanceX + distanceY <= c.range ) {
        characters.Add( allCharacters[i] );
      }
    }
    return characters;
  }


  // Popup
  // --------------------------------------------------------------------------
  public void ShowPopup(GameObject gameObject) {
    if( gameObject.tag == "ChangeRoom" ) {
      int direction = System.Int32.Parse( (gameObject.name.Split("_"[0])[1]) );
      mapEngine.ChangeRoom( direction );
      mapEngine.LoadRoom( direction );
      PrepareRoom();
    } else if( gameObject.tag == "Enemy" || gameObject.tag == "Player" ){
      List<Character> characters = mapEngine.GetCharacters();
      for(int i=0; i<characters.Count; i++) {
        if( characters[i].gameObject == gameObject ) {
          Popup popup = popupEngine.Create("Popup");
          popup.SetCharacter( characters[i] );
          popup.AddButton("OK");
          break;
        }
      }
    } else if( gameObject.tag == "Container" ) {
      if( !gameObject.GetComponent<OpeningEntity>().IsOpen() ) {
        Popup popup = popupEngine.Create("Popup");
        popup.SetTitle("You found a new chest! What do you want to do?", gameObject.GetComponent<SpriteRenderer>().sprite);

        popup.targetGameObject = gameObject;

        popup.AddButton("Open", new UnityAction(OpenContainer));
        popup.AddButton("Cancel");

      } else {
        Popup popup = popupEngine.Create("Popup");
        popup.SetTitle("This chest is already empty!", gameObject.GetComponent<SpriteRenderer>().sprite);
        popup.AddButton("Ok");
      }
    } else if ( gameObject.tag == "Exit" ) {
      Popup popup = popupEngine.Create("Popup");
      popup.SetTitle("This is the exit! Go to the next level?", gameObject.GetComponent<SpriteRenderer>().sprite);
      popup.AddButton("Yes", new UnityAction(NextLevel));
      popup.AddButton("No");
    }
  }

  public void ShowMessage(string message, int x, int y) {
    Vector3 v = Camera.main.WorldToViewportPoint(new Vector3(x, y, 0));
    GameObject.Find("Damage").transform.position = v;
    GameObject.Find("Damage").GetComponent<Text>().text = message;
  }

  public void NextLevel() {
    this.mapEngine.NewLevel( );
  }

  public void OpenContainer() {
    Popup currentPopup = popupEngine.GetCurrentPopup();
    currentPopup.targetGameObject.GetComponent<OpeningEntity>().Open( );
    Popup popup = popupEngine.Create("Popup");
    popup.SetTitle("You found a new item!");
    popup.SetItem( (mapEngine.GetEntity(currentPopup.targetGameObject) as Chest).item );

    List<Condition> conditions = new List<Condition>();
    conditions.Add(new Condition("type", "==", ((int)Entity.Type.Player).ToString()));

    List<Character> characters = mapEngine.GetCharacters(conditions);
    popup.SetCharacter( characters[0] );

    popup.TryItem("weapon", popup.GetItem());

    popup.AddButton("Equip", new UnityAction(EquipContainerItem));
    popup.AddButton("Add to the bag", new UnityAction(UseContainerItem));
  }

  public void UseContainerItem() {
    NextTurn();
  }

  public void EquipContainerItem() {
    Popup currentPopup = popupEngine.GetCurrentPopup();
    Item item = currentPopup.GetItem();

    List<Condition> conditions = new List<Condition>();
    conditions.Add(new Condition("type", "==", ((int)Entity.Type.Player).ToString()));

    List<Character> characters = mapEngine.GetCharacters(conditions);

    if( characters[0].HasEquipment("weapon") ) {
      Debug.Log("PROBLEM!!!!");
    } else {
      characters[0].Equip("weapon", item);
      NextTurn();
    }

  }

  public void SellContainerItem() {
    NextTurn();
  }

  // Map/Turn Engine
  // --------------------------------------------------------------------------
  public void EndTurn() {
    turnEngine.EndTurn();
  }

  public bool IsWaiting() {
    return turnEngine.isWaiting;
  }

  public bool IsActive() {
    if( popupEngine.IsActive() ) {
      return false;
    }
    return turnEngine.isActive;
  }

  public void NextTurn() {
    turnEngine.NextTurn();
  }

  private void PrepareRoom() {
    turnEngine.Init( mapEngine.GetCharacters() );
    StartTurn();
  }

  public void StartTurn() {
    turnEngine.StartTurn();
  }

  public void GameOver() {
    turnEngine.isActive = false;
    Debug.Log("GameOver");
  }
}
