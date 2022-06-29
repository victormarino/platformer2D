using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;

public class VFXManager : Singleton<VFXManager>
{
    public enum VFXType
    {
        JUMP,
        VFX2
    }

    public List<VFXManagerSetup> vfxSetup;

    public void PlayVFXByType(VFXType vFXType, Vector3 position)
    {
        foreach (var i in vfxSetup)
        {
            if (i.vfxType == vFXType)
            {
                var item = Instantiate(i.prefab);
                item.transform.position = position;
                Destroy(item.gameObject, 3f);
                break;
            }
        }
    }

}

[System.Serializable]
public class VFXManagerSetup
{
    public VFXManager.VFXType vfxType;
    public GameObject prefab;
}