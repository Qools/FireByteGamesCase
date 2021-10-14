using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Screen Text Object (TextMeshPro)")]
    [SerializeField] private TextMeshPro screenText;

    [Header("Screen Texts")]
    [SerializeField] private string isCollisionText;
    [SerializeField] private string notCollisionText;

    [Header("Light Sabers")]
    [SerializeField] private SaberController redSaber;
    [SerializeField] private SaberController blueSaber;

    [Header("Collision Angles")]
    [SerializeField] private float minCollisionAngle;
    [SerializeField] private float maxCollisionAngle;


    // Update is called once per frame
    void Update() => CheckSabreCollision();

    private void CheckSabreCollision()
    {
        if (redSaber.isHit && blueSaber.isHit)
            screenText.text = isCollisionText;

        else
            screenText.text = notCollisionText;
    }

    public void SlashSabers()
    {
        redSaber.isButtonPressed = true;
        redSaber.HitSabre();

        blueSaber.isButtonPressed = true;
        blueSaber.HitSabre();
    }
}
