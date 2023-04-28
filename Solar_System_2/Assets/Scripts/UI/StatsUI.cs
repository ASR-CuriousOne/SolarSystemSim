using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public Universe universe;
    public TextMeshProUGUI Energy;
    public TextMeshProUGUI Momentum;

    void Update(){
        Energy.SetText($"Energy: {universe.Energy.ToString("0.00000")} J");
        Momentum.SetText($"Momentum: {VectorToString(universe.Momentum)}");
    }

    string VectorToString(Vector3 input){
        return $"x: {input.x.ToString("0.000")} y: {input.y.ToString("0.000")} z: {input.z.ToString("0.000")}";
    }
}
