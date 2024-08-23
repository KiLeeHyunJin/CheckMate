using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    //login,          //0        login,          //0
    //lobby,          //1        lobby,          //1
    //mode_select,    //2        mode_select,    //2
    //ovenBreak,      //3        ovenBreak,      //3
    //characters,     //4        characters,     //4
    //Inventory,      //5        Inventory,      //5
    //store,          //6        store,          //6
    //mode_ready,     //7        mode_ready,     //7
    //mode_stage,     //8        mode_stage,     //8

public class SceneChanger : MonoBehaviour
{
    [SerializeField] DataType.SceneType[] scene;
    void Start()
    {
        scene = new DataType.SceneType[(int)DataType.SceneType.END];
        for (int i = 0; i < scene.Length; i++)
        {
            scene[i] = (DataType.SceneType)i;
        }

    }
    public void BackScene()
    {
        DataType.SceneType before = GameManager.instance.GetBeforeSceneType();
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Back);

        if (before == DataType.SceneType.login)
        {
            before = DataType.SceneType.lobby;
        }
        int BackSceneInt = (int)before;
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene[BackSceneInt].ToString());
        SetSceneType(BackSceneInt);
    }
    public void StageOut()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Back);
        GameManager.instance.SetStageNum(0);

        UnityEngine.SceneManagement.SceneManager.LoadScene("mode_select");
        SetSceneType((int)DataType.SceneType.mode_select);
    }
    public void MoveScene(int sceneNum)
    {
        if (sceneNum >= (int)DataType.SceneType.END)
            return;
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Check);
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene[sceneNum].ToString());
        SetSceneType(sceneNum);
    }
    public void StageToChapter()
    {
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Back);

        int _num = (int)DataType.SceneType.mode_select;
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene[_num].ToString());
        SetSceneType(_num);
    }
    public void MoveSceneBack(int sceneNum)
    {
        if (sceneNum >= (int)DataType.SceneType.END)
            return;
        SFXmanager.instance.PlayOnSFX(SFXmanager.SFXClip.Back);
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene[sceneNum].ToString());
        SetSceneType(sceneNum);
    }

    void SetSceneType(int sceneNum)
    {
        GameManager.instance.SetSceneType( (DataType.SceneType) sceneNum );
    }
}
