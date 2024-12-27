using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CoinAnimationPackage
{
    /// <summary>
    /// Manages coin animation effects, including scaling and movement, with support for RectTransform targets.
    /// </summary>
    public class AnimationManager : MonoBehaviour
    {
        [Header("Animation References")]
        [SerializeField] private GameObject coinPile; // Parent GameObject holding coin prefabs
        [SerializeField] private TextMeshProUGUI counter; // Optional: Counter to update coin count
        [SerializeField] private RectTransform targetRectTransform; // The target RectTransform (e.g., UI element)

        [Header("Animation Settings")]
        [SerializeField] private float scaleUpDuration = 0.3f; // Duration to scale up coins
        [SerializeField] private float moveDuration = 1f; // Duration to move coins
        [SerializeField] private float scaleDownDuration = 0.3f; // Duration to scale down coins
        [SerializeField] private float moveDelay = 0.5f; // Delay before moving coins
        [SerializeField] private float scaleDownDelay = 0.8f; // Delay before scaling down coins
        [SerializeField] private float staggerDelay = 0.1f; // Delay between animations of each coin

        private List<Vector3> initialPositions = new List<Vector3>();
        private List<Quaternion> initialRotations = new List<Quaternion>();

        private void Start()
        {
            // Store initial positions and rotations
            for (int i = 0; i < coinPile.transform.childCount; i++)
            {
                initialPositions.Add(coinPile.transform.GetChild(i).position);
                initialRotations.Add(coinPile.transform.GetChild(i).rotation);
            }
        }

        /// <summary>
        /// Resets the coin pile to its initial positions and rotations.
        /// </summary>
        private void ResetPile()
        {
            for (int i = 0; i < coinPile.transform.childCount; i++)
            {
                coinPile.transform.GetChild(i).position = initialPositions[i];
                coinPile.transform.GetChild(i).rotation = initialRotations[i];
            }
        }

        /// <summary>
        /// Converts a RectTransform position to world position.
        /// </summary>
        /// <returns>World position of the RectTransform.</returns>
        private Vector3 GetTargetWorldPosition()
        {
            if (targetRectTransform == null)
            {
                Debug.LogError("Target RectTransform is not assigned!");
                return Vector3.zero;
            }

            Canvas canvas = targetRectTransform.GetComponentInParent<Canvas>();

            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                return targetRectTransform.position; // In screen space, directly use position
            }
            else
            {
                // Convert from local space of targetRectTransform to world space
                RectTransformUtility.PixelAdjustRect(targetRectTransform, canvas);
                return targetRectTransform.position; // Return world position
            }
        }

        /// <summary>
        /// Triggers the coin reward animation.
        /// </summary>
        /// <param name="coinCount">Number of coins to animate (not used here, but can be integrated).</param>
        public void RewardPileOfCoins(int coinCount)
        {
            if (targetRectTransform == null)
            {
                Debug.LogError("Target RectTransform is not assigned!");
                return;
            }

            ResetPile();

            float delay = 0f;
            coinPile.SetActive(true);

            Vector3 targetWorldPosition = GetTargetWorldPosition();

            for (int i = 0; i < coinPile.transform.childCount; i++)
            {
                GameObject coin = coinPile.transform.GetChild(i).gameObject;

                // Scale up the coin
                LeanTween.scale(coin, Vector3.one, scaleUpDuration)
                    .setDelay(delay)
                    .setEase(LeanTweenType.easeOutBack);

                // Move the coin to the target position
                LeanTween.move(coin, targetWorldPosition, moveDuration)
                    .setDelay(delay + moveDelay)
                    .setEase(LeanTweenType.easeOutBack);

                // Scale down the coin after reaching the target
                LeanTween.scale(coin, Vector3.zero, scaleDownDuration)
                    .setDelay(delay + scaleDownDelay)
                    .setEase(LeanTweenType.easeOutBack);

                delay += staggerDelay;
            }
        }
    }
}
