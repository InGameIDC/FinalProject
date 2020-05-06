using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using System.IO;


[XmlRoot("HerosCollection")]
public class HeroContainer 
{
    [XmlArray("Heros")]
    [XmlArrayItem("hero")]
    public List<HeroData> heroesData = new List<HeroData>();

    public static HeroContainer Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);

        XmlSerializer serializer = new XmlSerializer(typeof(HeroContainer));

        StringReader reader = new StringReader(_xml.text);

        HeroContainer heroesData = serializer.Deserialize(reader) as HeroContainer;

        reader.Close();

        return heroesData;
    }
}
