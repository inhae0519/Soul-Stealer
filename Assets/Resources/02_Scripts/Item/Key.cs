using UnityEngine;

public class Key : Item
{
    [SerializeField] private AudioClip audioClip;
    public override void ItemEffect(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if(playerController.haveKey)
            return;
        AudioManager.Instance.PlayEffect(audioClip);
        playerController.haveKey = true;
        UIManager.Instance.KeyImageSetActive(playerController.haveKey);
        gameObject.SetActive(false);
    }
}
