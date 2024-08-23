using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[Serializable]
public class ObjectGroup
{
    /*[HideInInspector] */public List<objectPosition> objectPosition = new List<objectPosition>();
    [HideInInspector] public GameObject prefab;
    /*[HideInInspector]*/ public int CopyCount; // ���� ���纻 �ε���
    /*[HideInInspector]*/ public int CurrentCount; // ���� ��ġ�� ����
    public int FrontCount;
    [HideInInspector] public int TotalCount; //��ü���� �ִ� ����
}
public class TileObject
{
    [HideInInspector] public GameObject[] copy;

    public ObjectGroup objData = new ObjectGroup();
}
public class objectPosition
{
    public Vector3 localScale;
    public Vector3 position;
    public Quaternion rotation;
    public bool isActive = false;
}

[Serializable]
public class JellyObject
{
    [HideInInspector] public BaseJelly[] copy;

    public ObjectGroup objData = new ObjectGroup();
}

public class ObstacleObject
{
    [HideInInspector] public GameObject[] copy;

    public ObjectGroup objData = new ObjectGroup();
}