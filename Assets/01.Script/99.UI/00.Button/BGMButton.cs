using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMButton : MonoBehaviour
{
    public Slider slider;
    public Sprite onImage;
    public Sprite offImage;
    Text text;
    Toggle toggle;
    Outline outline;
    Color oncolor = new Color(0.3529412f, 0.4156863f, 0.1058824f, 1f);
    Color offcolor = new Color(0.4392157f, 0.4156863f, 0.3529412f, 1f);
    [Header("Buttons")]
    [SerializeField] Image OnButton;
    [SerializeField] Image OffButton;
    [Header("OnImage")]
    [SerializeField] Sprite OnEnableImage;
    [SerializeField] Sprite OnDisableImage;
    [Header("OffImage")]
    [SerializeField] Sprite OffEnsableImage;
    [SerializeField] Sprite OffDisableImage;
    private void Awake()
    {
        outline = gameObject.GetComponentInChildren<Outline>();
        text = gameObject.GetComponentInChildren<Text>();
        toggle = gameObject.GetComponent<Toggle>();
        if (!PlayerPrefs.HasKey("BGMisOn"))
            PlayerPrefs.SetInt("BGMisOn", 1);
    }
    public void ValueOn()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        OnImage();
        PlayerPrefs.SetInt("BGMisOn", 1);
        //gameObject.GetComponentInChildren<Image>().sprite = onImage;
        TextChange("켜짐");
        if (outline != null)
            gameObject.GetComponentInChildren<Outline>().effectColor = oncolor;
        BGMmanager.instance.SoundOn(PlayerPrefs.GetFloat("BGMslider"));
    }
    public void ValueOff()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);

        OffImage();
        PlayerPrefs.SetInt("BGMisOn", 0);
        //gameObject.GetComponentInChildren<Image>().sprite = offImage;
        TextChange("꺼짐");

        if (outline != null)
            gameObject.GetComponentInChildren<Outline>().effectColor = offcolor;
        BGMmanager.instance.SoundOff();
    }
    private void OnEnable()
    {
        //SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        if(PlayerPrefs.GetInt("BGMisOn") == 1)
        {
            OnImage();
            if(toggle != null)
                gameObject.GetComponent<Toggle>().isOn = true;
            //gameObject.GetComponentInChildren<Image>().sprite = onImage;
            TextChange("켜짐");
            if(outline != null)
                gameObject.GetComponentInChildren<Outline>().effectColor = oncolor;
            BGMmanager.instance.SoundOn(PlayerPrefs.GetFloat("BGMslider"));
        }
        else
        {
            OffImage();
            if(toggle != null)
                gameObject.GetComponent<Toggle>().isOn = false;
            //gameObject.GetComponentInChildren<Image>().sprite = offImage;
            TextChange("꺼짐");
            if (outline != null)
                gameObject.GetComponentInChildren<Outline>().effectColor = offcolor;
            BGMmanager.instance.SoundOff();
        }
    }

    //706A5A,5A6A1B//
    public void Valuechanged()
    {
        //SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        if (gameObject.GetComponent<Toggle>().isOn == true)
        {
            PlayerPrefs.SetInt("BGMisOn", 1);
            //gameObject.GetComponentInChildren<Image>().sprite = onImage;
            TextChange("켜짐");
            if(outline != null)
            gameObject.GetComponentInChildren<Outline>().effectColor = oncolor;
            BGMmanager.instance.SoundOn(PlayerPrefs.GetFloat("BGMslider"));
        }
        else
        {
            PlayerPrefs.SetInt("BGMisOn", 0);
            //gameObject.GetComponentInChildren<Image>().sprite = offImage;
            TextChange("꺼짐");

            if(outline != null)
            gameObject.GetComponentInChildren<Outline>().effectColor = offcolor;
            BGMmanager.instance.SoundOff();
        }
    }
    void TextChange(string _string)
    {
        if (text != null    )
        text.text = _string;
    }
    void OnImage()
    {
        if (OnButton != null)
        {
            if (OnEnableImage != null)
                OnButton.sprite = OnEnableImage;
        }
        if (OffButton != null)
        {
            if (OffDisableImage != null)
                OffButton.sprite = OffDisableImage;
        }
    }
    void OffImage()
    {
        if (OffButton != null)
        {
            if (OffEnsableImage != null)
                OffButton.sprite = OffEnsableImage;
        }
        if (OnButton != null)
        {
            if (OnDisableImage != null)
                OnButton.sprite = OnDisableImage;
        }
    }
}
