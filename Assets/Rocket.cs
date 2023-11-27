using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rocketMass = 0.1f;
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

    void Throttle()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.mass = rocketMass;
            rigidbody.AddRelativeForce(Vector3.up);
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
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * Time.deltaTime);
            return;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * Time.deltaTime);
            return;
        }
    }
}
