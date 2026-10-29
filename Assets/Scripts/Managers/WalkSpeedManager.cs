using System.Collections.Generic;

public class WalkSpeedManager : ValueMultiplierManager
{
    float defaultSpeed;
    bool setDefaultSpeed;

    private void Start()
    {
        Cache.walkSpeedManager = this;
    }
    private void Update()
    {
        if (Cache.moveData != null){
            if (!setDefaultSpeed){
                defaultSpeed = Cache.moveData.WalkFactor;
                setDefaultSpeed = true;
            }
        }

        Cache.moveData.WalkFactor = defaultSpeed;
        foreach (KeyValuePair<string, object> value in values)
        {
            Cache.moveData.WalkFactor *= System.Convert.ToSingle(value.Value);
        }
    }
}
