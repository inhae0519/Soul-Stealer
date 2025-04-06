using UnityEngine;

public class TutorialBgm : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    private void Awake()
    {
        AudioManager.Instance.PlayBGM(audioClip);
    }
}
