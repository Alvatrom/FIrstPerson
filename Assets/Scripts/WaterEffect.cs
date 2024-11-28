using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WaterEffect : MonoBehaviour
{

    private Volume volume;
    private LensDistortion distorsionEffect;
    private ColorAdjustments colorAdjustments;
    private float velocidad;
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();
        if (volume.profile.TryGet(out LensDistortion lensDistorsion))
        {
            distorsionEffect = lensDistorsion;
        }
        if (volume.profile.TryGet(out ColorAdjustments adjustments))
        {
            colorAdjustments = adjustments;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // metenmos el seno y cose en el efecto del agua y sumamos 1 para que vaya de 
        //0 a 2 y dividimos entre 2 para que vaya de 0 a 1.
        FloatParameter ejemplo = new FloatParameter(1 +Mathf.Sin(velocidad *Time.time) / 2);
        FloatParameter ejemplo2 = new FloatParameter(1+Mathf.Cos(velocidad*Time.time)/2);
        distorsionEffect.xMultiplier.SetValue(ejemplo);
        distorsionEffect.yMultiplier.SetValue(ejemplo2);

    }
}
