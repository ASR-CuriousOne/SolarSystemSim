using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UILeenTween))]
public class UILeenTweenEditor : Editor
{
    UILeenTween uILeenTween;
    private bool m_translationFoldOutOpen;
    private bool m_rotationFoldOutOpen;
    private bool m_scaleFoldOutOpen;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if(uILeenTween.typeOfAnimation is TypeOfAnimation.Translation) DrawTranslationFields();
        if(uILeenTween.typeOfAnimation is TypeOfAnimation.Rotation) DrawRotationFields();
        if(uILeenTween.typeOfAnimation is TypeOfAnimation.Scale) DrawScaleFields();

        if(GUILayout.Button("Animate")) uILeenTween.Animate();
    }

    private void DrawTranslationFields(){

    }

    private void DrawRotationFields(){

    }

    private void DrawScaleFields(){

    }

    private void OnEnable(){
        uILeenTween = (UILeenTween)target;
    }
}
