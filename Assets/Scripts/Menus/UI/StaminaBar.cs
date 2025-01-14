using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StaminaBar : MonoBehaviour
{
    public Slider staminaSlider;

    public float maxStamina = 100;
    private float currentStamina, regenerateStaminaTime = 0.1f, regenerateAmount = 2, losingStaminaTime = 0.1f;

    private Coroutine myCoroutineLosing;

    private Coroutine myCoroutineRegenerate;

    
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;
        
    }

    public void UseStamina(float amount)
    {
        if(currentStamina -amount > 0)
        {
            if(myCoroutineLosing != null)
            {
                StopCoroutine(myCoroutineLosing);
            }

            myCoroutineLosing = StartCoroutine(LosingStaminaCoroutine(amount));

            if (myCoroutineRegenerate != null)
            {
                StopCoroutine(myCoroutineRegenerate);
            }
            myCoroutineRegenerate = StartCoroutine(RegenerateStaminaCoroutine());
        }
        else
        {
            Debug.Log("Sin stamina");

            FindObjectOfType<FirstPerson>().isSprinting = false;
        }
    }
    private IEnumerator LosingStaminaCoroutine(float amount)
    {
        while (currentStamina>=0)
        {
            currentStamina -= amount;

            staminaSlider.value = currentStamina;

            yield return new WaitForSeconds(losingStaminaTime);
        }

        myCoroutineLosing = null;
        FindObjectOfType<FirstPerson>().isSprinting = false;


    }

    private IEnumerator RegenerateStaminaCoroutine()
    {
        yield return new WaitForSeconds(1);

        while (currentStamina < maxStamina)
        {
            currentStamina += regenerateAmount;

            staminaSlider.value = currentStamina;

            yield return new WaitForSeconds(regenerateStaminaTime);
        }
        myCoroutineRegenerate = null;
    }
}
