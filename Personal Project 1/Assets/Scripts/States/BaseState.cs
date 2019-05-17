using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.States
{
    public abstract class BaseState
    {
        protected GameObject gameObject;
        protected Transform transform;

        public BaseState(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.transform = gameObject.transform;
        }

        public abstract Type Tick();
    }
}
