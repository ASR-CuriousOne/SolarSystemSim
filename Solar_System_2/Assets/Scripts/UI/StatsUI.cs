using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public TextMeshProUGUI Energy;
    public TextMeshProUGUI Momentum;
    public TextMeshProUGUI Delta_Time;
    public TextMeshProUGUI Warp_Speed;
    public TextMeshProUGUI FPS_Counter;

    void LateUpdate(){
        Energy.SetText($"Energy: {NBodySimulation.Instance.Energy.ToString("0.00000")} J");
        Momentum.SetText($"Momentum: {VectorToString(NBodySimulation.Instance.Momentum)}");
        Delta_Time.SetText($"Del T: {NBodySimulation.Instance.Delta_time.ToString("0.00000")}");
        Warp_Speed.SetText($"WarpSpeed: {NBodySimulation.Instance.TimeWarp}");
        FPS_Counter.SetText($"FPS: {(1.0f/Time.smoothDeltaTime).ToString("0.00")}");
    }

    string VectorToString(Vector3 input){
        return $"x: {input.x.ToString("0.000")} y: {input.y.ToString("0.000")} z: {input.z.ToString("0.000")}";
    }
}
