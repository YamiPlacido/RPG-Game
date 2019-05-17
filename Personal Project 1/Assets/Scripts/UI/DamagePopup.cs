using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class DamagePopup: MonoBehaviour
    {
        private const float DISAPPEAR_TIMER_MAX = 1f;
        private TextMeshPro textMesh;
        private new Camera camera;
        private float disappearTimer;
        private Color textColor;

        private void Awake()
        {
            textMesh = GetComponent<TextMeshPro>();
            camera = Camera.main;
        }

        private void Update()
        {
            //damage text face the camera
            Vector3 v = camera.transform.position - transform.position;
            v.x = v.z = 0.0f;
            transform.LookAt(camera.transform.position - v);
            transform.rotation = (camera.transform.rotation); // Take care about camera rotation
            //damage text move
            float moveSpeed = 2f;
            transform.position += new Vector3(0, moveSpeed,moveSpeed) * Time.deltaTime;
            //damage disappear
            if (disappearTimer > DISAPPEAR_TIMER_MAX * 0.5f)
            {
                //First half of popup
                float increaseScale = 0.002f;
                transform.localScale += Vector3.one * increaseScale;
            }
            else
            {
                //Second half of popup
                float decreaseScale = 0.002f;
                transform.localScale -= Vector3.one * decreaseScale;
            }

            disappearTimer -= Time.deltaTime;
            if(disappearTimer < 0)
            {
                float disappearSp = 3f;
                textColor.a -= disappearSp * Time.deltaTime;
                textMesh.color = textColor;
                if(textColor.a < 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        public static DamagePopup CreatePopupDamage(Transform pfDamagePopup, Vector3 position, int damageAmount, bool isCriticalHit)
        {
            Transform instant = Instantiate(pfDamagePopup, position, Quaternion.identity);

            DamagePopup popup = instant.GetComponent<DamagePopup>();
            popup.DamageSetup(damageAmount,isCriticalHit);

            return popup;
        }

        public void DamageSetup(int damageAmount,bool isCriticalHit)
        {
            textMesh.SetText(damageAmount.ToString());
            Color newColor;

            if (!isCriticalHit)
            {
                //Normal hit
                textMesh.fontSize = 27;
                if(ColorUtility.TryParseHtmlString("#FFC500", out newColor))
                {
                    textColor = newColor;
                }
            }
            else
            {
                //Critical hit
                textMesh.fontSize = 36;
                if (ColorUtility.TryParseHtmlString("#FF2B00", out newColor))
                {
                    textColor = newColor;
                }
            }
            textMesh.color = textColor;
            disappearTimer = DISAPPEAR_TIMER_MAX;
        }
    }
}
