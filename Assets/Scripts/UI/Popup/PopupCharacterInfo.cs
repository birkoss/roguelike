using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PopupCharacterInfo : MonoBehaviour {

  public Image image;

  public Text textName;

  public Slider sliderHP;
  public Text textHP;

  public Slider sliderMP;
  public Text textMP;

  public Text textLevel;
  public Text textLevelToNext;
  public Slider sliderLevel;

  public PopupStat stat;

  private Character character;

  public void SetCharacter(Character newCharacter) {
    character = newCharacter;

    image.sprite = character.gameObject.GetComponent<SpriteRenderer>().sprite;
    textName.text = character.name;

    sliderHP.maxValue = character.GetStat("maxHealth");
    sliderHP.value = character.GetStat("health");
    textHP.text = character.GetStat("health") + "/" + character.GetStat("maxHealth");

    textMP.text = "0/100";
    sliderMP.maxValue = 100;
    sliderMP.value = 0;

    textLevel.text = "Level:" + character.level.ToString();
    textLevelToNext.text = character.experience.ToString() + "/" + character.experienceToNextLevel.ToString();
    sliderLevel.maxValue = character.experienceToNextLevel;
    sliderLevel.value = 0;

    stat.SetStats(character.GetStat("attack"), 0, character.GetStat("defense"), 0, character.GetStat("speed"), 0, character.GetStat("range"), 0);
  }

  public void TryItem(string position, Item item) {
    Character clone = character.Clone();
    clone.Equip(position, item);

    Debug.Log( clone.GetStat("attack") + " - " + character.GetStat("attack") );

    stat.SetAttackMod( clone.GetStat("attack") - character.GetStat("attack") );
  }
}
