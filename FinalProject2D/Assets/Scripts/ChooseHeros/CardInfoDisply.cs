using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoDisply : MonoBehaviour
{
    public GameObject rankText;
    public GameObject damageText;
    public GameObject healthText;
    public GameObject speedText;

    public GameObject xpToUpgradeText;
    public GameObject upgradeCostText;

    public GameObject profileImage;


    public void updateInfoPanel(int rank, float damage, float health, float speed, int xp, int cost, Sprite sprite)
    {
        rankText.GetComponent<TMPro.TextMeshProUGUI>().text = rank.ToString();
        damageText.GetComponent<TMPro.TextMeshProUGUI>().text = damage.ToString();
        healthText.GetComponent<TMPro.TextMeshProUGUI>().text = health.ToString();
        speedText.GetComponent<TMPro.TextMeshProUGUI>().text = speed.ToString();
        xpToUpgradeText.GetComponent<TMPro.TextMeshProUGUI>().text = xp.ToString();
        upgradeCostText.GetComponent<TMPro.TextMeshProUGUI>().text = cost.ToString();
        profileImage.GetComponent<Image>().sprite = sprite;

    }

}
