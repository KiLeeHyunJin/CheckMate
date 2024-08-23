using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "GradeItem", menuName = "GameItem/GradeItem", order = -999)]

public class GradeItem : ScriptableObject
{
    public string ItemName;
    public string CharacterName;
    public Sprite ItemSprite;
    public string Description;

}


#if UNITY_EDITOR
public class GradeItemEditor
{
    SerializedObject m_Target;

    SerializedProperty m_NameProperty;
    SerializedProperty m_CharacterNameProperty;
    SerializedProperty m_IconProperty;
    SerializedProperty m_DescriptionProperty;
    
    public void Init(SerializedObject target)
    {
        m_Target = target;

        m_NameProperty = m_Target.FindProperty(nameof(Item.ItemName));
        m_CharacterNameProperty = m_Target.FindProperty(nameof(Item.ItemName));
        m_IconProperty = m_Target.FindProperty(nameof(Item.ItemSprite));
        m_DescriptionProperty = m_Target.FindProperty(nameof(Item.Description));
    }

    public void GUI()
    {
        EditorGUILayout.PropertyField(m_IconProperty);
        EditorGUILayout.PropertyField(m_NameProperty);
        EditorGUILayout.PropertyField(m_CharacterNameProperty);
        EditorGUILayout.PropertyField(m_DescriptionProperty, GUILayout.MinHeight(128));
    }
}
#endif