using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StaminaBar : MonoBehaviour
{
    public Slider staminaSlider;

    public float maxStamina = 100;
    private float currentStamina, regenerateStaminaTime = 0.1f, regenerateAmount = 2/*, losingStaminaTime = 0.1f*/;

    //private Coroutine myCoroutineLosing;

    private Coroutine myCoroutineRegenerate;




    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;
    }

    // Comprueba si hay suficiente estamina
    public bool HasStamina()
    {
        return currentStamina > 0;
    }

    // Reduce la estamina continuamente
    public void UseStamina(float amount)
    {
        currentStamina = Mathf.Max(0, currentStamina - amount);
        staminaSlider.value = currentStamina;

        if (currentStamina <= 0)
        {
            FindObjectOfType<FirstPerson>().isSprinting = false;
        }

        // Si se está gastando estamina, detener la regeneración
        if (myCoroutineRegenerate != null)
        {
            StopCoroutine(myCoroutineRegenerate);
            myCoroutineRegenerate = null;
        }
    }

    // Inicia la regeneración si no hay una ya en curso
    public void RegenerateStamina()
    {
        if (myCoroutineRegenerate == null)
        {
            myCoroutineRegenerate = StartCoroutine(RegenerateStaminaCoroutine());
        }
    }

    private IEnumerator RegenerateStaminaCoroutine()
    {
        yield return new WaitForSeconds(1); // Espera antes de empezar a regenerar

        while (currentStamina < maxStamina)
        {
            currentStamina = Mathf.Min(maxStamina, currentStamina + regenerateAmount);
            staminaSlider.value = currentStamina;
            yield return new WaitForSeconds(regenerateStaminaTime);
        }

        myCoroutineRegenerate = null; // Libera la corrutina cuando la estamina está llena
    }
}
