using UnityEngine;

public static class ExtensionsColor
{
    public static void Random(this ref Color self, float spread = 0.15f)
    {
        do
        {
            self.r = UnityEngine.Random.value;
            self.g = UnityEngine.Random.value;
            self.b = UnityEngine.Random.value;
        }
        while (Mathf.Abs(self.r - self.g) <= spread && Mathf.Abs(self.r - self.b) <= spread && Mathf.Abs(self.g - self.b) <= spread);
    }

    public static void SetBrightness(this ref Color self, float brightness)
    {
        self.r *= brightness;
        self.g *= brightness;
        self.b *= brightness;
    }

    public static Color RandomRecolor(this Color self, Vector2 rangeR, Vector2 rangeG, Vector2 rangeB)
    {
        self.r *= GetMultiplier(rangeR);
        self.g *= GetMultiplier(rangeG);
        self.b *= GetMultiplier(rangeB);

        return self;

        #region Locla Function
        static float GetMultiplier(Vector2 range)
        {
            float spread = Mathf.Abs(range.y - range.x) / 10f;
            float value;
            do
            {
                value = range.RandomRange();
            }
            while (value > 1f - spread && value < 1f + spread);

            return value;
        }
        #endregion
    }
}
