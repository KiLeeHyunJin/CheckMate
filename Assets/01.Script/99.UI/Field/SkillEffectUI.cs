using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEffectUI : MonoBehaviour
{
    [SerializeField] GameObject feverUI;
    [SerializeField] GameObject SkillUI;
    [SerializeField] GameObject SensorUI;

    Animator FeverAnim;
    Animator SkillAnim;
    Animator SensorAnim;

    Animator CurrentAnim;
    float animSpeed = 1;


    // Start is called before the first frame update
    void Start()
    {
        FeverAnim = feverUI.GetComponent<Animator>();
        SkillAnim = SkillUI.GetComponent<Animator>();
        SensorAnim = SensorUI.GetComponent<Animator>();

        AllUISetFalse();
    }

    public void AnimSpeed(float _speed)
    {
        animSpeed = _speed;
    }

    public void SetSkillEffectUI(DataType.EffectUIType _type)
    {
        //Animator animator = null;
        //CurrentAnimNUll();
        switch (_type)
        {
            case DataType.EffectUIType.Fever:
                
                break;
            case DataType.EffectUIType.SKill:
                AnimStart(SkillAnim);
                Image img = SkillUI.GetComponent<Image>();
                if (img != null)
                    img.SetNativeSize();
                //animator = SkillAnim;
                break;
            case DataType.EffectUIType.Sensor:
                //animator = SensorAnim;
                break;
        }
    }

    void AnimStart(Animator animator)
    {
        if (AnimNullCheck(animator))
        {
            CurrentUISetActive(true, animator);
            animator.SetTrigger("Effect");
            animator.speed = animSpeed;
            StartCoroutine(AnimOff(animator));
        }
    }

    IEnumerator AnimOff(Animator animator)
    {
        while(true)
        {
            if (AnimNullCheck(animator) && AnimStateEndCheck(animator))
            {
                //CurrentAnimReset(animator);
                CurrentUISetActive(false, animator);
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    bool AnimStateEndCheck(Animator animator)
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            return true;
        return false;
    }

    bool AnimNullCheck(Animator animator)
    {
        if(animator == null)
            return false;
        return true;
    }
    void CurrentAnimReset(Animator animator)
    {
        animator.SetTrigger("Init");
    }
    void CurrentUISetActive(bool _state, Animator animator)
    {
        if(AnimNullCheck(animator))
            animator.gameObject.SetActive(_state);
    }

    void CurrentAnimNUll()
    {
        CurrentAnim = null;
    }

    void AllUISetFalse()
    {
        if (feverUI != null)
            feverUI.SetActive(false);
        if (SkillUI != null)
            SkillUI.SetActive(false);
        if (SensorUI != null)
            SensorUI.SetActive(false);
    }
}
