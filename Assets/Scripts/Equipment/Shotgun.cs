using UnityEngine;

public class Shotgun : Firearm
{
    [Header("Shotgun")]
    public int shots;

    [Header("Shotgun Camera Fixed Recoil")]
    public Vector3 shotgunCamFixedRecoilA;
    public Vector3 shotgunCamFixedRecoilB;
    public Vector3[] shotgunCamFixedRecoils;
    public int shotgunCamFixedRecoilSeed;
    public override void Start()
    {
        base.Start();
        GenerateShotgunCamFixedRecoil();
    }
    public void GenerateShotgunCamFixedRecoil()
    {
        System.Random rand = new System.Random(shotgunCamFixedRecoilSeed);
        shotgunCamFixedRecoils = new Vector3[ammoClipSize];
        for (int i = 0; i < shotgunCamFixedRecoils.Length; i++)
        {
            shotgunCamFixedRecoils[i] = new Vector3(Utilities.NextFloat(rand, shotgunCamFixedRecoilA.x, shotgunCamFixedRecoilB.x), Utilities.NextFloat(rand, shotgunCamFixedRecoilA.y, shotgunCamFixedRecoilB.y), Utilities.NextFloat(rand, shotgunCamFixedRecoilA.z, shotgunCamFixedRecoilB.z));
        }
    }
    public override void OnMainAction()
    {
        if (currentClipAmount > 0)
        {
            if (currentReloadTime < (reloadTime - reloadDisableShootTime))
            {
                for (int i = 0; i < shots; i++)
                {
                    base.OnMainAction();
                    targetCamRecoil += shotgunCamFixedRecoils[fixedRecoilPointer];
                    if (currentClipAmount <= 0) { break; }
                }
            }
        }
        else { NoAmmoShoot(); }
    }
}
