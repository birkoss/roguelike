using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class Table {
  [XmlAttribute("name")]
  public string name;

  [XmlArray("table_items"), XmlArrayItem("table_entry")]
  public TableEntry[] items;

  public string Generate() {
    List<string> loots = new List<string>();
    for(int i=0; i<items.Length; i++) {
      if( items[i].table != null ) {
        if( TableMaster.Instance().GetTable( items[i].table ) != null ) {
          AddItem(loots, TableMaster.Instance().GetTable( items[i].table ).Generate(), items[i].weight);
        }
      } else {
        AddItem(loots, items[i].item, items[i].weight);
      }
    }
    return loots[ Random.Range(0, loots.Count-1) ];
  }

  public void AddItem(List<string> list, string item, int quantity) {
    for(int i=0; i<quantity; i++) {
      list.Add( item );
    }
  }
}
