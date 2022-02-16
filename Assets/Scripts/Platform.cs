using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _multiplier;
    [SerializeField] private float _speed;
    [SerializeField] private float _boundsX;

    private string _platformType;
    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _platformType = Constants.TYPES[Random.Range(0,Constants.TYPES.Count)];
        switch(_platformType)
        {
            case Constants.NORMAL:
                _sr.color = Color.white;
                break;
            case Constants.JUMPING:
                _sr.color = Color.red;
                _jumpForce *= 1.5f;
                break;
            case Constants.MOVEABLE:
                _sr.color = Color.blue;
                StartCoroutine(MovePlatform());
                break;
            case Constants.MOVEABLE_JUMPING:
                _sr.color = Color.magenta;
                StartCoroutine(MovePlatform());
                _jumpForce *= 1.5f;
                break;
            case Constants.BREAKABLE:
                _sr.color = Color.black;
                break;
            case Constants.SINGLE:
                _sr.color = Color.grey;
                break;
        }
    }

    IEnumerator MovePlatform()
    {
        while(true)
        {
            Vector3 temp = transform.position;
            if (temp.x > _boundsX || temp.x < -_boundsX)
                _speed *= -1f;
            Vector3 offset =  Vector3.right * _speed*Time.deltaTime;
            transform.Translate(offset);
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (playerRb.velocity.y > 0f) return;
        Camera.main.GetComponent<CameraFollow>().UpdateCamera();
        GameManager.instance.UpdateScore((int)(transform.position.y * _multiplier));
        switch (_platformType)
        {
            case Constants.NORMAL:
                playerRb.velocity = new Vector2(playerRb.velocity.x,_jumpForce);
                break;
            case Constants.JUMPING:
                playerRb.velocity = new Vector2(playerRb.velocity.x, _jumpForce);
                break;
            case Constants.MOVEABLE:
                playerRb.velocity = new Vector2(playerRb.velocity.x, _jumpForce);
                break;
            case Constants.MOVEABLE_JUMPING:
                playerRb.velocity = new Vector2(playerRb.velocity.x, _jumpForce);
                break;
            case Constants.BREAKABLE:
                FindObjectOfType<Spawner>().SpawnPlatform();
                Destroy(gameObject);
                break;
            case Constants.SINGLE:
                playerRb.velocity = new Vector2(playerRb.velocity.x, _jumpForce);
                FindObjectOfType<Spawner>().SpawnPlatform();
                Destroy(gameObject);
                break;
        }
    }
}
