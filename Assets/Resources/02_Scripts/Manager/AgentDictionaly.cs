using System.Collections.Generic;
using UnityEngine;

public class AgentDictionaly : MonoSingleton<AgentDictionaly>
{
    [SerializeField] private UnitDataTypeSO[] unitDataTypeSOs;
    
    public Dictionary<string, UnitDataTypeSO> UnitDataTypeSoDic = new();
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        foreach (var unitDataTypeSo in unitDataTypeSOs)
        {
            UnitDataTypeSoDic.Add(unitDataTypeSo.Name, unitDataTypeSo);
        }
    }
}