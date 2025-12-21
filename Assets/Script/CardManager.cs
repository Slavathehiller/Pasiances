using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Assets.Script
{

    [System.Serializable]
    public struct CardType
    {
        public Suit Suit;
        public Rank Rank;
        public Sprite Image;
    }

    public enum DeckType
    {
        Deck36,
        Deck52
    }

    public class CardManager : MonoBehaviour 
    {
        [SerializeField]
        private CardType[] CardTypes;

        [SerializeField]
        private Sprite _crapImage;

        [SerializeField]
        private DeckType _deckType;

        private List<Card> _deck = new();

        private Sprite GetCardIcon(Suit suit, Rank rank)
        {
            CardType? cardIconRecord = CardTypes.FirstOrDefault(x => x.Suit == suit && x.Rank == rank);
            if (cardIconRecord != null)
            {
                return cardIconRecord.Value.Image;
            }
            else
            {
                Debug.LogError($"Icon not found for {suit} {rank}");
                return null;
            }
        }
        private void Awake()
        {
            InitCards();
        }

        public void ReInitCards(DeckType deckType, bool destroyOldCards = true)
        {
            if (destroyOldCards)
                for (int i = _deck.Count - 1; i >= 0; i--)
                    Destroy(_deck[i].gameObject);
            _deck.Clear();

            _deckType = deckType;
            InitCards();
        }

        private void InitCards()
        {
            CardType[] currentCardTypes;
            if (_deckType == DeckType.Deck36)
                currentCardTypes = CardTypes.Where(x => x.Rank > Rank.five).ToArray();
            else
                currentCardTypes = CardTypes.ToArray();

            // foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            foreach (CardType cardType in currentCardTypes)
            {
                Card card = CardFactory.CreateCard(cardType.Suit, cardType.Rank, GetCardIcon(cardType.Suit, cardType.Rank), _crapImage);
                _deck.Add(card);
            }
            Shaffle();
        }

        public void DistributeCards(List<CardDeck> decks)
        {
            foreach (CardDeck deck in decks)
                deck.DropAll();
            var i = 0;
            do
            {
                foreach (CardDeck deck in decks)
                {
                    var card = _deck[i++];
                    card.Reset();
                    card.Close();
                    deck.LayOnTop(card);
                    if (i >= _deck.Count)
                        break;
                }
            }
            while (i < _deck.Count);
        }

        public void Shaffle()
        {
            var newDeck = new List<Card>();
            do
            {
                var cardIndex = Random.Range(0, _deck.Count);
                newDeck.Add(_deck[cardIndex]);
                _deck.RemoveAt(cardIndex);
            }
            while (_deck.Count > 0);
            _deck = newDeck;
        }

        public void MoveCardToDeck(CardDeck giver, CardDeck reciever, Action<Card> callback)
        {
            var card = giver.GetTopCard();
            giver.RemoveTopCard();
            card.SetOrder(100);
            card.MoveTo(reciever.NextCardPosition(), callback, true);
        }

    }
}
