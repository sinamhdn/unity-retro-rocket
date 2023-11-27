using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rocketMass = 0.1f;
    [SerializeField] float frameRotationSpeed = 100f; // Reaction control system Thrust
    [SerializeField] float throttleSpeed = 100f;
    Rigidbody rigidbody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Throttle();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friend":
                break;
            case "Finish":
                SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
                break;
            default:
                SceneManager.LoadScene(0);
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
}
