using System.Xml;
using System.Xml.Serialization;

public class StatModifier {
  [XmlAttribute("name")]
  public string name;

  [XmlAttribute("value")]
  public int value;
}
