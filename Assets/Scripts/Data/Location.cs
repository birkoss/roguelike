using System.Xml;
using System.Xml.Serialization;

public class Location {
  [XmlAttribute("name")]
  public string name;

  [XmlAttribute("depth")]
  public int depth;

  [XmlAttribute("rarity")]
  public int rarity;
}
