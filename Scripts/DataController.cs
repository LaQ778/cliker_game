using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class DataController : MonoBehaviour
{
    private static DataController instance;

    public static DataController Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<DataController>();
                if (instance == null) {
                    GameObject container = new GameObject("DataController");
                    instance = container.AddComponent<DataController>();
                }
            }
            return instance;
        }
    }

    private PoketmonButton[] poketmonButtons;

    DateTime GetLastPlayDate() {
        if (!PlayerPrefs.HasKey("Time")) {
            return DateTime.Now;
        }

        String timeBinaryInString = PlayerPrefs.GetString("Time");
        long timeBinaryInLong = Convert.ToInt64(timeBinaryInString);
        
        return DateTime.FromBinary(timeBinaryInLong);
    }

    void onApplicationQuit() {
        UpdateLastPlayDate();
    }

    void UpdateLastPlayDate() {
        PlayerPrefs.SetString("Time", DateTime.Now.ToBinary().ToString());
    }

    public long gold {
        get {
            if (!PlayerPrefs.HasKey("Gold")) {
                return 0;
            }
            string tempGold = PlayerPrefs.GetString("Gold");
            return long.Parse(tempGold);
        }

        set {
            PlayerPrefs.SetString("Gold", value.ToString());
        }
    }

    public long goldPerClick {
        get {
            if (!PlayerPrefs.HasKey("GoldPerClick")) {
                return 1;
            }
            string tempGoldPerClick = PlayerPrefs.GetString("GoldPerClick");
            return long.Parse(tempGoldPerClick);
        }

        set {
            PlayerPrefs.SetString("GoldPerClick", value.ToString());
        }
    }

    public int timeAfterLastPlay {
        get {
            DateTime currentTime = DateTime.Now;
            DateTime lastPlayDate = GetLastPlayDate();

            return (int)currentTime.Subtract(lastPlayDate).TotalSeconds;
        }
    }

    void Awake() {
        // PlayerPrefs.DeleteAll();
        poketmonButtons = FindObjectsOfType<PoketmonButton>();
    }

    void Start() {
        gold += GetGoldPerSec() * timeAfterLastPlay;
        InvokeRepeating("UpdateLastPlayDate", 0f, 5f);
    }

    public void LoadUpgradeButton(UpgradeButton upgradeButton) {
        string key = upgradeButton.upgradeName;
        
        upgradeButton.level = PlayerPrefs.GetInt(key + "_level", 1);
        upgradeButton.upgradeGold = PlayerPrefs.GetInt(key + "_upgradeGold", upgradeButton.startUpgradeGold);
        upgradeButton.currentCost = PlayerPrefs.GetInt(key + "_cost", upgradeButton.startCost);
    }

    public void SaveUpgradeButton(UpgradeButton upgradeButton) {
        string key = upgradeButton.upgradeName;
        
        PlayerPrefs.SetInt(key + "_level", upgradeButton.level);
        PlayerPrefs.SetInt(key + "_upgradeGold", upgradeButton.upgradeGold);
        PlayerPrefs.SetInt(key + "_cost", upgradeButton.currentCost);
    }

    public void LoadPoketmonButton(PoketmonButton poketmonButton) {
        string key = poketmonButton.poketmonName;
        
        poketmonButton.level = PlayerPrefs.GetInt(key + "_level");
        poketmonButton.goldPerSec = PlayerPrefs.GetInt(key + "_goldPerSec");
        poketmonButton.currentCost = PlayerPrefs.GetInt(key + "_cost", poketmonButton.startCost);
        
        if (PlayerPrefs.GetInt(key + "_isPurchased") == 1) {
            poketmonButton.isPurchased = true;
        } else {
            poketmonButton.isPurchased = false;
        }
    }

    public void SavePoketmonButton(PoketmonButton poketmonButton) {
        string key = poketmonButton.poketmonName;
        
        PlayerPrefs.SetInt(key + "_level", poketmonButton.level);
        PlayerPrefs.SetInt(key + "_goldPerSec", poketmonButton.goldPerSec);
        PlayerPrefs.SetInt(key + "_cost", poketmonButton.currentCost);

        if (poketmonButton.isPurchased) {
            PlayerPrefs.SetInt(key + "_isPurchased", 1);
        } else {
            PlayerPrefs.SetInt(key + "_isPurchased", 0);
        }
    }

    public int GetGoldPerSec() {
        int goldPerSec = 0;

        for (int i = 0; i < poketmonButtons.Length; i++) {
            if (poketmonButtons[i].isPurchased == true) {
                goldPerSec += poketmonButtons[i].goldPerSec;
            }
        }
        return goldPerSec;
    }
}
