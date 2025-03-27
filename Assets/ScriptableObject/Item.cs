using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 0)]
public class Item : ScriptableObject
{
    public Sprite itemSprite;
    public Type itemType;
    public int itemUseCount = 1;

}
