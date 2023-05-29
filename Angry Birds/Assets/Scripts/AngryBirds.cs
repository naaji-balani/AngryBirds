using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryBirds : MonoBehaviour
{
    
    [SerializeField] LineRenderer[] _lineRenderers;
    [SerializeField] Transform[] _stripPositions;
    [SerializeField] Transform _center;
    [SerializeField] Transform idlePosition;

    [SerializeField] Vector3 _currentPosition;

    [SerializeField] float _maxLength;

    [SerializeField] float bottomBoundary;

    [SerializeField] bool isMouseDown;

    [SerializeField] GameObject[] birdPrefabs;

    [SerializeField] float birdPositionOffset;

    Rigidbody2D bird;
    Collider2D birdCollider;

    [SerializeField] float force;

    [SerializeField] int _birdCount;

    void Start()
    {
        _lineRenderers[0].positionCount = 2;
        _lineRenderers[1].positionCount = 2;
        _lineRenderers[0].SetPosition(0, _stripPositions[0].position);
        _lineRenderers[1].SetPosition(0, _stripPositions[1].position);

        CreateBird();
    }

    public void CreateBird()
    {
        if (_birdCount > 1) return;

        bird = Instantiate(birdPrefabs[_birdCount]).GetComponent<Rigidbody2D>();
        birdCollider = bird.GetComponent<Collider2D>();
        birdCollider.enabled = false;

        bird.isKinematic = true;

        ResetStrips();
    }

    void Update()
    {
        if (isMouseDown)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            _currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            _currentPosition = _center.position + Vector3.ClampMagnitude(_currentPosition
                - _center.position, _maxLength);

            _currentPosition = ClampBoundary(_currentPosition);

            SetStrips(_currentPosition);

            if (birdCollider)
            {
                birdCollider.enabled = true;
            }
        }
        else
        {
            ResetStrips();

        }

        if(_birdCount == 2 && Input.GetMouseButtonDown(0) && !isMouseDown)
        {
            FindObjectOfType<Bird>().GetComponent<Rigidbody2D>().velocity *= 4;
        }


    }

    private void OnMouseDown()
    {
        isMouseDown = true;

        _birdCount++;
    }

    private void OnMouseUp()
    {
        isMouseDown = false;
        Shoot();
        _currentPosition = idlePosition.position;
    }

    void Shoot()
    {
        bird.isKinematic = false;
        Vector3 birdForce = (_currentPosition - _center.position) * force * -1;
        bird.velocity = birdForce;

        bird = null;
        birdCollider = null;
        //Invoke("CreateBird", 2);
    }

    void ResetStrips()
    {
        _currentPosition = idlePosition.position;
        SetStrips(_currentPosition);
    }

    void SetStrips(Vector3 position)
    {
        _lineRenderers[0].SetPosition(1, position);
        _lineRenderers[1].SetPosition(1, position);

        if (bird)
        {
            Vector3 dir = position - _center.position;
            bird.transform.position = position + dir.normalized * birdPositionOffset;
            bird.transform.right = -dir.normalized;
        }
    }

    Vector3 ClampBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 1000);
        return vector;
    }
}