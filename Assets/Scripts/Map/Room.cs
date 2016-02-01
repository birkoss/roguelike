using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Room {

  enum Direction{Up, Right, Bottom, Left};

  public int width;
  public int height;

  public Dictionary<int, Vector2> neighboors;

  public int[,] floors;
  public int[,] decors;
  public int[,] doors;

  public List<Entity> entities;

  private bool generated = false;

  private int randomFillPercent = 20;

  public Room(int maxWidth, int maxHeight) {
    width = maxWidth;
    height = maxHeight;

    neighboors = new Dictionary<int, Vector2>();
    entities = new List<Entity>();
  }

  public void Generate(Level level) {
    if( !generated ) {
      string levelType = level.type;
      int nbrRegenerations = 10;
      int minimumEmptyTiles = 20;
      List<Vector2> doorPositions = new List<Vector2>();
      while( !generated ) {
        floors = new int[width,height];
        decors = new int[width,height];
        doors = new int[width,height];

        for(int y=0; y<height; y++) {
          for(int x=0; x<width; x++) {
            floors[x, y] = 1;

            if( x == 0 || y == 0 || y == height -1 || x == width -1 ) {
              decors[x, y] = 1;
            }
          }
        }

        // Remove decors
        // top, right, bottom, left
        for(int n=1; n<=4; n++) {
          if( neighboors.ContainsKey(n) ) {
            Vector2 position = new Vector2(0, 0);
            if( n == 1 ) {
              position.x = Random.Range(1, width-2);
              position.y = height-1;
            } else if ( n == 2 ) {
              position.x = width - 1;
              position.y = Random.Range(1, height-2);
            } else if( n == 3 ) {
              position.x = Random.Range(1, width-2);
              position.y = 0;
            } else if( n == 4 ) {
              position.x = 0;
              position.y = Random.Range(1, height-2);
            }
            doors[ (int)position.x, (int)position.y ] = n;
            doorPositions.Add( position );
            // Debug.Log("New Door at " + (int)position.x + "x" + (int)position.y);
            decors[ (int)position.x, (int)position.y ] = 0;
          }
        }

        switch( levelType ) {
          case "spreadable":
            RandomFillMap();
            for(int i=0; i<1; i++) {
              SmoothMap();
            }
            break;
        }


        // decors[2,2] = 1;
        // decors[2,3] = 1;
        // decors[3,2] = 1;
        // decors[3,3] = 1;
        // decors[4,2] = 1;
        // decors[4,3] = 1;
        //
        // decors[3,4] = 1;

        // Find all empty tiles
        // --------------------------------------------------------------------
        int emptyTiles = 0;
        for(int y=0; y<height; y++) {
          for(int x=0; x<width; x++) {
            if( decors[x, y] == 0 ) {
              emptyTiles++;
            }
          }
        }

        // Match the minimumEmptyTiles/maximum retries or regenerate
        // --------------------------------------------------------------------
        if( emptyTiles > minimumEmptyTiles || nbrRegenerations <= 0 ) {
          generated = true;
        } else {
          nbrRegenerations--;
          if( nbrRegenerations <= 1 ) {
            levelType = "empty";            // Force an empty room
          }
        }
      }

      // Find a place for all entities
      // ----------------------------------------------------------------------
      for(int i=0; i<entities.Count; i++) {
        Vector2 p = GetRandomPosition();
        entities[i].x = (int)p.x;
        entities[i].y = (int)p.y;
      }

      if( doorPositions.Count > 1 ) {
        PathFinding pf = new PathFinding(decors, width, height);
        for(int d=0; d<doorPositions.Count-1; d++) {
          List<Vector2> paths = pf.Find(doorPositions[d], doorPositions[d+1]);
          if( paths.Count > 2 ) {
            for(int i=0; i<paths.Count; i++) {
              floors[ (int)paths[i].x, (int)paths[i].y ] = 2;
            }
          }
        }
      }
    }
  }

  public void Add(int direction, Vector2 index) {
    neighboors.Add(direction, index);
  }

  public bool Contains(int direction) {
    return neighboors.ContainsKey(direction);
  }

  public Vector2 Get(int direction) {
    return neighboors[direction];
  }


  public bool IsEmpty(int x, int y) {
    if( decors[x, y] == 1 ) {
      return false;
    }
    if( doors[x, y] > 0 ) {
      return false;
    }
    for(int i=0; i<entities.Count; i++) {
      if( entities[i].x == x && entities[i].y == y ) {
        return false;
      }
    }
    return true;
  }


  public Vector2 GetRandomPosition() {
    List<Vector2> tiles = new List<Vector2>();
    for(int y=0; y<height; y++) {
      for(int x=0; x<width; x++) {
        if( IsEmpty(x, y) ) {
          tiles.Add( new Vector2(x, y) );
        }
      }
    }
    return tiles[ Random.Range(0, tiles.Count) ];
  }


  private void RandomFillMap() {
    for(int y=0; y<height; y++) {
      for(int x=0; x<width; x++) {
        if( x > 0 && y > 0 && x < width - 1 && y < height - 1 ) {
          decors[x, y] = Random.Range(0, 100) < randomFillPercent ? 1 : 0;
        }
      }
    }
  }


  private void SmoothMap() {
    for(int y=1; y<height-1; y++) {
      for(int x=1; x<width-1; x++) {

          int tilesCount = GetSurroundingWallCount(x, y);

          if( tilesCount > 4 ) {
            decors[x, y] = 1;
          } else if( tilesCount < 4 ) {
            decors[x, y] = 0;
          }
        }

    }
  }


  private int GetSurroundingWallCount(int x, int y) {
    int count = 0;

    for(int y2=y-1; y2<=y+1; y2++) {
      for(int x2=x-1; x2<=x+1; x2++) {
        if( x2 >= 0 && y2 >= 0 && x2 < width && y2 < height ) {
          if( x2 != x || y2 != y ) {
            if( doors[x2, y2] > 0 ) {
              count -= 10;
            } else {
              count += decors[x2, y2];
            }
          }
        } else {
            count ++;
        }
      }
    }

    return count;
  }
}
