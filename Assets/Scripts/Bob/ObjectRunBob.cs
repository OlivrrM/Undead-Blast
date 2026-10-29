using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRunBob : MonoBehaviour
{
    public LocalOffsetManager localOffsetManager;
    public AnimationCurve y;
    public AnimationCurve x;
    public float bobSmoothness;

    float yPointer;
    public float yBobSpeed;
    public float yBobAmount;
    [HideInInspector] public float currentYBob;

    float xPointer;
    public float xBobSpeed;
    public float xBobAmount;
    [HideInInspector] public float currentXBob;

    float runningMultiplier;

    float time;

    public float stopRunningSmoothness;

    public Vector2 baseAmountMultiplier; //Temp hardcoded system until dictionary system is fixed

    public Dictionary<string, bool> enablers = new Dictionary<string, bool>();
    public Dictionary<string, float> speedMultipliers = new Dictionary<string, float>();
    public Dictionary<string, Vector2> amountMultipliers = new Dictionary<string, Vector2>();
    private void Start()
    {
        //cameraRotationOffsetManager.AddOffset("RunBob", new Vector4(0, 0, 0, 0));
        localOffsetManager.AddValue("RunBob", Vector3.zero);
        enablers.Add("Running", false);
        baseAmountMultiplier = Vector2.one;
        Cache.itemRunBobManager = this;
    }
    private void Update()
    {
        float speedMultiplier = 0f;
        if (speedMultipliers.Count > 0)
        {
            foreach (KeyValuePair<string, float> mltp in speedMultipliers)
            {
                speedMultiplier += mltp.Value;
            }
        }
        else { speedMultiplier = 1f; }
        time += Time.deltaTime * runningMultiplier * FragMovementListener.hSpeedPercentage * speedMultiplier;
        if (float.IsNaN(time) || float.IsInfinity(time)) { time = 0f; }
        enablers["Running"] = FragMovementListener.running;
        bool enabled = false;
        foreach (KeyValuePair<string, bool> enabler in enablers)
        {
            if (enabler.Value) { enabled = true; break; }
        }
        if (enabled) { runningMultiplier = 1; }
        else { runningMultiplier = Mathf.Lerp(runningMultiplier, Utilities.BoolToInt(enabled), Time.deltaTime * stopRunningSmoothness); }
        yPointer = Mathf.Clamp01(Mathf.PingPong(time * yBobSpeed, 1));
        xPointer = Mathf.Clamp01(Mathf.PingPong(time * xBobSpeed, 1));
        if (float.IsNaN(yPointer)) { yPointer = 0f; }
        if (float.IsNaN(xPointer)) { xPointer = 0f; }
        float amountMultiplierX = 0f;
        float amountMultiplierY = 0f;
        bool didMultiplyAmount = false;
        if (amountMultipliers.Count > 0)
        {
            foreach (KeyValuePair<string, Vector2> mltp in amountMultipliers)
            {
                if (mltp.Value != Vector2.zero)
                {
                    amountMultiplierX += ((Vector2)mltp.Value).x;
                    amountMultiplierY += ((Vector2)mltp.Value).y;
                    didMultiplyAmount = true;
                }
            }
        }
        else { amountMultiplierX = 1f; amountMultiplierY = 1f; }
        if (!didMultiplyAmount) { amountMultiplierX = 1f; amountMultiplierY = 1f; }
        //print(amountMultiplierX + "    " + amountMultiplierY);
        currentYBob = Mathf.Lerp(currentYBob, y.Evaluate(yPointer) * (yBobAmount * amountMultiplierY * baseAmountMultiplier.y), Time.deltaTime * bobSmoothness);
        currentXBob = Mathf.Lerp(currentXBob, x.Evaluate(xPointer) * (xBobAmount * amountMultiplierX * baseAmountMultiplier.x), Time.deltaTime * bobSmoothness);
        //cameraRotationOffsetManager.UpdateOffset("RunBob", new Vector4(y.Evaluate(yPointer) * (yBobAmount* amountMultiplier), x.Evaluate(xPointer) * (xBobAmount* amountMultiplier), 0, 0));
        localOffsetManager.UpdateValue("RunBob", new Vector3(currentYBob, currentXBob, 0));
    }
}
