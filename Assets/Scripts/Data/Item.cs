using System.Xml;
using System.Xml.Serialization;

public class Item {
  [XmlAttribute("name")]
  public string name;

  public int prefab;

  [XmlArray("modifiers"), XmlArrayItem("stat")]
  public StatModifier[] modifiers;

  public int GetStat(string stat) {
    int value = 0;
    for(int i=0; i<modifiers.Length; i++) {
      if( modifiers[i].name == stat ) {
        value += modifiers[i].value;
      }
    }
    return value;
  }
}
