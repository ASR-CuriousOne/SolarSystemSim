using UnityEngine;

public class UILeenTween : MonoBehaviour
{
    [Header("Type of Animations")]
    public TypeOfAnimation typeOfAnimation = TypeOfAnimation.None;

    [Header("Animation Properties")]
    // Translation
    public Vector3 Start;
    public Vector3 End;
    public AnimationCurve animationCurve;

    public GameObject toggle;

    public void Animate(){
        
        
    }

    public void OpenAndClosingAnimation(bool TobeOpenedOrClosed){
        if(TobeOpenedOrClosed) {
            LeanTween.move(gameObject.GetComponent<RectTransform>(),Start,0.69f);
            LeanTween.scaleX(toggle,-1,0.69f);
            }
        else {
            LeanTween.move(gameObject.GetComponent<RectTransform>(),End,.69f);
            LeanTween.scaleX(toggle,1,0.69f);
        }
    }


}

public enum TypeOfAnimation {
    None,
    Translation,
    Rotation,
    Scale,
}
