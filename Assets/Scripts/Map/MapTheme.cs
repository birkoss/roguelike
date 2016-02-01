using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapTheme : MonoBehaviour {

  public GameObject[] floors;
  public GameObject[] decors;
  public GameObject[] exits;

	public GameObject GetFloor(int tile = 1) {
    return floors[ tile - 1 ];
  }

  public GameObject GetExit() {
    return exits[0];
  }

  public GameObject GetDecor(){//int x, int y, int[,] tiles, int width, int height) {
    return decors[0];
  }

  public GameObject GetDecor(int tileX, int tileY, int[,] tiles, int width, int height) {
    //  Debug.Log("GetDecor: " + tileX + "x" + tileY);
    int value = 1;
    int total = 0;
    for(int y=tileY-1; y<=tileY+1; y++) {
      for(int x=tileX-1; x<=tileX+1; x++) {
        if( (x == tileX && y != tileY) || (x != tileX && y == tileY) ) {
          if( x >=0 && x < width && y >= 0 && y < height ) {
            if( tiles[x, y] > 0 ) {
              total += value;
              // Debug.Log("Yes " + x + "x" + y + " = " + value);
            } else {
              // Debug.Log("No " + x + "x" + y + " = " + value);
            }
          } else {
            //  Debug.Log("Outside " + x + "x" + y + " = " + value);
            total += value;
          }
          value = (value * 2);
        }
      }
    }

    int index = 0;

    switch(total) {
      // case 1:
      //   index = 11;
      //   break;
      // case 3:
      //   index = 14;
      //   break;
      // case 7:
      //   index = 8;
      //   break;
      // case 10:
      //   index = 1;
      //   break;
      case 3:
        index = 3;
        break;
      case 5:
        index = 1;
        break;
      case 7:
        index = 2;
        break;
      case 10:
        index = 9;
        break;
      case 12:
        index = 7;
        break;
      case 14:
        index = 8;
        break;
      case 15:
        index = 5;
        break;

    }

if( index == 11 ) {
  index = 6;
}
if( index == 14 ) {
  index = 5;
}

  //  if( index == 0 ) {
            Debug.Log("Total: " + tileX + "x" + tileY + " = " + total);
      //    }
//     List<Vector2> positions = new List<Vector2>();
//     List<int> values = new List<int>();
//
//     positions.Add( new Vector2(-1, -1) );
//     values.Add( 1 );
//     positions.Add( new Vector2(0, -1) );
//     values.Add( 2 );
//     positions.Add( new Vector2(1, -1) );
//     values.Add( 4);
//     positions.Add( new Vector2(-1, 0) );
//     values.Add( 8);
//     positions.Add( new Vector2(1, 0) );
//     values.Add( 16);
//     positions.Add( new Vector2(-1, 1) );
//     values.Add( 32);
//     positions.Add( new Vector2(0, 1));
//     values.Add(64);
//     positions.Add( new Vector2(1, 1));
//     values.Add(128);
//     int total = 0;
//     int x2, y2;
//     for(int v=0; v<positions.Count; v++) {
//       x2 = x + (int)positions[v].x;
//       y2 = y + (int)positions[v].y;
//
//       if( x2 >=0 && x2 < width && y2 >= 0 && y2 < height ) {
//         total += (tiles[x2, y2] > 0 ? values[v] : 0);
//       }
//     }
// Debug.Log(x + "x" + y + "=" + total);
//     switch(total) {
//       case 11:
//         return decors[1];
//       case 7:
//         return decors[2];
//       case 22:
//         return decors[3];
//       case 255:
//         return decors[5];
//       case 111:
//         return decors[6];
//
//     }

    return decors[index];
  }

  public GameObject GetDecor(int index) {
    return decors[index];
  }
}
