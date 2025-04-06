using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{    
    private RectTransform imageTrm;
    private Image currentImage;
    private Image currentHealthBar;
    private Image currentMentalBar;
    private Text currentNameText;
    private Text currentLevelText;
    private void Awake()
    {
        currentImage = transform.Find("Image").GetComponent<Image>();
        imageTrm = currentImage.GetComponent<RectTransform>();
        currentHealthBar = transform.Find("Bars").Find("HealthBar").GetComponent<Image>();
        currentMentalBar = transform.Find("Bars").Find("MentalBar").GetComponent<Image>();
        currentNameText = transform.Find("Name").GetComponent<Text>();
        currentLevelText = transform.Find("Level").Find("Text").GetComponent<Text>();
    }
    
    public void UpdateStatus(UnitDataTypeSO unitDataTypeSo, PlayerController currentUnit)
    {
        Debug.Log("PlayerUpdate");
        currentHealthBar.fillAmount = currentUnit.unitCurrentHp/unitDataTypeSo.Hp;
        currentLevelText.text = unitDataTypeSo.SoulLevel.ToString();
    }
}
