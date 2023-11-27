using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rocketMass = 0.1f;
    [SerializeField] float frameRotationSpeed = 100f; // Reaction control system Thrust
    [SerializeField] float throttleSpeed = 100f;
    Rigidbody rigidbody;
    AudioSource audioSource;

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
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) return;

        switch (collision.gameObject.tag)
        {
            case "Friend":
                break;
            case "Finish":
                state = State.Leveling;
                Invoke("LoadNextScene", 1f);
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f);
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
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    void Rotate()
    {
        rigidbody.freezeRotation = true;

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
        rigidbody.freezeRotation = false;
    }


    void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }
}
