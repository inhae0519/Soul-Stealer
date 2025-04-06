using UnityEngine;
using UnityEngine.Serialization;

public abstract class Agent : MonoBehaviour
{
    [HideInInspector] public float unitSpeed;
    [HideInInspector] public float unitJumpPower;
    [HideInInspector] public float unitAtk;
    [HideInInspector] public float unitAtkDelay;
    [HideInInspector] public float unitHp;
    [HideInInspector] public float unitCurrentHp;
    
    [SerializeField] public float unitSoulLevel;
    [SerializeField] public string unitName;
    private UnitDataTypeSO dataTypeSo;
    
    protected virtual void Start()
    {
        dataTypeSo = AgentDictionaly.Instance.UnitDataTypeSoDic[unitName];
        SetStat();
        unitCurrentHp = unitHp;
    }
    
    public void SetStat()
    {
        unitSoulLevel += dataTypeSo.SoulLevel;
        unitHp = dataTypeSo.Hp + unitSoulLevel;
        unitAtk = dataTypeSo.Atk + unitSoulLevel;
        unitSpeed = dataTypeSo.Speed;
        unitJumpPower = dataTypeSo.JumpPower;
        unitAtkDelay = dataTypeSo.AtkDelay;
    }
}
