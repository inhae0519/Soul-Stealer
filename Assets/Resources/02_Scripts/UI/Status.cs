using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    private RectTransform imageTrm;
    private Image currentImage;
    private Image keyImage;
    private Image currentHealthBar;
    private Image currentMentalBar;
    private Text currentNameText;
    private Text currentLevelText;
    private void Awake()
    {
        currentImage = transform.Find("Image").GetComponent<Image>();
        keyImage = transform.Find("KeyImage").GetComponent<Image>();
        imageTrm = currentImage.GetComponent<RectTransform>();
        currentHealthBar = transform.Find("Bars").Find("HealthBar").GetComponent<Image>();
        currentMentalBar = transform.Find("Bars").Find("MentalBar").GetComponent<Image>();
        currentNameText = transform.Find("Name").GetComponent<Text>();
        currentLevelText = transform.Find("Level").Find("Text").GetComponent<Text>();
    }
    
    public void UpdateStatus(UnitDataTypeSO unitDataTypeSo, Controlble currentUnit)
    {
        Debug.Log("Update");
        if (unitDataTypeSo.Name == "Player")
        {
            imageTrm.localScale = Vector3.one;
            imageTrm.anchoredPosition = new Vector3(-250f, 6,0);
        }
        else if (unitDataTypeSo.Name == "Goblin")
        {
            Debug.Log("goblin");
            imageTrm.localScale = new Vector3(3, 3, 1);
            imageTrm.anchoredPosition = new Vector3(-250f, 30,0);
        }
        else if (unitDataTypeSo.Name == "Mushroom")
        {
            imageTrm.localScale = new Vector3(3, 3, 1);
            imageTrm.anchoredPosition = new Vector3(-250f, 30,0);
        }
        else if (unitDataTypeSo.Name == "Skeleton")
        {
            imageTrm.localScale = new Vector3(3, 3, 1);
            imageTrm.anchoredPosition = new Vector3(-250f, 0,0);
        }

        currentImage.sprite = unitDataTypeSo.Sprite;
        currentNameText.text = unitDataTypeSo.Name;
        currentHealthBar.fillAmount = currentUnit.unitCurrentHp/currentUnit.unitHp;
        currentLevelText.text = currentUnit.unitSoulLevel.ToString();
    }

    public void UpdateKeyImage(bool haveKey)
    {
        keyImage.enabled = haveKey;
    }
}
