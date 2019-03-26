using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    private int value;

    public virtual int GetValue()
    {
        return value;
    }

    public virtual void setValue(int newValue)
    {
        value = newValue;
    }
}
