using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chest : Entity {

  public bool isOpen;
  public bool isLocked;

	public Item item;

  public Chest(int _x, int _y, bool locked = false) : base(Entity.Type.Chest, _x, _y, 1)  {
    isOpen = false;
    isLocked = locked;
  }

  public bool IsOpen() {
    return isOpen;
  }

  
}
