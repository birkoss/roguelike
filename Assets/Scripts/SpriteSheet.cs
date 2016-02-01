using UnityEngine;
using System.Collections;

public class SpriteSheet : MonoBehaviour {

  public Sprite[] sprites;

  public Sprite Get(int index) {
    return sprites[index];
  }
}
