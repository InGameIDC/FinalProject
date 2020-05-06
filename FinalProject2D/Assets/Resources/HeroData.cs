using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

public class HeroData
{
    [XmlAttribute("name")]
    public string heroName;

    [XmlElement("Id")]
    public int heroId;

    [XmlElement("FamilyId")]
    public int familyId;

    [XmlElement("Damage")]
    public float damage;

    [XmlElement("Health")]
    public float health;
}
