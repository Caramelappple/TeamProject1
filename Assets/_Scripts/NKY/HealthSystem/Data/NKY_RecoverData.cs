using UnityEngine;

public readonly struct NKY_RecoverData
{
    public readonly NKY_Health giver;
    public readonly int recoverValue;

    public  NKY_RecoverData(NKY_Health giver, int recoverValue)
    {
        this.giver = giver;
        this.recoverValue = recoverValue;
    }

    public static NKY_RecoverData Create(NKY_Health giver, int recoverValue)
    {
        NKY_RecoverData result = new NKY_RecoverData(giver, recoverValue);
        return result;
    }
}

public readonly struct NKY_RecoverResultData
{
    public readonly NKY_Health giver;
    public readonly int recoverValue;
    public readonly int resourceData;

    public NKY_RecoverResultData(NKY_Health giver, int recoverValue, int resourceData)
    {
        this.giver = giver;
        this.recoverValue = recoverValue;
        this.resourceData = resourceData;
    }

    public static NKY_RecoverResultData Create(NKY_Health giver, int recoverValue, int resourceData)
    {
        NKY_RecoverResultData result = new NKY_RecoverResultData(giver, recoverValue, resourceData);
        return result;
    }
}

