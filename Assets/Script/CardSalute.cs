using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSalute : MonoBehaviour
{
    [SerializeField]
    private CardDeck _deck;

    [SerializeField]
    private CardManager _manager;   

    [SerializeField]
    private float _cardPullInterval = 0.3f;

    private float _lastStart = 0;

    private bool _newDeckIsCreating;


    void Start()
    {
        _manager.DistributeCards(new List<CardDeck> { _deck });
    }

    private void CreateNewDeck()
    {
        _manager.ReInitCards(DeckType.Deck36, false);
        _manager.DistributeCards(new List<CardDeck> { _deck });
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _lastStart > _cardPullInterval)
        {
            if (_deck.isEmpty())
            {
                if (!_newDeckIsCreating)
                {
                    _newDeckIsCreating = true;
                    CreateNewDeck();
                    _newDeckIsCreating = false;
                }
                return;
            }
            _lastStart = Time.time;            
            var card = _deck.GetTopCard();
            _deck.RemoveTopCard();
            card.Open();
            card.SetGravityOn();
            var pullDirectionX = Random.Range(-500f, 500f);
            var pullDirectionY = Random.Range(200f, 700f);
            card.Pull(new Vector3(pullDirectionX, pullDirectionY, 0));
            card.Destoy(10);
        }
    }
}

