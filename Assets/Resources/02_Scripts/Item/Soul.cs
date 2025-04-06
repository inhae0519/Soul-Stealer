using UnityEngine;

public class Soul : Item
{
    [SerializeField] private AudioClip audioClip;
    public float soulIndex;
    public override void ItemEffect(GameObject player)
    {
        AudioManager.Instance.PlayEffect(audioClip);
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.unitSoulLevel += soulIndex;
        playerController.unitAtk += (soulIndex/2);
        playerController.unitHp += soulIndex;    
        playerController.unitCurrentHp += soulIndex;    
        UIManager.Instance.StartUpdateStatus(AgentDictionaly.Instance.UnitDataTypeSoDic["Player"], playerController);
        Destroy(gameObject);
    }
}
