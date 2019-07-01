using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class HitEffect: MonoBehaviour
    {
        private ParticleSystem thisParticleSystem;

        private void Start()
        {
            thisParticleSystem = this.GetComponent<ParticleSystem>();

            if (!thisParticleSystem.main.loop)
            {
                Destroy(this.gameObject, thisParticleSystem.main.duration - 3);
            }
        }
    }
}
