using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

[XmlRoot("loot_table")]
public class TableMaster {
  [XmlArray("tables"),XmlArrayItem("table")]
  public Table[] tables;

  private static TableMaster tableMaster;

  public static TableMaster Instance () {
      if ( tableMaster == null ) {
          tableMaster = new TableMaster();
      }
      return tableMaster;
  }

  public void Load(string path) {
    TextAsset textAsset = (TextAsset) Resources.Load(path);

  	var serializer = new XmlSerializer(typeof(TableMaster));
  		tables = (serializer.Deserialize(new StringReader(textAsset.text)) as TableMaster).tables;

  }

  public Table GetTable(string name = "Main") {
    for(int i=0; i<tables.Length; i++) {
      if( tables[i].name == name ) {
        return tables[i];
      }
    }
    return null;
  }
}
