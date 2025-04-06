using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    private Image currentHealthBar;
    private Text currentLevelText;
    private Camera mainCam;
    
    private void Awake()
    {
        mainCam = Camera.main;
        currentHealthBar = transform.Find("HealthBar").GetComponent<Image>();
        currentLevelText = transform.Find("Level").Find("Text").GetComponent<Text>();
    }

    public void UpdateEnemyStatus(EnemyController currentUnit)
    {
        currentHealthBar.fillAmount = currentUnit.unitCurrentHp/currentUnit.unitHp;
        currentLevelText.text = currentUnit.unitSoulLevel.ToString();
    }
    
    public void EnemyStatusFollow(Vector3 pos, float paddingY)
    {
        transform.position = mainCam.WorldToScreenPoint(new Vector3(pos.x + -1, pos.y+paddingY, 0));
    }
}
