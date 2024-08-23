using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SensorItem", menuName = "SensorItem/SensorItem", order = -999)]
public class SensorItem : ScriptableObject
{
    public DataType.SensorType sensorType;//센서 능력

    public DataType.SensorLevelType type;//스페이드, 하트 ,다이아, 클로버
    public DataType.PriceType priceType;    //구매 재화
    public DataType.SensorActiveType activeType; //발동 종류

    [TextArea]
    public string sensorInfo;   //센서 설명
    public float skillTime;       //발동시간
    public float percentScore;    //능력치
    public string sensorName; // 센서 이름
    public int sensorLevel; // 센서 레벨

    //이미지
    public Sprite ReadySensorImage;
    public Sprite sensorSprite; // 아이콘
    public Sprite InGameSensorSprite; // 아이콘

    //public int AddFever; 

    //상점 가격
    public int GoldPrice;
    public int DiaPrice; 


    //레벨업 가격
    public int levelUpDiaPrice;
    public int levelUpGoldPrice;


    public int DiaLevel2;
    public int DiaLevel3;
    public int DiaLevel4;
    public int DiaLevel5;
    public int DiaLevel6;
    public int DiaLevel7;

    public int GoldLevel2;
    public int GoldLevel3;
    public int GoldLevel4;
    public int GoldLevel5;
    public int GoldLevel6;
    public int GoldLevel7;

    //레벨별 능력치 상향 값
    public int Level2_SkillTime;
    public int Level3_SkillTime;
    public int Level4_SkillTime;
    public int Level5_SkillTime;
    public int Level6_SkillTime;
    public int Level7_SkillTime;
}
#if UNITY_EDITOR
public class SensorEditor
{
    SerializedObject m_Target;

    SerializedProperty m_type;
    SerializedProperty m_NameProperty;
    SerializedProperty m_IconProperty;
    SerializedProperty m_SkillTime;
    SerializedProperty m_percentScore;
    SerializedProperty m_DescriptionProperty;
    SerializedProperty m_AddFever;
    SerializedProperty m_GoldPrice;
    SerializedProperty m_DiaPrice;

    SerializedProperty m_DiaLevel2;
    SerializedProperty m_DiaLevel3;
    SerializedProperty m_DiaLevel4;
    SerializedProperty m_DiaLevel5;
    SerializedProperty m_DiaLevel6;
    SerializedProperty m_DiaLevel7;

    SerializedProperty m_GoldLevel2;
    SerializedProperty m_GoldLevel3;
    SerializedProperty m_GoldLevel4;
    SerializedProperty m_GoldLevel5;
    SerializedProperty m_GoldLevel6;
    SerializedProperty m_GoldLevel7;


    SerializedProperty m_Level2_SkillTime;
    SerializedProperty m_Level3_SkillTime;
    SerializedProperty m_Level4_SkillTime;
    SerializedProperty m_Level5_SkillTime;
    SerializedProperty m_Level6_SkillTime;
    SerializedProperty m_Level7_SkillTime;

    public void Init(SerializedObject target)
    {
        m_Target = target;
        m_type = m_Target.FindProperty(nameof(SensorItem.type));
        m_NameProperty = m_Target.FindProperty(nameof(SensorItem.sensorName));
        m_IconProperty = m_Target.FindProperty(nameof(SensorItem.sensorSprite));
        m_percentScore = m_Target.FindProperty(nameof(SensorItem.percentScore));
        m_SkillTime = m_Target.FindProperty(nameof(SensorItem.skillTime));
        //m_DescriptionProperty = m_Target.FindProperty(nameof(SensorItem.Description));
        m_GoldPrice = m_Target.FindProperty(nameof(SensorItem.GoldPrice));
        m_DiaPrice = m_Target.FindProperty(nameof(SensorItem.DiaPrice));
        //m_AddFever = m_Target.FindProperty(nameof(SensorItem.AddFever));

        m_DiaLevel2 = m_Target.FindProperty(nameof(SensorItem.DiaLevel2));
        m_DiaLevel3 = m_Target.FindProperty(nameof(SensorItem.DiaLevel3));
        m_DiaLevel4 = m_Target.FindProperty(nameof(SensorItem.DiaLevel4));
        m_DiaLevel5 = m_Target.FindProperty(nameof(SensorItem.DiaLevel5));
        m_DiaLevel6 = m_Target.FindProperty(nameof(SensorItem.DiaLevel6));
        m_DiaLevel7 = m_Target.FindProperty(nameof(SensorItem.DiaLevel7));

        m_GoldLevel2 = m_Target.FindProperty(nameof(SensorItem.GoldLevel2));
        m_GoldLevel3 = m_Target.FindProperty(nameof(SensorItem.GoldLevel3));
        m_GoldLevel4 = m_Target.FindProperty(nameof(SensorItem.GoldLevel4));
        m_GoldLevel5 = m_Target.FindProperty(nameof(SensorItem.GoldLevel5));
        m_GoldLevel6 = m_Target.FindProperty(nameof(SensorItem.GoldLevel6));
        m_GoldLevel7 = m_Target.FindProperty(nameof(SensorItem.GoldLevel7));

        m_Level2_SkillTime = m_Target.FindProperty(nameof(SensorItem.Level2_SkillTime));
        m_Level3_SkillTime = m_Target.FindProperty(nameof(SensorItem.Level3_SkillTime));
        m_Level4_SkillTime = m_Target.FindProperty(nameof(SensorItem.Level4_SkillTime));
        m_Level5_SkillTime = m_Target.FindProperty(nameof(SensorItem.Level5_SkillTime));
        m_Level6_SkillTime = m_Target.FindProperty(nameof(SensorItem.Level6_SkillTime));
        m_Level7_SkillTime = m_Target.FindProperty(nameof(SensorItem.Level7_SkillTime));
    }

    public void GUI()
    {
        EditorGUILayout.PropertyField(m_type);
        EditorGUILayout.PropertyField(m_IconProperty);
        EditorGUILayout.PropertyField(m_NameProperty);
        EditorGUILayout.PropertyField(m_percentScore);
        EditorGUILayout.PropertyField(m_SkillTime);
        EditorGUILayout.PropertyField(m_GoldPrice);
        EditorGUILayout.PropertyField(m_DiaPrice);
        EditorGUILayout.PropertyField(m_AddFever);
        EditorGUILayout.PropertyField(m_DescriptionProperty, GUILayout.MinHeight(128));

        EditorGUILayout.PropertyField(m_DiaLevel2);
        EditorGUILayout.PropertyField(m_DiaLevel3);
        EditorGUILayout.PropertyField(m_DiaLevel4);
        EditorGUILayout.PropertyField(m_DiaLevel5);
        EditorGUILayout.PropertyField(m_DiaLevel6);
        EditorGUILayout.PropertyField(m_DiaLevel7);

        EditorGUILayout.PropertyField(m_GoldLevel2);
        EditorGUILayout.PropertyField(m_GoldLevel3);
        EditorGUILayout.PropertyField(m_GoldLevel4);
        EditorGUILayout.PropertyField(m_GoldLevel5);
        EditorGUILayout.PropertyField(m_GoldLevel6);
        EditorGUILayout.PropertyField(m_GoldLevel7);

        EditorGUILayout.PropertyField(m_Level2_SkillTime);
        EditorGUILayout.PropertyField(m_Level3_SkillTime);
        EditorGUILayout.PropertyField(m_Level4_SkillTime);
        EditorGUILayout.PropertyField(m_Level5_SkillTime);
        EditorGUILayout.PropertyField(m_Level6_SkillTime);
        EditorGUILayout.PropertyField(m_Level7_SkillTime);
    }
}
#endif