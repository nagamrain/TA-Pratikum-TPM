using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreScript : MonoBehaviour
{
    [SerializeField] TMP_Text rankGameObject;
    [SerializeField] TMP_Text scoreNameGameObject;
    [SerializeField] TMP_Text scoreGameObject;
    
    public void SetScore(string rank, string name, string score)
    {
        this.rankGameObject.GetComponent<TextMeshProUGUI>().text = rank;
        this.scoreNameGameObject.GetComponent<TextMeshProUGUI>().text = name;
        this.scoreGameObject.GetComponent<TextMeshProUGUI>().text = score;
    }
}
