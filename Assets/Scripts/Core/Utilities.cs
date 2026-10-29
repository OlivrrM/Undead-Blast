using UnityEngine;
using System.Collections;

public static class Utilities {
    public static int BoolToInt(bool boolean)
    {
        if (boolean) return 1;
        return 0;
    }
    public static int BoolToIntn(bool boolean)
    {
        if (boolean) return 1;
        return -1;
    }
    public static bool IntToBool(int integer)
    {
        if (integer == 1) return true;
        return false;
    }
    public static bool CoinFlip()
    {
        int dice = UnityEngine.Random.Range(0, 2);
        return IntToBool(dice);
    }
    public static float CoinFlipn()
    {
        int dice = 0;
        while (true)
        {
            dice = UnityEngine.Random.Range(-1, 2);
            if (dice != 0) break;
        }

        return dice;
    }
    // SmoothDamp ensures a frame-rate independent interpolation
    public static Quaternion SmoothDamp(Quaternion current, Quaternion target, ref Quaternion velocity, float smoothTime, float deltaTime)
    {
        if (deltaTime < Mathf.Epsilon) return current;

        float t = 1f - Mathf.Exp(-deltaTime / smoothTime);
        return Quaternion.Slerp(current, target, t);
    }
    public static Vector3 V3All(float value) { return new Vector3(value, value, value); }
    public static Vector3 RandomRangeVector3(Vector3 min, Vector3 max)
    {
        return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    }
    public static float NextFloat(System.Random random, float min, float max)
    {
        return (float)(random.NextDouble() * (max - min) + min);
    }
}
