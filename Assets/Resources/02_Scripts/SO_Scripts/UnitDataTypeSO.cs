using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyDataType")]
public class UnitDataTypeSO : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string name;
    [SerializeField] private float hp;
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float atk;
    [SerializeField] private float atkDelay;
    [SerializeField] private float soulLevel;
    private float currentHp;

    public Sprite Sprite => sprite;
    public string Name => name;
    public float Hp => hp;
    public float CurrentHp => currentHp;
    public float Speed => speed;
    public float JumpPower => jumpPower;
    public float Atk => atk;
    public float AtkDelay => atkDelay;
    public float SoulLevel => soulLevel;

}