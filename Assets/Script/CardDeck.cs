using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Script
{
    public class CardDeck : MonoBehaviour
    {
        [SerializeField]
        private BoxCollider2D _collider;

        private Vector3 _offset = new Vector3(0.015f, 0.015f, 0);

        private List<Card> _cards = new();

        public event UnityAction<CardDeck> OnClick;

        public int CardNumber => _cards.Count;
        public bool isEmpty()
        {
            return _cards.Count == 0;
        }
        public Card GetTopCard()
        {
            if (isEmpty())
                Debug.LogError($"Deck is empty, cant't get top card");
            return _cards[0];
        }

        public Vector3 NextCardPosition()
        {
            if (_cards.Count == 0)
                return transform.position;
            else
                return _cards[0].transform.position + _offset;
        }
        public void LayOnTop(Card card)
        {
            card.transform.SetParent(transform, false);

            var cardPosition = Vector3.zero;
            if (_cards.Count > 0)            
                cardPosition = _cards[0].transform.localPosition + _offset;          

            card.transform.localPosition = cardPosition;
            card.SetOrder(_cards.Count);
            _cards.Insert(0, card);
        }

        //public void LayToBottom(Card card)
        //{
        //    _cards.Add(card);
        //}

        public void OnMouseDown()
        {
            OnClick?.Invoke(this);
        }

        public void OpenTopCard()
        {
            if (_cards.Count > 0)
                _cards[0].Open();
        }

        public void RemoveTopCard()
        {
            if ( _cards.Count > 0)
                _cards.RemoveAt(0);
        }

        public void DropAll()
        {
            _cards.Clear();
        }

    }
}
