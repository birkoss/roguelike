using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding {

  private int[,] map;

  private int width;
  private int height;
  private int emptyValue;

  private bool includeStartPosition = true;

  public PathFinding(int[,] _map, int _width, int _height, int _emptyValue = 0) {
    width = _width;
    height = _height;
    emptyValue = _emptyValue;

    map = new int[width, height];
    System.Array.Copy(_map, map, width*height);
  }

  public List<Vector2> Find(Vector2 start, Vector2 end) {
    return FindPath(map, width, height, start, end);
  }

  // Find the quickest path from (start) to (end) depending on the (map)
  // --------------------------------------------------------------------------
  private List<Vector2> FindPath(int[,] map, int width, int height, Vector2 start, Vector2 end) {
    Dictionary<Vector2, Vector2>completePaths = new Dictionary<Vector2, Vector2>();

    List<Vector2> queue = new List<Vector2>();
    queue.Add( start );
    completePaths.Add( start, start);

    // Queue all adjacent tiles not already visited
    // ------------------------------------------------------------------------
    while( queue.Count > 0 ) {
      Vector2 current = queue[0];
      queue.RemoveAt(0);

      List<Vector2> neighboors = FindNeighboors(current.x, current.y, width, height);
      for(int i=0; i<neighboors.Count; i++) {
        if( !completePaths.ContainsKey( neighboors[i] ) ) {
          if( map[ (int)neighboors[i].x, (int)neighboors[i].y ] == emptyValue ) {
            queue.Add( neighboors[i] );
            completePaths.Add(neighboors[i], current);      // Save where we are from
          }
        }
      }
    }

    // Get the complete path from (end) to (start)
    // ------------------------------------------------------------------------
    List<Vector2> paths = new List<Vector2>();
    if( completePaths.ContainsKey(end) ) {
      Vector2 current = end;
      paths.Add(end);

      while ( current != start ) {
        current = completePaths[current];
        if( current != start || includeStartPosition ) {
          paths.Add(current);
        }
      }
    }

    paths.Reverse();
    return paths;
  }

  // Find all neighboors from the current position (not out of bounds)
  // --------------------------------------------------------------------------
  private List<Vector2> FindNeighboors(float x, float y, int width, int height) {
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
}
