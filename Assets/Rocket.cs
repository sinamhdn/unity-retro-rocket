using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] AudioClip engineSound;
    [SerializeField] AudioClip victorySound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] ParticleSystem engineParticle;
    [SerializeField] ParticleSystem victoryParticle;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] float rocketMass = 0.1f;
    [SerializeField] float frameRotationSpeed = 100f; // Reaction control system Thrust
    [SerializeField] float throttleSpeed = 100f;
    [SerializeField] float levelLoadDelay = 1f;
    Rigidbody rigidbody;
    AudioSource audioSource;
    bool collisionDetectionEnabled = true;

    enum State { Alive, Dying, Leveling }
    State state;
    // State state = State.Alive; also works

    // Start is called before the first frame update
    void Start()
    {
        state = State.Alive;
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // because when invoke waits 1 second update atill runs
        if (state == State.Alive)
        {
            Throttle();
            Rotate();
        }

        if (Debug.isDebugBuild) DebugKeys();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || !collisionDetectionEnabled) return;

        switch (collision.gameObject.tag)
        {
            case "Friend":
                break;
            case "Finish":
                state = State.Leveling;
                audioSource.Stop();
                audioSource.PlayOneShot(victorySound);
                victoryParticle.Play();
                Invoke("LoadNextScene", levelLoadDelay);
                break;
            default:
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(deathSound);
                deathParticle.Play();
                Invoke("LoadFirstLevel", levelLoadDelay);
                break;
        }
    }

    void Throttle()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.mass = rocketMass;
            rigidbody.AddRelativeForce(Vector3.up * throttleSpeed * Time.deltaTime);
            // we add if because when we hold space it will try to play again and again
            // plays default clip on the audio source
            // if (!audioSource.isPlaying) audioSource.Play();
            // plays the audio clip passed to the function
            engineParticle.Play();
            if (!audioSource.isPlaying) audioSource.PlayOneShot(engineSound);
        }
        else
        {
            audioSource.Stop();
            engineParticle.Stop();
        }
    }

    void Rotate()
    {
        // freezing rotation make the game object fall slowly
        // rigidbody.freezeRotation = true;
        rigidbody.angularVelocity = Vector3.zero; // remove physics rotation

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * frameRotationSpeed * Time.deltaTime);
            return;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * frameRotationSpeed * Time.deltaTime);
            return;
        }

        // resumes physics control of the rotation
        // rigidbody.freezeRotation = false;
    }


    void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

    void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L)) LoadNextScene();
        if (Input.GetKeyDown(KeyCode.C)) ToggleCollisionDetection();
    }

    void ToggleCollisionDetection()
    {
        collisionDetectionEnabled = !collisionDetectionEnabled;
    }
}
