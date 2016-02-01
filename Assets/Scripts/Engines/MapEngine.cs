using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapEngine : MonoBehaviour {

  public GameObject[] themePrefabs;
  private MapTheme theme;

  public GameObject[] tilesGUI;

  public GameObject[] tilesPlayers;
  public GameObject[] tilesEntities;

  public GameObject[] tilesPreview;

  public GameObject[] tilesAnimations;

  private GameObject mapImage;

  private Dictionary<Vector2, Room> rooms = new Dictionary<Vector2, Room>();
  private Vector2 currentRoom;

  private int maxRooms = 10;

  private Transform containerMap;

  private Vector2 sizeViewport;

  private List<Character> players;

  private Level level;
  private int currentLevel;

  public void Init(Vector2 _sizeViewport, Level _level) {
    sizeViewport = _sizeViewport;
    level = _level;

    containerMap = new GameObject("Map").transform;
    containerMap.SetParent( transform );

    players = new List<Character>();
    Character c = new Character(10, 0, 49, 0, 0);
    c.name = "Fighter";
    c.actions.Add( new Action("attack") );

    c.health = c.maxHealth = 20;
    c.attack = 13;
    c.defense = 6;
    c.experience = 0;
    c.gold = 0;

    players.Add( c );

    theme = themePrefabs[0].GetComponent<MapTheme>();

    currentLevel = 0;
    NewLevel();
  }

  public void NewLevel() {
    currentLevel++;

    GameObject.Find("Panel Text").GetComponent<Text>().text = level.name + " (" + currentLevel + "/" + level.nbrLevel + ")";

    this.rooms.Clear();
    GenerateRooms((int)sizeViewport.x, (int)sizeViewport.y - (int)(GameObject.Find("Panel Background").GetComponent<RectTransform>().rect.height / GameEngine.instance.size), maxRooms);

    mapImage = GameObject.Find("MiniMap");

    LoadRoom();
  }

  public void ChangeRoom(int direction) {
    currentRoom = rooms[currentRoom].Get( direction );
  }

  public void LoadRoom(int direction = 0) {
    foreach ( Transform t in containerMap ) {
      Destroy( t.gameObject );
    }

    rooms[currentRoom].Generate(level);

    GameObject go;

    for(int y=0; y<height(); y++) {
      for(int x=0; x<width(); x++) {
        if( rooms[currentRoom].floors[x, y] > 0 ) {
          GameObject floor = (Instantiate(theme.GetFloor(rooms[currentRoom].floors[x, y]), new Vector3(x, y, 0f), Quaternion.identity) as GameObject);
          floor.name = "floor_" + x + "x" + y;
          floor.transform.SetParent(containerMap);
        }

        if( rooms[currentRoom].decors[x, y] == 1 ) {
          GameObject decor = (Instantiate(theme.GetDecor(), new Vector3(x, y, 0f), Quaternion.identity) as GameObject);
          decor.name = "decor_" + x + "x" + y;
          decor.transform.SetParent(containerMap);
        }

        if( rooms[currentRoom].doors[x, y] > 0 ) {
          GameObject action = (Instantiate(tilesGUI[rooms[currentRoom].doors[x, y]-1], new Vector3(x, y, 0f), Quaternion.identity) as GameObject);
          action.name = "action_" + rooms[currentRoom].doors[x, y];
          action.transform.SetParent(containerMap);

          List<Vector2> positions = new List<Vector2>();
          switch( rooms[currentRoom].doors[x, y] ) {
            case 1:
              positions = GetAllPositions((int)x, -1, (int)y + 1, (int)sizeViewport.y);
              break;
            case 2:
              positions = GetAllPositions(-1, (int)y, (int)x + 1, (int)sizeViewport.x);
              break;
            case 3:
              positions = GetAllPositions((int)x, -1, (int)y, 0);
              break;
            case 4:
              positions = GetAllPositions(-1, (int)y, (int)x, 0);
              break;
          }

          for(int i=0; i<positions.Count; i++) {
            GameObject floor = (Instantiate(theme.GetFloor(), new Vector3(positions[i].x, positions[i].y, 0f), Quaternion.identity) as GameObject);
            floor.name = "filler_" + positions[i].x + "x" + positions[i].y;
            floor.transform.SetParent(containerMap);
          }
        }
      }
    }

    // Add entities
    for(int i=0; i<rooms[currentRoom].entities.Count; i++) {
      switch( rooms[currentRoom].entities[i].type ) {
        case Entity.Type.Enemy:
          go = (Instantiate(tilesEntities[rooms[currentRoom].entities[i].sprite], new Vector3(rooms[currentRoom].entities[i].x, rooms[currentRoom].entities[i].y, 0f), Quaternion.identity) as GameObject);
          go.name = "entity_" + rooms[currentRoom].entities[i].x + "x" + rooms[currentRoom].entities[i].y;
          go.transform.SetParent(containerMap);

          go.GetComponent<LivingCharacter>().SetCharacter( (rooms[currentRoom].entities[i] as Character) );
          go.GetComponent<LivingCharacter>().CheckStatus();
          go.GetComponent<AttackingCharacter>().SetCharacter( (rooms[currentRoom].entities[i] as Character) );
          rooms[currentRoom].entities[i].gameObject = go;
          break;
        case Entity.Type.Chest:
          go = (Instantiate(tilesEntities[rooms[currentRoom].entities[i].sprite], new Vector3(rooms[currentRoom].entities[i].x, rooms[currentRoom].entities[i].y, 0f), Quaternion.identity) as GameObject);
          go.name = "chest_" + rooms[currentRoom].entities[i].x + "x" + rooms[currentRoom].entities[i].y;
          go.transform.SetParent(containerMap);
          go.GetComponent<OpeningEntity>().SetEntity( (rooms[currentRoom].entities[i] as Chest) );
          go.GetComponent<OpeningEntity>().CheckStatus();
          rooms[currentRoom].entities[i].gameObject = go;
          break;
        case Entity.Type.Exit:
          go = (Instantiate(theme.GetExit(), new Vector3(rooms[currentRoom].entities[i].x, rooms[currentRoom].entities[i].y, 0f), Quaternion.identity) as GameObject);
          go.name = "exit_" + rooms[currentRoom].entities[i].x + "x" + rooms[currentRoom].entities[i].y;
          go.transform.SetParent(containerMap);
          rooms[currentRoom].entities[i].gameObject = go;
          break;
      }


    }

    // Add players (Change choose random position)
    int directionFrom = 0;
    Vector2 entranceDoorPosition = new Vector2();
    switch( direction ) {
      case 1:
        directionFrom = 3;
        break;
      case 2:
        directionFrom = 4;
        break;
      case 3:
        directionFrom = 1;
        break;
      case 4:
        directionFrom = 2;
        break;
    }
    if( directionFrom > 0 ) {
      for(int y=0; y<height(); y++) {
        for(int x=0; x<width(); x++) {
          if( rooms[currentRoom].doors[x, y] == directionFrom ) {
            entranceDoorPosition = new Vector2(x, y);
          }
        }
      }
    }

    for(int i=0; i<players.Count; i++) {
      if( directionFrom == 0 ) {
        Vector2 position = rooms[currentRoom].GetRandomPosition();
        players[i].x = (int)position.x;
        players[i].y = (int)position.y;
      } else {
        Vector2 position = GetEmptyPositionNear( entranceDoorPosition );
        players[i].x = (int)position.x;
        players[i].y = (int)position.y;
      }
      go = (Instantiate(tilesPlayers[players[i].sprite], new Vector3(players[i].x, players[i].y, 0f), Quaternion.identity) as GameObject);
      go.name = "player_" + players[i].x + "x" + players[i].y;
      go.transform.SetParent(containerMap);

      go.GetComponent<AttackingCharacter>().SetCharacter( players[i] );
      go.GetComponent<LivingCharacter>().SetCharacter( players[i] );
      players[i].gameObject = go;
    }
  }

  public void Move(GameObject gameObject, int x, int y) {
    List<Condition> conditions = new List<Condition>();
    conditions.Add( new Condition("type", "==", Entity.Type.Enemy.ToString()) );
    List<Character> characters = GetCharacters();
    for(int i=0; i<characters.Count; i++) {
      if( characters[i].gameObject == gameObject ) {
        gameObject.transform.position = new Vector3(x, y, 0);
        characters[i].x = x;
        characters[i].y = y;
        break;
      }
    }
  }


  public Entity GetEntity(GameObject gameObject) {
    for(int i=0; i<rooms[currentRoom].entities.Count; i++) {
      if( rooms[currentRoom].entities[i].gameObject == gameObject ) {
        return rooms[currentRoom].entities[i];
      }
    }
    return null;
  }

  public bool IsEmpty(int x, int y) {
    for(int i=0; i<players.Count; i++) {
      if( players[i].x == x && players[i].y == y ) {
        return false;
      }
    }

    return rooms[currentRoom].IsEmpty(x, y);
  }


  public List<Character> GetCharacters(List<Condition> conditions = null) {
    conditions = conditions ?? new List<Condition>();

    List<Character> characters = new List<Character>();

    for(int p=0; p<players.Count; p++) {
      characters.Add( players[p] );
    }
    for(int p=0; p<rooms[currentRoom].entities.Count; p++) {
      if( rooms[currentRoom].entities[p].type == Entity.Type.Enemy  ) {
        characters.Add( rooms[currentRoom].entities[p] as Character );
      }
    }

    if( conditions.Count > 0 ) {
      List<Character> allCharacters = characters;
      characters = new List<Character>();
      for(int i=0; i<allCharacters.Count; i++) {
        bool isOk = true;
        for(int c=0; c<conditions.Count; c++) {
          switch( conditions[c].type ) {
            case "type":
              if( conditions[c].operation == "==" ) {
                if( allCharacters[i].type != (Entity.Type)(System.Int32.Parse(conditions[c].value)) ) {
                  isOk = false;
                }
              } else if( conditions[c].operation == "!=" ) {
                if( allCharacters[i].type == (Entity.Type)(System.Int32.Parse(conditions[c].value)) ) {
                  isOk = false;
                }
              }
              break;
            case "health":
              if( conditions[c].operation == ">" ) {
                if( allCharacters[i].health <= System.Int32.Parse(conditions[c].value) ) {
                  isOk = false;
                }
              }
              break;
          }
        }
        if( isOk ) {
          characters.Add( allCharacters[i] );
        }
      }
    }

    return characters;
  }


  private List<Vector2> GetAllPositions(int x, int y, int from, int to) {
    List<Vector2> positions = new List<Vector2>();
    if( from < to ) {
      for( int i=from; i<to; i++ ) {
        Vector2 v = new Vector2(x, i);
        if( x == -1 ) {
          v = new Vector2(i, y);
        }
        positions.Add( v );
      }
    } else {
      for( int i=from; i>=to; i-- ) {
        Vector2 v = new Vector2(x, i);
        if( x == -1 ) {
          v = new Vector2(i, y);
        }
        positions.Add( v );
      }
    }
    return positions;
  }

  private void GenerateRooms(int maxWidth, int maxHeight, int maxRooms) {
    currentRoom = new Vector2( (int)maxWidth/2, (int)maxHeight/2 );
    List<Vector2> queue = new List<Vector2>();

    rooms.Add( currentRoom, new Room(maxWidth, maxHeight));
    queue.Add( currentRoom );

    while(queue.Count > 0 && rooms.Count < maxRooms ) {
      Vector2 current = queue[0];
      queue.RemoveAt(0);

      List<Vector2> neighboors = FindNeighboors(current.x, current.y, maxWidth, maxHeight);
      int doors = Random.Range(1, 3);
      while(neighboors.Count > 0 && doors > 0 ) {
        int neighboorsIndex = Random.Range(0, neighboors.Count);
        Vector2 neighboor = neighboors[neighboorsIndex];
        neighboors.RemoveAt(neighboorsIndex);
        if( !rooms.ContainsKey( neighboor) ) {
          rooms.Add( neighboor, new Room(maxWidth, maxHeight));
          queue.Add( neighboor );

          Vector2 direction = new Vector2();
          Vector2 difference = new Vector2( current.x - neighboor.x, current.y - neighboor.y );
          if( difference.x < 0 ) {
            direction.x = 4;
            direction.y = 2;
          } else if ( difference.x > 0 ) {
            direction.x = 2;
            direction.y = 4;
          } else if( difference.y < 0 ) {
            direction.x = 1;
            direction.y = 3;
          } else {
            direction.x = 3;
            direction.y = 1;
          }

          rooms[current].Add((int)direction.x, neighboor);
          rooms[neighboor].Add((int)direction.y, current);

          doors--;
        }
      }
    }

    // Set the enemies/chests into the rooms
    // ------------------------------------------------------------------------
    int nbrEnemies = 12;
    int nbrChests = 6;

    List<Vector2> keys = new List<Vector2>();
    foreach (KeyValuePair<Vector2, Room> room in rooms) {
      keys.Add(room.Key);
    }
    keys.Remove(currentRoom); // Nothing where we start...

    for(int i=0; i<nbrEnemies; i++) {
      string enemyName = TableMaster.Instance().GetTable("Enemies").Generate();
      Enemy e = EnemyMaster.Instance().GetEnemy( enemyName );

      int depth = 1;
      int mod = (int)Mathf.Floor( depth / 5 );

      Character c = new Character(1, e.prefab, e.speed, 0, 0, Entity.Type.Enemy);
      c.name = enemyName;

      c.actions.Add( new Action("attack") );

      int maxValue = (depth - (mod * 5)) * 5;

      c.health = c.maxHealth = (e.health * (mod+1) ) + Random.Range(0, maxValue);
      c.attack = (e.attack * (mod+1) ) + Random.Range(0, (int)Mathf.Floor(maxValue/10));
      c.defense = (e.defense * (mod+1) ) + Random.Range(0, (int)Mathf.Floor(maxValue/10));
      // c.experience = 2 * (mod + 1);
      // c.gold = 10 * (mod + 1);

      rooms[ keys[ Random.Range(0, keys.Count-1)  ] ].entities.Add( c );
    }


    for(int r=0; r<nbrChests; r++) {
      Chest c = new Chest( 0, 0 );
      c.item = ItemMaster.Instance().Get( TableMaster.Instance().GetTable("Weapon").Generate() );
      rooms[ keys[ Random.Range(0, keys.Count-1)  ] ].entities.Add( c );
    }

    Entity exit = new Entity(Entity.Type.Exit);
    rooms[ keys[ Random.Range(0, keys.Count-1)  ] ].entities.Add( exit );

  }

  // Find all neighboors from the current position (not out of bounds)
  // --------------------------------------------------------------------------
  List<Vector2> FindNeighboors(float x, float y, int width, int height) {
    List<Vector2> neighboors = new List<Vector2>();
    for(int y2=-1; y2<=1; y2++) {
      for(int x2=-1; x2<=1; x2++) {
        if( Mathf.Abs(x2) != Mathf.Abs(y2) ) {
          Vector2 current = new Vector2(x + x2, y + y2);
          if( current.x >= 0 && current.x < width && current.y >=0 && current.y < height ) {
            neighboors.Add( current );
          }
        }
      }
    }
    return neighboors;
  }

  private int height() {
    if( rooms.ContainsKey(currentRoom) ) {
      return rooms[currentRoom].height;
    }
    return 1;
  }

  private int width() {
    if( rooms.ContainsKey(currentRoom) ) {
      return rooms[currentRoom].width;
    }
    return 1;
  }


  public void SaveState() {
    Debug.Log("SaveState...");

  }

  public Vector2 GetEmptyPositionNear(Vector2 startPosition) {
    List<Vector2> queue = new List<Vector2>();
    List<Vector2> positionChecked = new List<Vector2>();

    queue.Add( startPosition );
    positionChecked.Add( startPosition );

    while( queue.Count > 0 ) {
      Vector2 current = queue[0];

      if( IsEmpty((int)current.x, (int)current.y) ) {
        return current;
      }

      queue.RemoveAt(0);

      List<Vector2> neighboors = FindNeighboors(current.x, current.y, width(), height());
      for(int i=0; i<neighboors.Count; i++) {
        if( !positionChecked.Contains( neighboors[i] ) ) {
          queue.Add( neighboors[i] );
          positionChecked.Add(neighboors[i]);
        }
      }
    }
    return rooms[currentRoom].GetRandomPosition();
  }


}
