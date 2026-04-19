using UnityEngine;
using StarterAssets;

[RequireComponent(typeof(AudioSource))]
public class BellNoise : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private float walkVolume = 0.3f;
    [SerializeField] private float sprintVolume = 0.8f;
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
                    sprintVolume,
                    boostSpeed * Time.deltaTime
                );
            }
            else
            {
                bellAudio.volume = Mathf.MoveTowards(
                    bellAudio.volume,
                    walkVolume,
                    boostSpeed * Time.deltaTime
                );
            }
            IsRinging = true;
        }
        else
        {
            bellAudio.volume = Mathf.MoveTowards(
                bellAudio.volume,
                0f,
                boostSpeed * Time.deltaTime
            );
            if (bellAudio.volume <= 0f)
            {
                bellAudio.Stop();
                IsRinging = false;
            }
        }
    }
}