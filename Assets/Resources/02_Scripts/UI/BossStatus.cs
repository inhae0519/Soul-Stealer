using UnityEngine;
using UnityEngine.UI;

public class BossStatus : MonoBehaviour
{
    private Image currentHealthBar;
    
    private void Awake()
    {
        currentHealthBar = transform.Find("Bar").Find("HealthBar").GetComponent<Image>();
    }

    public void UpdateBossStatus(BossController currentUnit)
    {
        currentHealthBar.fillAmount = currentUnit.unitCurrentHp/currentUnit.unitHp;
    }
}
