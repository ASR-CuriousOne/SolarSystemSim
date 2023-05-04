using UnityEngine;


public class UILeenTween : MonoBehaviour
{
    [Header("Type of Animations")]
    public TypeOfAnimation typeOfAnimation = TypeOfAnimation.None;

    [Header("Animation Properties")]
    // Translation
    Vector3 Start;
    Vector3 End;

    void Animate(){
        
    }


}

public enum TypeOfAnimation {
    None,
    Translation,
    Rotation,
    Scale,
}
