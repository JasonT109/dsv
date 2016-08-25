using UnityEngine;
using System.Collections;

namespace Meg.Graphics
{
    [System.Serializable]
    public class megColorTheme
    {
        public string name;

        public Color backgroundColor;
        public Color keyColor;
        public Color highlightColor;

        public enum Option
        {
            Background,
            Key,
            Highlight,
            Text
        }

        public Color textColor
        {
            get
            {
                // Infer text color from highlight color.
                // TODO: This should probably be explictly defined.
                var hsb = HSBColor.FromColor(highlightColor);
                hsb.b = 0.8f;

                return hsb.ToColor();
            }
        }

        public Color GetColor(Option option)
        {
            switch (option)
            {
                case Option.Background:
                    return backgroundColor;
                case Option.Highlight:
                    return highlightColor;
                case Option.Key:
                    return keyColor;
                case Option.Text:
                    return textColor;
                default:
                    return highlightColor;
            }
        }

    }
}
