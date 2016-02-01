using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PopupStat : MonoBehaviour {

  public Text textAttack;
  public Text textAttackMod;

  public Text textDefense;
  public Text textDefenseMod;

  public Text textSpeed;
  public Text textSpeedMod;

  public Text textRange;
  public Text textRangeMod;

  public void SetAttack(int attack) {
    textAttack.text = attack.ToString();
  }

  public void SetDefense(int defense) {
    textDefense.text = defense.ToString();
  }

  public void SetSpeed(int speed) {
    textSpeed.text = speed.ToString();
  }

  public void SetRange(int range) {
    textRange.text = range.ToString();
  }

  public void SetAttackMod(int mod) {
    ChangeText(textAttackMod, mod);
  }

  public void SetDefenseMod(int mod) {
    ChangeText(textDefenseMod, mod);
  }

  public void SetSpeedMod(int mod) {
    ChangeText(textSpeedMod, mod);
  }

  public void SetRangeMod(int mod) {
    ChangeText(textRangeMod, mod);
  }

  public void SetStats(int attack, int attackMod, int defense, int defenseMod, int speed, int speedMod, int range, int rangeMod) {
    SetAttack(attack);
    SetDefense(defense);
    SetSpeed(speed);
    SetRange(range);

    SetAttackMod(attackMod);
    SetDefenseMod(defenseMod);
    SetSpeedMod(speedMod);
    SetRangeMod(rangeMod);
  }

  private void ChangeText(Text textBox, int mod) {
    string text = (mod == 0 ? "" : mod.ToString());

    Color color = Color.white;

    if( mod > 0 ) {
      text = "+" + text;
      color = Color.green;
    } else if( mod < 0 ) {
      color = Color.red;
    }

    textBox.color = color;
    textBox.text = text;
  }

}
