using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoketmonButton : MonoBehaviour
{
    public Color upgradableColor = Color.blue;

    public Color notUpgradableColor = Color.red;

    public Image colorImage;

    public Text poketmonDisplay;

    public CanvasGroup canvasGroup;

    public Slider slider;

    public string poketmonName;

    public int level;

    [HideInInspector] public int currentCost;

    public int startCost = 1;

    [HideInInspector] public int goldPerSec;

    public int startGoldPerSec = 1;

    public float costPow = 3.14f;

    public float upgradePow = 1.07f;

    [HideInInspector] public bool isPurchased = false;

    void Start() {
        DataController.Instance.LoadPoketmonButton(this);
        StartCoroutine("AddGoldLoop");
        UpdateUI();
    }

    public void PurchasePoketmon() {
        if (DataController.Instance.gold >= currentCost) {
            isPurchased = true;
            DataController.Instance.gold -= currentCost;
            level ++;
            UpdatePoketmon();
            UpdateUI();
            DataController.Instance.SavePoketmonButton(this);
        }
    }

    IEnumerator AddGoldLoop() {
        while(true) {
            if(isPurchased) {
                DataController.Instance.gold += goldPerSec;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void UpdatePoketmon() {
        goldPerSec = goldPerSec + startGoldPerSec * (int) Mathf.Pow(upgradePow, level);
        currentCost = startCost * (int) Mathf.Pow(costPow, level);
    }

    public void UpdateUI() {
        poketmonDisplay.text = poketmonName + "\nLevel: " + level +  "\nCost: " + currentCost +  "\nGold Per Sec: " + goldPerSec;
        
        slider.minValue = 0;
        slider.maxValue = currentCost;

        slider.value = DataController.Instance.gold;

        if (isPurchased) {
            canvasGroup.alpha = 1.0f;
        } else {
            canvasGroup.alpha = 0.6f;
        }

        if (currentCost <= DataController.Instance.gold) {
            colorImage.color = upgradableColor;
        } else {
            colorImage.color = notUpgradableColor;
        }
    }

    private void Update() {
        UpdateUI();
    }
}
