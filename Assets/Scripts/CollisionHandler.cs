using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    // PARAMETERS
    [SerializeField] float reloadDelay = 1f;
    [SerializeField] float loadDelay = 1f;

    // CACHE
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip successSFX;
    [SerializeField] ParticleSystem crushParticles;
    [SerializeField] ParticleSystem successParticles;
    AudioSource audioSource;

    // STATE
    bool isTransitioning = false;
    bool collisonDisable = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            collisonDisable = !collisonDisable; // toggle collison
            Debug.Log("Collision disable state is: " + collisonDisable);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisonDisable) { return; }

        switch (other.gameObject.tag)
        {
            case "Finish":
                StartLoadingSequence();
                break;
            case "Friendly":
                Debug.Log("Friendly thing");
                break;
            default:
                StartCrashingSequence();
                break;
        }
    }

    void StartCrashingSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSFX);
        crushParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", reloadDelay);
    }

    void StartLoadingSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(successSFX);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", loadDelay);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);

    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}