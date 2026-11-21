
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MotorcycleEngineAudio : MonoBehaviour
{
    [Header("Engine Sound")]
    public AudioClip engineSound;

    [Header("Speed Settings")]
    public float maxSpeed = 8f;

    [Header("Volume")]
    public float idleVolume = 0.12f;
    public float maxVolume = 0.9f;

    [Header("Pitch")]
    public float idlePitch = 0.75f;
    public float maxPitch = 1.8f;

    [Header("Smooth Transitions")]
    public float responseSpeed = 3f;

    private AudioSource engineAudio;
    private Vector3 lastPosition;

    void Start()
    {
        engineAudio = GetComponent<AudioSource>();

        lastPosition = transform.position;

        engineAudio.clip = engineSound;
        engineAudio.loop = true;
        engineAudio.playOnAwake = false;
        engineAudio.spatialBlend = 0f;

        engineAudio.volume = idleVolume;
        engineAudio.pitch = idlePitch;

        if (engineSound != null)
        {
            engineAudio.Play();
        }
        else
        {
            Debug.LogWarning("Please assign a motorcycle engine sound!");
        }
    }

    void Update()
    {
        if (engineSound == null)
            return;

        // Measure actual motorcycle movement
        float speed = Vector3.Distance(
            transform.position,
            lastPosition
        ) / Mathf.Max(Time.deltaTime, 0.001f);

        lastPosition = transform.position;

        // Convert speed into a percentage
        float speedPercent = Mathf.Clamp01(speed / maxSpeed);

        // Calculate engine volume and pitch
        float targetVolume = Mathf.Lerp(
            idleVolume,
            maxVolume,
            speedPercent
        );

        float targetPitch = Mathf.Lerp(
            idlePitch,
            maxPitch,
            speedPercent
        );

        // Smoothly change engine sound
        engineAudio.volume = Mathf.Lerp(
            engineAudio.volume,
            targetVolume,
            Time.deltaTime * responseSpeed
        );

        engineAudio.pitch = Mathf.Lerp(
            engineAudio.pitch,
            targetPitch,
            Time.deltaTime * responseSpeed
        );
    }
}
