using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [SerializeField]
    public List<Item> ItemBase = new List<Item>();
    public Item Druck;
    public Item Potion;
    public Item Axe;
    public Item Head;
    private void Awake()
    {
        ItemBase.Add(Druck);
        ItemBase.Add(Potion);
        ItemBase.Add(Axe);
        if(Head != null)   
            ItemBase.Add(Head);
    }
}

