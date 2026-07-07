using Core;
using UnityEngine;

namespace Character.Player
{
    public class PlayerView : MonoBehaviour
    {
        public void PlayFootstepSound()
        {
            AudioManager.Instance.PlaySfx("PlayerStep",transform.position,.5f, Random.Range(.5f,1f));
        }

        public void SpawnStepVfx()
        {
            VfxManager.Instance.SpawnFromPool("DustParticle", transform.position, .7f);
        }
    }
}
