using UnityEngine;

public class MoveHorizontal : MonoBehaviour
{

    public bool movingLeft;
    public float speed;

    int directionalMultiplier;
    Vector3 newPosition;

    void Awake()
    {
        directionalMultiplier = movingLeft ? -1 : 1;
    }

    // Update is called once per frame
    void Update()
    {
        newPosition.Set(
            transform.position.x + (speed * directionalMultiplier * Time.deltaTime),
            transform.position.y,
            transform.position.z);
        transform.position = newPosition;
    }
}
