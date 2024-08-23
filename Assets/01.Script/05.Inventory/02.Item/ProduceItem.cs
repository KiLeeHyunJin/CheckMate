using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ProduceItem//제작 아이템
{
    public Item MakeItem; //제작할 아이템 이미지

    public int Time; //제작 소요 시간

    public Item[] item; //소요될 아이템 종류 배열
    public int[] Count; //소요될 아이템 개수 배열
}
[Serializable]

public class UpgradeItem//제작 아이템
{
    public int Time; //제작 소요 시간

    public Item[] item; //소요될 아이템 종류 배열
    public int[] Count; //소요될 아이템 개수 배열
}

public class Living : MonoBehaviour
{
    public bool IsLoad;
}
