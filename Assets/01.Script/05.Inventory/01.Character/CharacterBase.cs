using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "GameCharacter/Character", order = -999)]

public class CharacterBase : ScriptableObject
{
    public string CharacterName;    //캐릭터 이름
    public DataType.CharacterType CharacterType;
    public DataType.PriceType LevelUpPriceType;

    [Header("Image")]
    public Sprite readyCharacterImage;
    public Sprite mainCharacter;
    public Sprite CharacterSprite;  //캐릭터 이미지
    public Sprite MarketCharacterSprite;  //상점 이미지
    public Sprite ProfileIconImage; //프로핊 아이콘
    public Sprite LobbySelectIconImage; //대기 캐릭터 아이콘
    [TextArea]
    public string CharacterDescription;

    public int Num;
    public int currentLevel;
    public float hpMinusPercent;             //캐릭터 방어력
    public float itemPercent;             //캐릭터 아이템 지속시간 증가량
    public int SkillAbility;
    public float fevetTime;

    [Header("Exp")]
    public int CurrentEXP;
    public int MaxEXP;


    [Header("BuyPrice")]
    public int characterDiaPrice;
    public int characterGoldPrice;

    [Header("LevelUpPrice")]
    public int levelGoldPrice;
    public int levelDiaPrice;

    [Header("Gold")]
    public int GoldLevel2;
    public int GoldLevel3;
    public int GoldLevel4;
    public int GoldLevel5;
    public int GoldLevel6;
    public int GoldLevel7;

    [Header("Dia")]
    public int DiaLevel2;
    public int DiaLevel3;
    public int DiaLevel4;
    public int DiaLevel5;
    public int DiaLevel6;
    public int DiaLevel7;
}


#if UNITY_EDITOR
public class CharacterEditor
{
    SerializedObject m_Target;

    SerializedProperty m_NameProperty;
    SerializedProperty m_jobType;
    SerializedProperty m_grade;
    SerializedProperty m_level;
    SerializedProperty m_Description;
    SerializedProperty m_IconProperty;
    SerializedProperty m_MarketIconProperty;
    SerializedProperty m_hpMinusPercent;
    SerializedProperty m_itemPercent;
    SerializedProperty m_SkillAbility;
    SerializedProperty m_health;
    SerializedProperty m_CurrentEXP;
    SerializedProperty m_MaxEXP;
    SerializedProperty m_feverTime;
    SerializedProperty m_GoldPrice;
    SerializedProperty m_DiaPrice;

    SerializedProperty m_GoldLevel2;
    SerializedProperty m_GoldLevel3;
    SerializedProperty m_GoldLevel4;
    SerializedProperty m_GoldLevel5;
    SerializedProperty m_GoldLevel6;
    SerializedProperty m_GoldLevel7;

    SerializedProperty m_DiaLevel2;
    SerializedProperty m_DiaLevel3;
    SerializedProperty m_DiaLevel4;
    SerializedProperty m_DiaLevel5;
    SerializedProperty m_DiaLevel6;
    SerializedProperty m_DiaLevel7;

    public void Init(SerializedObject target)
    {
        m_Target = target;

        m_NameProperty = m_Target.FindProperty(nameof(CharacterBase.CharacterName));
        m_IconProperty = m_Target.FindProperty(nameof(CharacterBase.CharacterSprite));
        m_MarketIconProperty = m_Target.FindProperty(nameof(CharacterBase.MarketCharacterSprite));
        m_level = m_Target.FindProperty(nameof(CharacterBase.currentLevel));
        m_hpMinusPercent = m_Target.FindProperty(nameof(CharacterBase.hpMinusPercent));
        m_itemPercent = m_Target.FindProperty(nameof(CharacterBase.itemPercent));
        m_Description = m_Target.FindProperty(nameof(CharacterBase.CharacterDescription));
        m_CurrentEXP = m_Target.FindProperty(nameof(CharacterBase.CurrentEXP));
        m_SkillAbility = m_Target.FindProperty(nameof(CharacterBase.SkillAbility));
        m_MaxEXP = m_Target.FindProperty(nameof(CharacterBase.MaxEXP));
        m_feverTime = m_Target.FindProperty(nameof(CharacterBase.fevetTime));
        m_GoldPrice = m_Target.FindProperty(nameof(CharacterBase.levelGoldPrice));
        m_DiaPrice = m_Target.FindProperty(nameof(CharacterBase.levelDiaPrice));

        m_GoldLevel2 = m_Target.FindProperty(nameof(CharacterBase.GoldLevel2));
        m_GoldLevel3 = m_Target.FindProperty(nameof(CharacterBase.GoldLevel3));
        m_GoldLevel4 = m_Target.FindProperty(nameof(CharacterBase.GoldLevel4));
        m_GoldLevel5 = m_Target.FindProperty(nameof(CharacterBase.GoldLevel5));
        m_GoldLevel6 = m_Target.FindProperty(nameof(CharacterBase.GoldLevel6));
        m_GoldLevel7 = m_Target.FindProperty(nameof(CharacterBase.GoldLevel7));

        m_DiaLevel2 = m_Target.FindProperty(nameof(CharacterBase.DiaLevel2));
        m_DiaLevel3 = m_Target.FindProperty(nameof(CharacterBase.DiaLevel3));
        m_DiaLevel4 = m_Target.FindProperty(nameof(CharacterBase.DiaLevel4));
        m_DiaLevel5 = m_Target.FindProperty(nameof(CharacterBase.DiaLevel5));
        m_DiaLevel6 = m_Target.FindProperty(nameof(CharacterBase.DiaLevel6));
        m_DiaLevel7 = m_Target.FindProperty(nameof(CharacterBase.DiaLevel7));
    }

    public void GUI()
    {
        EditorGUILayout.PropertyField(m_IconProperty );
        EditorGUILayout.PropertyField(m_MarketIconProperty);
        EditorGUILayout.PropertyField(m_NameProperty );
        EditorGUILayout.PropertyField(m_jobType);
        EditorGUILayout.PropertyField(m_grade);
        EditorGUILayout.PropertyField(m_level);
        EditorGUILayout.PropertyField(m_hpMinusPercent);
        EditorGUILayout.PropertyField(m_itemPercent);
        EditorGUILayout.PropertyField(m_SkillAbility);
        EditorGUILayout.PropertyField(m_health );
        EditorGUILayout.PropertyField(m_CurrentEXP);
        EditorGUILayout.PropertyField(m_MaxEXP);
        EditorGUILayout.PropertyField(m_feverTime);
        EditorGUILayout.PropertyField(m_GoldPrice);
        EditorGUILayout.PropertyField(m_DiaPrice);

        EditorGUILayout.PropertyField(m_GoldLevel2);
        EditorGUILayout.PropertyField(m_GoldLevel3);
        EditorGUILayout.PropertyField(m_GoldLevel4);
        EditorGUILayout.PropertyField(m_GoldLevel5);
        EditorGUILayout.PropertyField(m_GoldLevel6);
        EditorGUILayout.PropertyField(m_GoldLevel7);

        EditorGUILayout.PropertyField(m_DiaLevel2);
        EditorGUILayout.PropertyField(m_DiaLevel3);
        EditorGUILayout.PropertyField(m_DiaLevel4);
        EditorGUILayout.PropertyField(m_DiaLevel5);
        EditorGUILayout.PropertyField(m_DiaLevel6);
        EditorGUILayout.PropertyField(m_DiaLevel7);
    }
}
#endif