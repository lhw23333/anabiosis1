using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpike : MonoBehaviour
{
    public float rotateSpeed;
    public float currentRotation;
    // Start is called before the first frame update
    void Start()
    {
        currentRotation = transform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0,0,currentRotation + rotateSpeed);
        currentRotation += rotateSpeed;
      
    }
}
