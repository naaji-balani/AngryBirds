using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    Rigidbody2D birdRigidbody;
    bool hasCollided;
    [SerializeField] AngryBirds _angryBirdsScript;

    // Start is called before the first frame update
    void Start()
    {
        birdRigidbody = GetComponent<Rigidbody2D>();
        _angryBirdsScript = FindObjectOfType<AngryBirds>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasCollided && birdRigidbody.velocity == Vector2.zero)
        {
            Destroy(gameObject);
            _angryBirdsScript.CreateBird();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) => hasCollided = true;

}
