using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DemoGameplayEvent : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;
    private void OnTriggerEnter(Collider other)
    {
        objectiveText.color = Color.grey;
    }
}
