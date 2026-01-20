using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Globalization;

namespace jam.CodeBase.Utils
{
    public static class TmpTextDotweenExtensions
    {
        public static Tweener DOFloatNumber(
            this TMP_Text text,
            float to,
            float duration,
            string format = "{0:0}",
            float step = 1f)
        {
            if (text == null)
            {
                Debug.LogError("DOFloatNumber: text is null");
                return null;
            }

            float from = 0f;

            var raw = text.text;
            if (!string.IsNullOrEmpty(raw))
            {
                raw = raw.Replace(" ", string.Empty)
                         .Replace("$", string.Empty)
                         .Replace(",", "."); 

                if (float.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed))
                    from = parsed;
            }

            float lastShown = from;

            return DOTween
                .To(() => from, x =>
                {
                    from = x;

                    if (Mathf.Abs(from - lastShown) >= step)
                    {
                        lastShown = from;
                        text.SetText(format, from);
                    }
                }, to, duration)
                .OnComplete(() =>
                {
                    text.SetText(format, to);
                });
        }

        public static Tweener DOFloatNumber(
            this TMP_Text text,
            float from,
            float to,
            float duration,
            string format = "{0:0}",
            float step = 1f)
        {
            if (text == null)
            {
                Debug.LogError("DOFloatNumber (from,to): text is null");
                return null;
            }

            float current = from;
            float lastShown = from;

            return DOTween
                .To(() => current, x =>
                {
                    current = x;

                    if (Mathf.Abs(current - lastShown) >= step)
                    {
                        lastShown = current;
                        text.SetText(format, current);
                    }
                }, to, duration)
                .OnComplete(() =>
                {
                    text.SetText(format, to);
                });
        }
    }
}
