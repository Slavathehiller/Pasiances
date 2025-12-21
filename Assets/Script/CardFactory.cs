using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script
{
    public static class CardFactory
    {
        private static GameObject _cardPrefab;

        static CardFactory()
        {
            _cardPrefab = Resources.Load("Card") as GameObject;
        }
        public static Card CreateCard(Suit suit, Rank rank, Sprite IconSprite, Sprite crapSprite)
        {
            var instance = GameObject.Instantiate(_cardPrefab);
            var card = instance.GetComponent<Card>();
            card.Suit = suit;
            card.Rank = rank;
            card.SetIconAndCrap(IconSprite, crapSprite);
            //card.gameObject.SetActive(false);
            return card;
        }
    }
}
