using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : Entity {

  public string name;

	public int health;
  public int maxHealth;

  public int speed;
  public int range = 1;
  public int attack;
  public int defense;
  public int experience;
  public int experienceToNextLevel = 12;
  public int level = 1;
  public int gold;
  public int storedLevel = 0;

  public Dictionary<string, Item> equipments;

  public List<Action> actions;

  public Character(int _health = 0, int _sprite = 0, int _speed = 0, int _x = 0, int _y = 0, Entity.Type _type = Entity.Type.Player) : base(_type, _x, _y, _sprite) {
    equipments = new Dictionary<string, Item>();

    health = _health;
    speed = _speed;

    actions = new List<Action>();
  }

  public Character Clone() {
    Character c = new Character();
    c.health = health;
    c.sprite = sprite;
    c.x = x;
    c.y = y;
    c.speed = speed;
    c.type = type;
    c.range = range;
    c.actions = actions;
    c.attack = attack;
    c.defense = defense;

    foreach(KeyValuePair<string,Item> KV in equipments) {
      c.equipments.Add(KV.Key, KV.Value);
    }

    return c;
  }

  public void Equip(string position, Item item) {
    equipments[position] = item;
  }

  public bool HasEquipment(string position) {
    return (equipments.ContainsKey(position));
  }

  public bool IsAlive() {
    return (health > 0);
  }

  public int GetStat(string stat, bool getBaseOnly = false) {
    int value = 0;

    switch( stat ) {
      case "attack":
        value += attack;
        break;
      case "defense":
        value += defense;
        break;
      case "range":
        value += range;
        break;
      case "maxHealth":
        value += maxHealth;
        break;
      case "health":
        value += health;
        break;
    }

    if( !getBaseOnly ) {
      // for(Item item in equipments.Values) {
      foreach(KeyValuePair<string,Item> KV in equipments) {
        // value += item.GetStat(stat);
        value += KV.Value.GetStat(stat);
      }
    }

    return value;
  }
}
