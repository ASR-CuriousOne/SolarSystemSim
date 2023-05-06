using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public TextMeshProUGUI Energy;
    public TextMeshProUGUI Momentum;
    public TextMeshProUGUI Delta_Time;
    public TextMeshProUGUI Warp_Speed;

    void Update(){
        Energy.SetText($"Energy: {NBodySimulation.Instance.Energy.ToString("0.00000")} J");
        Momentum.SetText($"Momentum: {VectorToString(NBodySimulation.Instance.Momentum)}");
        Delta_Time.SetText($"Del T: {NBodySimulation.Instance.Delta_time.ToString("0.00000")}");
        Warp_Speed.SetText($"WarpSpeed: {NBodySimulation.Instance.TimeWarp}");
    }

    string VectorToString(Vector3 input){
        return $"x: {input.x.ToString("0.000")} y: {input.y.ToString("0.000")} z: {input.z.ToString("0.000")}";
    }
}
