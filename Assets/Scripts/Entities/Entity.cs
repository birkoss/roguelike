using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity {

  public enum Type : int {
    Player = 1,
    Enemy = 2,
    Chest = 3,
    Exit = 4
  };

  public int sprite;
  public int x;
  public int y;
  public GameObject gameObject;
  public Type type;  // 1 = player, 2 = enemy, 3 = chest 4 = Exit

  public Entity(Type _type = Entity.Type.Player, int _x = 0, int _y = 0, int _sprite = 0) {
    sprite = _sprite;
    x = _x;
    y = _y;
    type = _type;
  }

}
