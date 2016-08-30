using UnityEngine;
using System.Collections;

public class DCC_PatientLogScroll : MonoBehaviour
{
    public bool updateOn = true;
    public Vector2 Scroll = new Vector2(0.05f, 0.05f);
    Vector2 Offset = new Vector2(0f, 0f);
    public Renderer rend;
    

    void OnEnable()
    {
        StartCoroutine(updateOff());
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (updateOn == true)
        {
            Offset += Scroll * Time.deltaTime;
            rend.material.SetTextureOffset("_MainTex", Offset);
        }
    } 


    IEnumerator updateOff()
    {
        yield return new WaitForSeconds(40.0f);
        updateOn = false;
    }
}
