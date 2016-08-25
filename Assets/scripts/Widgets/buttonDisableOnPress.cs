using UnityEngine;
using System.Collections;

public class buttonDisableOnPress : MonoBehaviour
{

    public buttonControl Button;
    public float Delay = 0.5f;

    private void Start()
    {
        if (!Button)
            Button = GetComponent<buttonControl>();

        if (Button)
            Button.onPressed += OnButtonPressed;
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        SetInputEnabled(true);
    }

    private void OnButtonPressed()
    {
        SetInputEnabled(false);
        StartCoroutine(EnableRoutine());
    }

    private IEnumerator EnableRoutine()
    {
        yield return new WaitForSeconds(Delay);
        SetInputEnabled(true);
    }

    private void SetInputEnabled(bool value)
    {
        if (!Button)
            return;

        var c = Button.GetComponent<Collider>();
        if (c)
            c.enabled = value;
    }

}
