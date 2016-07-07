using UnityEngine;
using System.Collections;

public class sonarRotate : MonoBehaviour
{
    public float speed = 1.0f;
    public float rotateSpeed = 1.0f;
    public Color color1;
    public Color color2;
    public GameObject beam;
    public GameObject[] directionTicks;
    private Material dm;

	void Update ()
    {

        if (directionTicks.Length > 0)
        {
            for (int i = 0; i < directionTicks.Length; i++)
            {
                dm = directionTicks[i].GetComponent<Renderer>().material;
                if (gameObject.transform.eulerAngles.z > (360 - (45 * (i + 1))) && gameObject.transform.eulerAngles.z < (360 - (45 * i)))
                {
                    dm.SetColor("_TintColor", Color.Lerp(dm.GetColor("_TintColor"), color2, Time.deltaTime * speed));
                }
                else
                {
                    dm.SetColor("_TintColor", Color.Lerp(dm.GetColor("_TintColor"), color1, Time.deltaTime * speed));
                }
            }
        }
        gameObject.transform.Rotate(0, 0, -(rotateSpeed * Time.deltaTime));
    }
}
