using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ProduceItem//���� ������
{
    public Item MakeItem; //������ ������ �̹���

    public int Time; //���� �ҿ� �ð�

    public Item[] item; //�ҿ�� ������ ���� �迭
    public int[] Count; //�ҿ�� ������ ���� �迭
}
[Serializable]

public class UpgradeItem//���� ������
{
    public int Time; //���� �ҿ� �ð�

    public Item[] item; //�ҿ�� ������ ���� �迭
    public int[] Count; //�ҿ�� ������ ���� �迭
}

public class Living : MonoBehaviour
{
    public bool IsLoad;
}
