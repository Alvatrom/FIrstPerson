using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Caracteristicas Arma")]

public class ArmaSO : ScriptableObject
{
    //Datos
    public int balasCargador, balasBolsa;
    public float cadenciaAtaque, distanciaAtaque, danhoAtaque;
}
