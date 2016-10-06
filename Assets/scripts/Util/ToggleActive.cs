using UnityEngine;
using System.Collections;

public class ToggleActive : MonoBehaviour
{

    public GameObject Target;

    public void Toggle()
        { Target.SetActive(!Target.activeSelf); }
}
