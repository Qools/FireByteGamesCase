using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SaberController : MonoBehaviour
{
    [Header("Light Saber Slider")]
    [SerializeField] private Slider lightSaberSlider;

    [Header("Min and Max Slider Rotation Values")]
    [SerializeField] private float minRotationVal;
    [SerializeField] private float maxRotationVal;

    [Header("Light Saber")]
    [SerializeField] private GameObject lightSaber;

    [Header("Opponent Light Saber Tag")]
    [SerializeField] private string opponentSaberTag;

    [Header("Hit Effect")]
    [SerializeField] private GameObject hitEffect;

    [Header("Light Saber Hit Timer")]
    [SerializeField] private float hitTimer;

    [Header("Light Sabre Layer")]
    [SerializeField] private LayerMask sabreLayer;
    [Header("Ray Start Point")]
    [SerializeField] private Transform rayCastPoint;

    [Header("Hit Shake Effect Strength")]
    [SerializeField] private Vector3 shakeStrength;
    [Header("Hit Shake Effect Lenght")]
    [SerializeField] private float shakeLenght;

    [Header("Sound Effects (Idle, Slash, Hit)")]
    [SerializeField] private AudioClip idleFx, slashFx, hitFx;

    public bool isHit = false;

    private Rigidbody rb;

    private AudioSource audioSource;

    private Vector3 tempRotation;

    [HideInInspector] public bool isButtonPressed = false;

    private Vector3 startRotation;

    // Start is called before the first frame update
    void Awake() => SetupGame();


    // Update is called once per frame
    void Update()
    {
        if(!isButtonPressed)
            RotateSaber();   
    }

    private void FixedUpdate() => CheckIfSabreHits();


    private void SetupGame()
    {
        lightSaberSlider.minValue = minRotationVal;
        lightSaberSlider.maxValue = maxRotationVal;

        hitEffect.SetActive(false);

        startRotation = lightSaber.transform.rotation.eulerAngles;

        lightSaberSlider.value = startRotation.z;

        if (GetComponent<Rigidbody>())
            rb = GetComponent<Rigidbody>();

        if (GetComponent<AudioSource>())
            audioSource = GetComponent<AudioSource>();

        audioSource.clip = idleFx;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void RotateSaber()
    {
        tempRotation = new Vector3(0f, lightSaber.transform.eulerAngles.y, lightSaberSlider.value);

        lightSaber.transform.DOLocalRotate(tempRotation, 0.1f).SetEase(Ease.Linear);
    }

    private void CheckIfSabreHits()
    {
        Vector3 direction = rayCastPoint.TransformDirection(Vector3.up);

        if (Physics.Raycast(rayCastPoint.position, direction, out RaycastHit hit, sabreLayer))
        {
            isHit = true;
        }
        else
        {
            isHit = false;
        }
    } 

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(opponentSaberTag) && isButtonPressed)
        {
            hitEffect.SetActive(true);

            lightSaber.transform.DOShakePosition(shakeLenght, shakeStrength);

            audioSource.clip = hitFx;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    public void HitSabre()
    {
        lightSaber.transform.DOLocalRotate(
            new Vector3(0f, lightSaber.transform.eulerAngles.y, 0f),
            hitTimer).SetEase(Ease.Linear).OnComplete(() =>
            {
                audioSource.clip = slashFx;
                audioSource.loop = false;
                audioSource.Play();

                rb.isKinematic = true;

                lightSaber.transform.DOLocalRotate(tempRotation, hitTimer * .25f).SetEase(Ease.Linear);

                DOVirtual.DelayedCall(1f, () => ResetSabers());
            });
    }

    private void ResetSabers()
    {
        lightSaber.transform.DOLocalRotate(startRotation, hitTimer).SetEase(Ease.Linear);

        rb.isKinematic = false;
        hitEffect.SetActive(false);

        isButtonPressed = false;

        audioSource.clip = idleFx;
        audioSource.loop = true;
        audioSource.Play();
    }
}
