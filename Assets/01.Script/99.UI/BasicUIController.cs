using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUIController : MonoBehaviour
{
    [SerializeField] GameObject controllGameObject;
    // Start is called before the first frame update
    void Start()
    {
        if (controllGameObject == null)
            controllGameObject = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseButton()
    {
        if(controllGameObject != null)
            controllGameObject.SetActive(false);
    }
}
