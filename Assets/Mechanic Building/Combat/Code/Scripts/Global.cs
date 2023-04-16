namespace ΩLul{

using UnityEngine;
using UnityEngine.UI;
public static class Global
{
    public static IceMagicCore IceMagicCore;

    public static ParticleSystem FrozenHitEffect;
    public static void PlayFrozenHitParticle(Vector3 position)
    {
        FrozenHitEffect.transform.position = position;
        FrozenHitEffect.Play();
        // var emitParams = new ParticleSystem.EmitParams();
        // emitParams.
        // emitParams.position = position;
        // FrozenHitEffect.Emit(emitParams, 1);
    }
}

}
