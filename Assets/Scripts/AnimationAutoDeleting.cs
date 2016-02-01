using UnityEngine;
using System.Collections;

public class AnimationAutoDeleting : MonoBehaviour {

  private Animator animator;

  void Awake() {
    animator = GetComponent<Animator>();
  }

	void FixedUpdate () {
    if( animator.GetNextAnimatorStateInfo(0).IsName("End") ) {
      Destroy(gameObject);
    }
	}
}
