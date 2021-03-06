using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttackingCharacter: MovingCharacter {

  public float attackTime = 0.1f;
  private float inverseAttackTime;                                

  private BoxCollider2D boxCollider;
  private Rigidbody2D rb2D;

  void Awake() {
    boxCollider = GetComponent<BoxCollider2D>();
    rb2D = GetComponent<Rigidbody2D>();

    inverseAttackTime = 1 / attackTime;
  }

  public bool Attack(GameObject target) {
    // Can only attack living object
    // ------------------------------------------------------------------------
    if( target.GetComponent<LivingCharacter>() == null ) {
      return false;
    }
    // Face the target
    // ------------------------------------------------------------------------
    if( (target.transform.position.x != transform.position.x) ) {
      transform.localScale = new Vector3( (target.transform.position.x > transform.position.x ? -1 : 1) , 1, 1);
    }

    StartCoroutine(SmoothAttack(target));

    return true;
  }

  private IEnumerator SmoothAttack(GameObject target) {

    // Disable collider to prevent OnTriggerEnter2D to trigger while attacking
    // TODO: Needed anymore ?
    // ------------------------------------------------------------------------
    boxCollider.enabled = false;

    GetComponent<SpriteRenderer>().sortingOrder = 2;

    Vector3 endPosition = target.transform.position;
    Vector3 startPosition = transform.position;

    float sqrRemainingDistance = (transform.position - endPosition).sqrMagnitude;

    while(sqrRemainingDistance > float.Epsilon) {
      Vector3 newPosition = Vector3.MoveTowards(rb2D.position, endPosition, inverseAttackTime * Time.deltaTime);
      rb2D.MovePosition(newPosition);
      sqrRemainingDistance = (transform.position - endPosition).sqrMagnitude;
      yield return null;
    }

    // atk + ( level / 10 * atk ) * (80~120) / 200 * ((store+100) / 100) - (def * (60~80) / 200)
    float attack_value = (character.GetStat("attack") + (character.level / 10 * character.GetStat("attack"))) * Random.Range(80, 120) / 200 * ( (character.storedLevel+100) / 100 );
    float defense_value = (target.GetComponent<LivingCharacter>().GetCharacter().GetStat("defense") * Random.Range(60, 80) / 200);
    int damage = (int)Mathf.Round(attack_value - defense_value);
    if( damage <= 0 ) {
      damage = 1;
    }

    target.GetComponent<LivingCharacter>().TakeDamage( damage );
    // GameEngine.instance.ShowMessage(attack.ToString(), (int)target.transform.position.x, (int)target.transform.position.y);

    GameObject go = (Instantiate(GameEngine.instance.mapEngine.tilesAnimations[0], endPosition, Quaternion.identity) as GameObject);

    Vector3 v = Camera.main.WorldToScreenPoint(endPosition);
    GameObject go2 = (Instantiate(GameEngine.instance.mapEngine.tilesAnimations[1], v, Quaternion.identity) as GameObject);
    go2.transform.SetParent( GameObject.Find("Canvas").transform );
    go2.GetComponent<Text>().text = damage.ToString();
//
    sqrRemainingDistance = (endPosition - startPosition).sqrMagnitude;

    while(sqrRemainingDistance > float.Epsilon) {
      Vector3 newPosition = Vector3.MoveTowards(rb2D.position, startPosition, inverseAttackTime * Time.deltaTime);
      rb2D.MovePosition(newPosition);
      sqrRemainingDistance = (transform.position - startPosition).sqrMagnitude;
      yield return null;
    }

    boxCollider.enabled = true;

    GetComponent<SpriteRenderer>().sortingOrder = 1;
    GameEngine.instance.NextTurn();
  }
}
