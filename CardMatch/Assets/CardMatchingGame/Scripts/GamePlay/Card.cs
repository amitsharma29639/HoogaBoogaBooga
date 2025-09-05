using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;

namespace CardGame.GamePlay
{
    /// <summary>
    /// Represents a playing card in the game, handling its visual state, interactions, and event notifications.
    /// </summary>
    [RequireComponent(typeof(ClickableObject))]
    public class Card : MonoBehaviour
    {
        #region Constants

        private const float FLIP_DURATION = 0.2f;

        #endregion

        #region Serialized Fields

        [SerializeField]
        private SpriteRenderer frontFace;

        #endregion

        #region Private Fields

        private Vector3 faceUpRotation = new Vector3(0, 0, 0);
        private Vector3 faceDownRotation = new Vector3(0, 180, 0);

        private CardData cardData;
        private CardFace currentFace;
        private List<ICardEventsListner> listeners;
        public ClickableObject clickable;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current face of the card.
        /// </summary>
        public CardFace CurrentFace
        {
            get => currentFace;
            set => currentFace = value;
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            clickable = GetComponent<ClickableObject>();
            listeners = new List<ICardEventsListner>();
        }

        private void OnEnable()
        {
            clickable.OnClick += OnCardClicked;
        }

        private void OnDisable()
        {
            clickable.OnClick -= OnCardClicked;
            NotifyCardDisableAnimationFinished();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the card with the provided data.
        /// </summary>
        public void Init(CardData data)
        {
            this.cardData = data;
            frontFace.sprite = data.Sprite;
            currentFace = CardFace.backFace;
            transform.localEulerAngles = faceDownRotation;
        }

        #endregion

        #region Listener Management

        /// <summary>
        /// Adds a listener for card events.
        /// </summary>
        public void AddListener(ICardEventsListner listener)
        {
            listeners.Add(listener);
        }

        /// <summary>
        /// Removes a listener for card events.
        /// </summary>
        public void RemoveListener(ICardEventsListner listener)
        {
            listeners.Remove(listener);
        }

        /// <summary>
        /// Clears all listeners for card events.
        /// </summary>
        public void ClearListeners()
        {
           listeners.Clear();
        }

        #endregion

        #region Card Actions

        /// <summary>
        /// Gets the card data.
        /// </summary>
        public CardData GetCardData()
        {
            return cardData;
        }

        /// <summary>
        /// Handles the card click event.
        /// </summary>
        private void OnCardClicked()
        {
            if (currentFace == CardFace.backFace)
            {
               // AudioManager.Instance.PlayOneShot(AudioGroupConstants.GAMEPLAYSFX, AudioGroupConstants.FLIP, AudioGroupConstants.GAMEPLAYSFX);
                NotifyCardClicked();
                FlipCard();
            }
        }

        /// <summary>
        /// Flips the card between face up and face down.
        /// </summary>
        public void FlipCard()
        {
            clickable.enabled = false;
            Vector3 targetRotation = currentFace == CardFace.backFace ? faceUpRotation : faceDownRotation;
            currentFace = currentFace == CardFace.backFace ? CardFace.frontFace : CardFace.backFace;
            DOTween.To(() => transform.localEulerAngles, x => transform.localEulerAngles = x, targetRotation, FLIP_DURATION).onComplete += () =>
            {
                NotifyCardFlipped();
                if (currentFace == CardFace.frontFace)
                {
                    NotifyCardFaceUpAnimationFinished();
                }
                else
                {
                    NotifyCardFaceDownAnimationFinished();
                }
                clickable.enabled = true;
            };
        }

        /// <summary>
        /// Flips the card face up for a hint, then flips it back down after a delay.
        /// </summary>
        public void FlipCardForHint()
        {
            clickable.enabled = false;
            DOTween.To(() => transform.localEulerAngles, x => transform.localEulerAngles = x, faceUpRotation, FLIP_DURATION).onComplete += async () =>
            {
                await Task.Delay(1000);
                DOTween.To(() => transform.localEulerAngles, x => transform.localEulerAngles = x, faceDownRotation, FLIP_DURATION).onComplete += () =>
                {
                    clickable.enabled = true;
                };
            };
        }

        /// <summary>
        /// Instantly sets the card to face up.
        /// </summary>
        public void SetCardFrontFacing()
        {
            NotifyCardClicked();
            clickable.enabled = false;
            transform.localEulerAngles = faceUpRotation;
            CurrentFace = CardFace.frontFace;
            NotifyCardFaceUpAnimationFinished();
            clickable.enabled = true;
        }

        #endregion

        #region Event Notification

        private void NotifyCardClicked()
        {
            foreach (var listener in listeners)
            {
                listener.OnCardClicked(this);
            }
        }

        private void NotifyCardFlipped()
        {
            foreach (var listener in listeners)
            {
                listener.OnCardFlipped(this);
            }
        }

        private void NotifyCardFaceUpAnimationFinished()
        {
            foreach (var listener in listeners)
            {
                listener.OnCardFaceUpAnimationFinished(this);
            }
        }

        private void NotifyCardFaceDownAnimationFinished()
        {
            foreach (var listener in listeners)
            {
                listener.OnCardFaceDownAnimationFinished(this);
            }
        }

        private void NotifyCardDisableAnimationFinished()
        {
            foreach (var listener in listeners)
            {
                listener.OnCardDisableAnimationFinished(this);
            }
        }

        #endregion
    }

    /// <summary>
    /// Data structure representing a card's identity and sprite.
    /// </summary>
    public struct CardData : IEquatable<CardData>
    {
        public int id;
        public string suit;
        public string rank;
        private Sprite sprite;

        public CardData(int id, string suit, string rank, SpriteAtlas atlas)
        {
            this.id = id;
            this.suit = suit;
            this.rank = rank;
            this.sprite = atlas.GetSprite("card_" + suit + "_" + rank);
        }

        public Sprite Sprite => sprite;

        public bool Equals(CardData other)
        {
            return this.id != other.id && this.suit == other.suit && this.rank == other.rank;
        }
    }

    /// <summary>
    /// Enum representing the card's face state.
    /// </summary>
    public enum CardFace
    {
        frontFace,
        backFace
    }
}