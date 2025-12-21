using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeelRoutine : MonoBehaviour
{
    [SerializeField]
    private CardManager _cardManager;

    [SerializeField]
    private List<CardDeck> _weelDecks;

    [SerializeField]
    private CardDeck _resultDeck1;
    [SerializeField]
    private CardDeck _resultDeck2;

    [SerializeField]
    private GameObject _victoryWindow;

    [SerializeField]
    private GameObject _loseWindow;


    private CardDeck _selectDeck;

    private float _weelRadius = 3f;
    void Start()
    {
        ArrangeDecks();
        StartNew();
        foreach (var deck in _weelDecks)
            deck.OnClick += OnDeckClick;
    }

    private void StartNew()
    {
        _resultDeck1.DropAll();
        _resultDeck2.DropAll();
        _cardManager.Shaffle();
        _cardManager.DistributeCards(_weelDecks);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ArrangeDecks()
    {
        if (_weelDecks.Count == 0)
            return;
        float angleStep = 2 * Mathf.PI / _weelDecks.Count;

        var center = new Vector3(1, 0 , 0);
        for (int i = 0; i < _weelDecks.Count; i++)
        {
            float angle = Mathf.PI / 2 - i * angleStep;

            float x = center.x + _weelRadius * Mathf.Cos(angle);
            float y = center.y + _weelRadius * Mathf.Sin(angle);

            _weelDecks[i].transform.position = new Vector3(x, y, center.z);
        }
    }

    private void OnDeckClick(CardDeck deck)
    {
        if (deck.isEmpty())
            return;
        if (deck.GetTopCard().IsOpen)
        {
            deck.GetTopCard().LiftUp();
            if (_selectDeck == null)
            {
                _selectDeck = deck;
            }
            else
            {
                if (_selectDeck != deck && _selectDeck.GetTopCard().Rank == deck.GetTopCard().Rank)
                {
                    _cardManager.MoveCardToDeck(_selectDeck, _resultDeck1, (card) => { _resultDeck1.LayOnTop(card); CheckBoth(); });
                    _cardManager.MoveCardToDeck(deck, _resultDeck2, (card) => { _resultDeck2.LayOnTop(card); CheckBoth(); });
                    _selectDeck = null;
                }
                else
                {
                    _selectDeck.GetTopCard().PutDown();
                    deck.GetTopCard().LiftUp();
                    _selectDeck = deck;
                }
            }
        }
        else
        {
            deck.OpenTopCard();
            CheckLose();
        }        
                    
        //var decks = _weelDecks.Where(x => !x.isEmpty() && x.GetTopCard().IsOpen).OrderBy(x => x.transform.position.x);
        //var decksWithDoubles = decks
        //        .GroupBy(x => x.GetTopCard().Rank)
        //        .Where(g => g.Count() > 1)
        //        .SelectMany(g => g)
        //        .ToList();
        //if (decksWithDoubles.Any())
        //{
        //    _cardManager.MoveCardToDeck(decksWithDoubles[0], _resultDeck1, (card) => { _resultDeck1.LayOnTop(card); CheckBoth(); });
        //    _cardManager.MoveCardToDeck(decksWithDoubles[1], _resultDeck2, (card) => { _resultDeck2.LayOnTop(card); CheckBoth(); });
        //}
        //else
        //    CheckLose();
    }


    private bool CheckWin()
    {

        if (!_weelDecks.Any(x => !x.isEmpty()))
        {
            _victoryWindow.SetActive(true);
            return true;
        }
        return false;
    }

    private void CheckLose()
    {
        if (!_weelDecks.Any(x => !x.isEmpty() && !x.GetTopCard().IsOpen)
            && !_weelDecks.Where(x => !x.isEmpty()).GroupBy(x => x.GetTopCard().Rank)
                                        .Where(g => g.Count() > 1)
                                        .SelectMany(g => g)
                                        .ToList()
                                        .Any()
            )
        {
            _loseWindow.SetActive(true);
        }
    }

    private void CheckBoth()
    {
        if (!CheckWin())
            CheckLose();
    }

    public void OnRestartButtonClick()
    {
        _victoryWindow.SetActive(false);
        _loseWindow.SetActive(false);
        StartNew();
    }

    public void OnCloseButtonClick()
    {
        SceneManager.LoadScene(Scenes.MAIN_MENU);
    }
}
