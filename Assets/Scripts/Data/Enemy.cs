using System.Xml;
using System.Xml.Serialization;

public class Enemy {
  [XmlAttribute("name")]
  public string name;

  public int health;
  public int speed;
  public int attack;
  public int defense;
  public int range;
  public int prefab;

  [XmlArray("actions"), XmlArrayItem("action")]
  public string[] actions;
}
