using Assets._Scripts.NKY;
using UnityEngine;

public readonly struct NKY_RecoverData
{
    public readonly Entity giver;
    public readonly int recoverValue;

    public  NKY_RecoverData(Entity giver, int recoverValue)
    {
        this.giver = giver;
        this.recoverValue = recoverValue;
    }

    public static NKY_RecoverData Create(Entity giver, int recoverValue)
    {
        NKY_RecoverData result = new NKY_RecoverData(giver, recoverValue);
        return result;
    }
}

public readonly struct NKY_RecoverResultData
{
    public readonly Entity giver;
    public readonly int recoverValue;
    public readonly int resourceData;

    public NKY_RecoverResultData(Entity giver, int recoverValue, int resourceData)
    {
        this.giver = giver;
        this.recoverValue = recoverValue;
        this.resourceData = resourceData;
    }

    public static NKY_RecoverResultData Create(Entity giver, int recoverValue, int resourceData)
    {
        NKY_RecoverResultData result = new NKY_RecoverResultData(giver, recoverValue, resourceData);
        return result;
    }
}

