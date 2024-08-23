using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyBell : MonoBehaviour
{
    [SerializeField] GameObject bellButton;

    public void OpenBell()
    {
        bellButton.SetActive(true);
    }
    public void CloseBell()
    {
        bellButton.GetComponent<Animator>().SetTrigger("IsClose");
        //bellButton.SetActive(false);
    }
}
