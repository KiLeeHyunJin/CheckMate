using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    //public GameObject IntroPanel;
    //public GameObject LogoPanel;
    public DataType.SceneType sceneType;
    public Animator animator;
    public float defaultTime = 2f;
    [SerializeField] float addTime; 
    // Start is called before the first frame update
    void Start()
    {
        addTime = 0f;
        //StartCoroutine(DelayTime(2));
        if (animator ==null)
            animator = GetComponent<Animator>();
    }
    IEnumerator DelayTime(float time)
    {
        yield return new WaitForSeconds(time);
    }
    // Update is called once per frame
    void Update()
    {
        addTime += Time.deltaTime;

        if (animator != null)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            {
                SetSceneType();
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneType.ToString());
            }
            return;
        }
                //IntroPanel.SetActive(true);
                //LogoPanel.SetActive(false);
        if(defaultTime < addTime)
        {
            SetSceneType();
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneType.ToString());
        }

    }

    void SetSceneType()
    {
        GameManager.instance.SetSceneType(DataType.SceneType.lobby, DataType.SceneType.login);
    }
}
