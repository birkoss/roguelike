using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

[XmlRoot("enemy_master")]
public class EnemyMaster {
  [XmlArray("enemies"),XmlArrayItem("enemy")]
  public Enemy[] enemies;

  private static EnemyMaster enemyMaster;

  public static EnemyMaster Instance () {
      if ( enemyMaster == null ) {
          enemyMaster = new EnemyMaster();
      }
      return enemyMaster;
  }

  public void Load(string path) {
    TextAsset textAsset = (TextAsset) Resources.Load(path);

  	var serializer = new XmlSerializer(typeof(EnemyMaster));
  		enemies = (serializer.Deserialize(new StringReader(textAsset.text)) as EnemyMaster).enemies;

  }

  public Enemy GetEnemy(string name = "Main") {
    for(int i=0; i<enemies.Length; i++) {
      if( enemies[i].name == name ) {
        return enemies[i];
      }
    }
    return null;
  }
}
