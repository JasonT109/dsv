using UnityEngine;
using System.Collections;

public class DCCLogoRotator : MonoBehaviour
{
    public float speed = 32f;

    void Update()
    {
        this.transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
