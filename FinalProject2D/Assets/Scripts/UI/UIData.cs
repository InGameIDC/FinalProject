using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UI Data", menuName = "UI Data", order = 51)]
public class UIData : ScriptableObject
{
    public static UIData instance;
    [SerializeField] private Color _selectedHeroIconColor = Color.white;
    [SerializeField] private Color _heroIconColor = Color.white;
    [SerializeField] private Color _heroNotAvailableIconColor = Color.gray;

    public UIData()
    {
        instance = this;
    }

    public Color getSelectedHeroIconColor() => _selectedHeroIconColor;
    public Color getHeroIconColor() => _heroIconColor;
    public Color getHeroNotAvailableIconColor() => _heroNotAvailableIconColor;
}
