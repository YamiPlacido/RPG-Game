using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.States
{
    public class NormalState:BaseState
    {
        private Character _char;
        private Animator _animator => _char.GetComponent<Animator>();
        private CharacterController _charController => _char.GetComponent<CharacterController>();

        public NormalState(Character character) : base(character.gameObject)
        {
            _char = character;
        }

        public override Type Tick()
        {
            if (Input.GetMouseButton(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit mouseHit;
                if (Physics.Raycast(ray, out mouseHit))
                {
                    GameObject hitObject = mouseHit.transform.gameObject;
                    if (hitObject.tag == "Enemy")
                    {
                        _char.SetTarget(hitObject.transform);
                        return typeof(ChaseState);
                    }
                }
            }

            return null;
        }
    }
}
