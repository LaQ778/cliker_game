using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButton : MonoBehaviour
{

    public void onClick() {
        long goldPerClick = DataController.Instance.goldPerClick;
        DataController.Instance.gold += goldPerClick;
    }
}
