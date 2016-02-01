using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

[XmlRoot("item_master")]
public class ItemMaster {
  [XmlArray("items"),XmlArrayItem("item")]
  public Item[] items;

  private static ItemMaster itemMaster;

  public static ItemMaster Instance () {
      if ( itemMaster == null ) {
          itemMaster = new ItemMaster();
      }
      return itemMaster;
  }

  public void Load(string path) {
    TextAsset textAsset = (TextAsset) Resources.Load(path);

  	var serializer = new XmlSerializer(typeof(ItemMaster));
  		items = (serializer.Deserialize(new StringReader(textAsset.text)) as ItemMaster).items;

  }

  public Item Get(string name = "Main") {
    for(int i=0; i<items.Length; i++) {
      if( items[i].name == name ) {
        return items[i];
      }
    }
    return null;
  }
}
