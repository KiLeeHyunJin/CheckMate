using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] SampleScrolling sample;
    [SerializeField] float value;
    [SerializeField] Vector3 pos;
    private void Update()
    {
        obj.transform.position = pos;
    }
}
