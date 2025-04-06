using System;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private float backSoulLevel;
    [SerializeField] private float backHp;
    [SerializeField] private float backAtk;
    [SerializeField] private float backCurrentHp;
    public float defaultLevel;
    public float defaultHp;
    public float defaultCurrentHp;
    public float defaultAtk;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        defaultLevel = backSoulLevel;
        defaultHp = backHp;
        defaultCurrentHp = backCurrentHp;
        defaultAtk = backAtk;
    }

    public void GetBackData(float soulLevel, float currentHp, float hp, float atk)
    {
        backSoulLevel = soulLevel;
        backHp = hp;
        backAtk = atk;
        backCurrentHp = currentHp;
    }

    public float GetHpStat()
    {
        return backHp;
    }
    public float GetCurrentHpStat()
    {
        return backCurrentHp;
    }
    public float GetAtkStat()
    {
        return backAtk;
    }
    public float GetLevelStat()
    {
        return backSoulLevel;
    }

    public void StatDefault()
    {
        backSoulLevel = defaultLevel;
        backHp = defaultHp;
        backCurrentHp = defaultCurrentHp;
        backAtk = defaultAtk;
    }
}
