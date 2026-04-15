using UnityEngine;
using StarterAssets;

[RequireComponent(typeof(AudioSource))]
public class BellNoise : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;

    [SerializeField] private float baseRunVolume = 0.7f;
    [SerializeField] private float maxRunVolume = 1f;
    [SerializeField] private float boostSpeed = 2f;

    private AudioSource bellAudio;

    public bool IsRinging { get; private set; }

    private void Awake()
    {
        bellAudio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (starterAssetsInputs == null)
            starterAssetsInputs = GetComponent<StarterAssetsInputs>();

        bellAudio.playOnAwake = false;
        bellAudio.loop = true;
        bellAudio.volume = 0f;
        bellAudio.Stop();
    }

    private void Update()
    {
        bool isMoving = starterAssetsInputs.move.sqrMagnitude > 0.01f;
        bool isRunning = starterAssetsInputs.sprint;

        if (isMoving && isRunning)
        {
            if (!bellAudio.isPlaying)
            {
                bellAudio.volume = baseRunVolume;
                bellAudio.Play();
            }

            bellAudio.volume = Mathf.MoveTowards(
                bellAudio.volume,
                maxRunVolume,
                boostSpeed * Time.deltaTime
            );

            IsRinging = true;
        }
        else
        {
            if (bellAudio.isPlaying)
                bellAudio.Stop();

            IsRinging = false;
        }
    }
}