using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Meg.Networking;
using Random = UnityEngine.Random;

public class DiagnosticFeedItem : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    public Renderer Backdrop;
    public widgetText Number;
    public widgetText Code;
    public widgetText Text;
    public Vector2 CharactersPerSecondRange = new Vector2(120, 180);
    public Vector2 InitialTextDelayRange = new Vector2(0.5f, 1.5f);

    /** Possible states. */
    public ItemState[] States;

    public enum ItemType
    {
        Inactive,
        Normal,
        Pending,
        Warning,
        Error
    }

    [System.Serializable]
    public struct ItemState
    {
        public ItemType Type;
        public Color Backdrop;
    }

    /** Current state. */
    public ItemState State { get; set; }


    // Unity Methods
    // ------------------------------------------------------------

    /** Enabling. */
    private void OnEnable()
    {
        Text.gameObject.SetActive(true);
        if (State.Type != ItemType.Inactive)
            Configure(State.Type, true);
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Set the items' current state by type. */
    public void Configure(ItemType type, bool animate)
    {
        State = States.FirstOrDefault(state => state.Type == type);
        SetBackdropColor(State.Backdrop, animate);

        var hideText = (type == ItemType.Inactive) ||
            (type == ItemType.Normal && Random.value < 0.05f);

        if (hideText)
        {
            Number.Text = "";
            Code.Text = "";
            Text.Text = "";
            return;
        }

        Number.Text = Random.Range(1, 10).ToString("D2");
        Code.Text = Random.Range(-300000, 1100000).ToString("D5");

        var text = GenerateRandomText(type);
        StartCoroutine(RevealTextRoutine(Text, text));
    }


    // Private Methods
    // ------------------------------------------------------------

    private void SetBackdropColor(Color c, bool animate)
    {
        if (animate)
            Backdrop.material.DOColor(c, 0.25f);
        else
            Backdrop.material.color = c;
    }

    private string GenerateRandomText(ItemType type)
    {
        try
        {
            switch (type)
            {
                case ItemType.Error:
                    return GenerateRandomErrorText();
                case ItemType.Warning:
                    return GenerateRandomWarningText();
                case ItemType.Pending:
                    return GenerateRandomPendingText();
                default:
                    return GenerateRandomNormalText();
            }
        }
        catch (Exception)
        {
            return "...";
        }
    }

    private string GenerateRandomNormalText()
    {
        var index = Random.Range(0, 9);
        switch (index)
        {
            case 0:
                return string.Format("CAM {0:D2} LIVE...", Random.Range(0, 10));
            case 1:
                return "TARGET ON";
            case 2:
                return "TARGET OFF";
            case 3:
                return string.Format("READING {0:D3} SEC", Random.Range(0, 50));
            case 4:
                return string.Format("SYSTEM CHECK {0:D2}SEC", Random.Range(0, 50));
            case 5:
                return string.Format("FEED CHECK {0:N2}ms", Random.Range(0, 2.5f));
            case 6:
                var errorKeys = GetErrorKeys();
                var error = errorKeys[Random.Range(0, errorKeys.Count)];
                return string.Format("{0} NOMINAL", error.Replace("error_", "").ToUpper());
            default:
                return "";
        }
    }

    private string GenerateRandomPendingText()
    {
        var index = Random.Range(0, 5);
        switch (index)
        {
            case 0:
                return string.Format("READING {0:D3}", Random.Range(0, 50));
            case 1:
                return string.Format("SYSTEM CHECK {0:D2}SEC", Random.Range(0, 50));
            case 2:
                return string.Format("FEED CHECK {0:N2}ms", Random.Range(0, 2.5f));
            case 3:
                var errorKeys = GetErrorKeys();
                var error = errorKeys[Random.Range(0, errorKeys.Count)];
                return string.Format("CHECKING {0}...", error.Replace("error_", "").ToUpper());
            default:
                return "PENDING...";
        }
    }

    private string GenerateRandomWarningText()
    {
        var errors = GetActiveErrors();
        if (errors.Count <= 0)
            return string.Format("WARNING {0:D3}", Random.Range(0, 500));

        var error = errors[Random.Range(0, errors.Count)];
        return string.Format("WARNING: {0}", error.Replace("error_", "").ToUpper());
    }

    private string GenerateRandomErrorText()
    {
        var errors = GetActiveErrors();
        if (errors.Count <= 0)
            return "UNKNOWN ERROR...";

        var error = errors[Random.Range(0, errors.Count)];
        return string.Format("ERROR: {0}", error.Replace("error_", "").ToUpper());
    }

    private List<string> GetErrorKeys()
    {
        return serverUtils.Parameters
            .Where(p => p.StartsWith("error_"))
            .ToList();
    }

    private List<string> GetActiveErrors()
    {
        // Try to pick a random server error that's currently active.
        return serverUtils.Parameters
            .Where(p => p.StartsWith("error_") && serverUtils.GetServerData(p) > 0)
            .ToList();
    }

    private IEnumerator RevealTextRoutine(widgetText control, string input)
    {
        var delay = Random.Range(InitialTextDelayRange.x, InitialTextDelayRange.y);
        yield return new WaitForSeconds(delay);
        
        var characterDelay = new WaitForSeconds(1 / Random.Range(
            CharactersPerSecondRange.x, CharactersPerSecondRange.y));

        control.Text = "";
        for (var i = 0; i < input.Length; i++)
        {
            control.Text += input[i];
            yield return characterDelay;
        }

        control.Text = input;

        // Blink text for more serious stuff.
        if (State.Type > ItemType.Normal)
            StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        int n = Random.Range(20, 20);
        var wait = new WaitForSeconds(0.25f);
        for (var i = 0; i < n; i ++)
        {
            Text.gameObject.SetActive(!Text.gameObject.activeSelf);
            yield return wait;
        }

        Text.gameObject.SetActive(true);
    }

}
