using UnityEngine;

public abstract class ItemScriptable : ScriptableObject
{
    public ItemData ItemData;

    public abstract void Use();
}
