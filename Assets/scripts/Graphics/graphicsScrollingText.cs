using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random = UnityEngine.Random;

public class graphicsScrollingText : widgetText
{

    [TextArea]
    public string Prefix;

    [TextArea]
    public string Lines;

    [TextArea]
    public string Suffix;

    public Vector2 InitialDelayRange = new Vector2(0, 0.5f);
    public Vector2 CharactersPerSecondRange = new Vector2(120, 180);
    public Vector2 DelayBetweenLinesRange = new Vector2(0.25f, 0.5f);

    public int MaxVisibleLines = 0;

    private void OnEnable()
    {
        Text = "";
        StopAllCoroutines();
        StartCoroutine(TextRoutine());
    }

    private IEnumerator TextRoutine()
    {
        if (string.IsNullOrEmpty(Lines))
            yield break;

        var inputLines = new Queue<string>(Lines.Split(new [] { "\r\n", "\n" }, StringSplitOptions.None));
        var displayedLines = new List<string>();

        var initialDelay = new WaitForSeconds(Random.Range(
            InitialDelayRange.x,
            InitialDelayRange.y));

        yield return initialDelay;

        while (true)
        {
            // Rebuild the display with currently visible lines.
            var input = inputLines.Dequeue();
            var current = string.Join("\n", displayedLines.ToArray());
            if (!string.IsNullOrEmpty(current))
                current += "\n";

            // Output the current line to display.
            var characterDelay = new WaitForSeconds(1 / Random.Range(
                CharactersPerSecondRange.x, 
                CharactersPerSecondRange.y));

            for (var i = 0; i < input.Length; i++)
            {
                current += input[i];

                var value = "";
                if (!string.IsNullOrEmpty(Prefix))
                    value += Prefix + "\n";

                value += current;

                if (!string.IsNullOrEmpty(current))
                    value += "\n" + Suffix;

                Text = value;
                yield return characterDelay;
            }

            // Update the display line buffer.
            displayedLines.Add(input);
            if (MaxVisibleLines > 0 && displayedLines.Count > MaxVisibleLines)
            {
                inputLines.Enqueue(displayedLines[0]);
                displayedLines.RemoveAt(0);
            }

            // Wait a bit before starting next line.
            var lineDelay = new WaitForSeconds(Random.Range(
                DelayBetweenLinesRange.x,
                DelayBetweenLinesRange.y));

            yield return lineDelay;
        }
    }


}
