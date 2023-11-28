using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField][Range(0, 1)] float movementFactor;
    [SerializeField] float period = 2f;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) return;
        float cycles = Time.time / period; // grows continually from 0
        const float tau = Mathf.PI * 2;
        float rawSineWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSineWave / 2f + 0.5f;
        Vector3 displacement = movementFactor * movementVector;
        transform.position = startingPos + displacement;
    }
}
