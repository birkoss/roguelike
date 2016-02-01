using System.Xml;
using System.Xml.Serialization;

public class TableEntry {
  [XmlAttribute("item")]
  public string item;

  [XmlAttribute("table")]
  public string table;

  [XmlAttribute("weight")]
  public int weight;
}
