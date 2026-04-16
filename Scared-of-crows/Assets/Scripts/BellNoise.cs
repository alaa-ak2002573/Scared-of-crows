using UnityEngine;
using StarterAssets;

[RequireComponent(typeof(AudioSource))]
public class BellNoise : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;

    [SerializeField] private float baseRunVolume = 0.2f;
    [SerializeField] private float maxRunVolume = 0.025f;
    [SerializeField] private float boostSpeed = 5f;

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

        if (isMoving)
        {
            if (!bellAudio.isPlaying)
                bellAudio.Play();

            if (isRunning)
            {
                bellAudio.volume = Mathf.MoveTowards(
                    bellAudio.volume,
                    maxRunVolume,
                    boostSpeed * Time.deltaTime
                );
                IsRinging = true;
            }
            else
            {
                bellAudio.volume = Mathf.MoveTowards(
                    bellAudio.volume,
                    baseRunVolume * 0.05f,
                    boostSpeed * Time.deltaTime
                );
                IsRinging = false;
            }
        }
        else
        {
            if (bellAudio.isPlaying)
                bellAudio.Stop();
            IsRinging = false;
        }
    }
}