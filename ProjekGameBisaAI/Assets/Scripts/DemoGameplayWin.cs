using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DemoGameplayWin : MonoBehaviour
{
    public static bool isWin = false;
    public TextMeshProUGUI objectiveText;
    private void OnTriggerEnter(Collider other)
    {
        objectiveText.color = Color.grey;
        isWin = true;
    }


}
