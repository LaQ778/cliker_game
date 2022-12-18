using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public Text upgradeDisplay;
    public string upgradeName;

    [HideInInspector] public int upgradeGold;
    public int startUpgradeGold = 1;

    [HideInInspector] public int currentCost = 1;
    public int startCost = 1;

    [HideInInspector] public int level = 1;

    public float upgradePow = 1.07f;
    public float costPow = 3.14f;

    // start speed < awake speed
    void Start() {
        DataController.Instance.LoadUpgradeButton(this);
        UpdateUI();
    }

    public void PurchaseUpgrade() {
        if (DataController.Instance.gold >= currentCost) {
            level += 1;
            DataController.Instance.gold -= currentCost;
            DataController.Instance.goldPerClick += upgradeGold;

            UpdateUpgrade();
            UpdateUI();
            DataController.Instance.SaveUpgradeButton(this);
        }
    }

    public void UpdateUpgrade() {
        upgradeGold = upgradeGold + startUpgradeGold * (int) Mathf.Pow(upgradePow, level);
        currentCost = startCost * (int) Mathf.Pow(costPow, level);
    }

    public void UpdateUI() {
        upgradeDisplay.text = upgradeName + "\nUpGrade Cost: " + currentCost + "\nLevel: " + level + "\nNext New Gold Per Click: " + upgradeGold;
    }
}
