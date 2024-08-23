using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationObject : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float rotationRange;
    [SerializeField] float addRange;
    int addMinus = 1;
    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(addRange) > rotationRange)
        {
            addMinus *= -1;
        }
        transform.Rotate(new Vector3(0,0, rotationSpeed * Time.deltaTime * addMinus));
        addRange += Time.deltaTime * rotationSpeed * addMinus;

    }
}
