using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public Sprite sprite;
    public string Nameitem;
    public int point;
}
