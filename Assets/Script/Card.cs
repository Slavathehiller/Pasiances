using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Suit
{
    hearts, 
    spades, 
    diamonds, 
    clubs
}

public enum Rank
{
    two,
    three, 
    four, 
    five, 
    six, 
    seven, 
    eight, 
    nine, 
    ten, 
    Jack, 
    Queen, 
    King, 
    Ace
}

public class Card : MonoBehaviour
{
    public Suit Suit {  get; set; }
    public Rank Rank { get; set; }

    private Rigidbody2D _rigidBody;
    public bool IsOpen => _isOpen;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private SpriteRenderer _contourSpriteRenderer;

    private Sprite _iconSprite;
    private Sprite _crapSprite;
    private bool _isOpen;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void SetIconAndCrap(Sprite iconSprite, Sprite crapSprite)
    {
        _iconSprite = iconSprite;
        _crapSprite = crapSprite;
    }

    public void Open()
    {
        _isOpen = true;
        _spriteRenderer.sprite = _iconSprite;
        _contourSpriteRenderer.enabled = true;
    }
    public void Close()
    {
        _isOpen = false;
        _spriteRenderer.sprite = _crapSprite;
        _contourSpriteRenderer.enabled = false;
    }

    public void SetOrder(int order)
    {
        _spriteRenderer.sortingOrder = order;
        _contourSpriteRenderer.sortingOrder = order + 1;
    }

    public void LiftUp()
    {
        transform.localScale = new Vector3(0.25f, 0.25f, 1);
    }

    public void PutDown()
    {
        transform.localScale = new Vector3(0.2f, 0.2f, 1);
    }

    public void MoveTo(Vector3 position, System.Action<Card> callback, bool rotate = false)
    {
        StartCoroutine(MoveCorutine(position, callback, rotate));
    }

    private IEnumerator MoveCorutine(Vector3 position, System.Action<Card> callback, bool rotate = false)
    {
        var speed = 10f;
        LiftUp();
        yield return new WaitForSeconds(0.3f);
        if (rotate)
            _rigidBody.AddTorque(1000);
        while (Vector3.Distance(transform.position, position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
            yield return null;
        }
        PutDown();
        callback(this);
        Reset();
    }

    public void Reset()
    {
        StopAllCoroutines();
        _rigidBody.angularVelocity = 0;
        transform.localRotation = Quaternion.identity;
    }
    public void SetGravityOn()
    {
        _rigidBody.gravityScale = 1;
    }

    public void Pull(Vector3 direction)
    {
        _rigidBody.AddForce(direction);
    }

    public void Destoy(float latency)
    {
        Destroy(gameObject, latency);
        Destroy(_contourSpriteRenderer.gameObject, latency);
    }

}
