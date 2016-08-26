using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random = UnityEngine.Random;

public class StartUpScrollScript : widgetText
{

    [TextArea]
    public string Lines;

    [TextArea]
    public string Footer;

    public float CharactersPerSecond = new float();
    public Vector2 DelayBetweenLinesRange = new Vector2(0.0f, 0.5f);

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(Lines))
            Lines = Text;

        Text = "";
        StopAllCoroutines();
        StartCoroutine(TextRoutine());
    }

    private IEnumerator TextRoutine()
    {
        if (string.IsNullOrEmpty(Lines)) yield break;

        var inputLines = new Queue<string>(Lines.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None));
        var displayedLines = new List<string>();

        while (inputLines.Count > 0)
        {
            var input = inputLines.Dequeue();
            var current = string.Join("\n", displayedLines.ToArray());
            if (!string.IsNullOrEmpty(current)) current += "\n";

            var tNext = Time.time;
            var intervalBetweenCharacters = 1 / CharactersPerSecond;

            // var characterDelay = new WaitForSeconds();

            for (var i = 0; i < input.Length; i++)
            {
                current += input[i];

                var value = "";
                if (!string.IsNullOrEmpty(Footer)) value += Footer + "\n";

                value += current;

                if (!string.IsNullOrEmpty(current)) value += "\n" + Footer;

                Text = value;

                tNext += intervalBetweenCharacters;

                if (Time.time < tNext)
                    yield return new WaitForSeconds(tNext - Time.time);
            }

            displayedLines.Add(input);

            var lineDelay = new WaitForSeconds(Random.Range(
                DelayBetweenLinesRange.x,
                DelayBetweenLinesRange.y));

            yield return lineDelay;
            

        }
    }
}

