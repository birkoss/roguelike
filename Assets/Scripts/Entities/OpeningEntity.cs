using UnityEngine;
using System.Collections;

public class OpeningEntity : MonoBehaviour {

  public Sprite openSprite;

  private Entity entity;

  public void SetEntity(Entity newEntity) {
    entity = newEntity;
  }
  public Entity GetEntity() {
    return entity;
  }

  public void Open() {
    (entity as Chest).isOpen = true;
    CheckStatus();
  }

  public void CheckStatus() {
    if( IsOpen() ) {
      GetComponent<SpriteRenderer>().sprite = openSprite;
    }
  }

  public bool IsOpen() {
    return (entity as Chest).IsOpen();
  }
}
