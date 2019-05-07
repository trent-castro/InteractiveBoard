using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles Canvas Animations
/// </summary>
public class AH_CanvasManager : MonoBehaviour
{
    /// <summary>
    /// A struct that keeps references to the left player and the right player's text objects
    /// </summary>
    [System.Serializable]
    struct PlayerText
    {
        public TextMeshProUGUI leftText;
        public TextMeshProUGUI rightText;

        public PlayerText(TextMeshProUGUI lefttext, TextMeshProUGUI righttext)
        {
            leftText = lefttext;
            rightText = righttext;
        }
    }

    [Header("External References")]
    [Tooltip("References to the in game score text mesh pro objects")]
    [SerializeField]
    private PlayerText scoreTextReferences = default;
    [Tooltip("A reference to the score text animation object pool")]
    [SerializeField]
    private ObjectPool scoreIncreaseObjectPool = null;
    [Tooltip("A reference to the play again menu object")]
    [SerializeField]
    private GameObject gameOverMenu = null;
    [Tooltip("References to the in game result text mesh pro objects")]
    [SerializeField]
    private PlayerText winLossResultTextReferences = default;
    [Tooltip("The reference to the parent object for the win loss texts")]
    [SerializeField]
    private GameObject winLossResultTextParentReference = null;


    [Header("Configuration")]
    [SerializeField]
    private Vector3 scoreIncreaseSpawnOffsetLeft = Vector3.zero;
    [SerializeField]
    private Vector3 scoreIncreaseSpawnOffsetRight = Vector3.zero;

    /// <summary>
    /// Starts the Animation for Point Incrase on respective side
    /// </summary>
    public void OnPointEarned(bool scoredGoalIsRight)
    {
        AH_GameMaster.Instance.IncreaseScore(scoredGoalIsRight);
        StartCoroutine(ScoreAnimationCoroutine(scoredGoalIsRight));
    }

    /// <summary>
    /// Coroutine that waits for the Score animation to complete before updating the Score UI
    /// </summary>
    /// <param name="scoredGoalIsRight">wWhich goal is scored on</param>
    /// <returns>The delay between the animation playing and the scores being updated</returns>
    IEnumerator ScoreAnimationCoroutine(bool scoredGoalIsRight)
    {
        //Locate Proper spawning point
        Vector3 spawnPosition = ((scoredGoalIsRight) ? scoreTextReferences.leftText.transform : scoreTextReferences.rightText.transform).position;
        Vector3 spawnOffset = scoredGoalIsRight ? scoreIncreaseSpawnOffsetLeft : scoreIncreaseSpawnOffsetRight;
        spawnPosition += spawnOffset;

        // Activate score animation
        ActivateAnimation(spawnPosition);

        //Wait for the duration of Animation
        yield return new WaitForSecondsRealtime(0.75f);

        // Update score
        UpdateScore();

        // Stop coroutine
        StopCoroutine("ScoreAnimationCoroutine");
    }

    /// <summary>
    /// Updates the scores
    /// </summary>
    private void UpdateScore()
    {
        scoreTextReferences.leftText.text = "" + AH_GameMaster.Instance.GetPlayerOneScore();
        scoreTextReferences.rightText.text = "" + AH_GameMaster.Instance.GetPlayerTwoScore();
    }

    /// <summary>
    /// Activates the Animation
    /// </summary>
    /// <param name="spawnPosition">The location at which to spawn the animation at</param>
    private void ActivateAnimation(Vector3 spawnPosition)
    {
        // Get a reference to the next object to use as the animation
        GameObject scoreAnimation = scoreIncreaseObjectPool.GetNextPooledObject();

        // Activate the animation
        scoreAnimation.transform.position = spawnPosition;
        scoreAnimation.SetActive(true);
    }

    /// <summary>
    /// Enables the ability to see the win/loss text
    /// </summary>
    public void ActivateWinLossText()
    {
        winLossResultTextParentReference.SetActive(true);
    }

    /// <summary>
    /// Updates the win loss texts for ending the game
    /// </summary>
    /// <param name="leftText">The phrase to change the left text to upon winning or losing the game</param>
    /// <param name="rightText">The phrase to change the right text to upon winning or losing the game</param>
    public void UpdateWinLossTexts(string leftText, string rightText)
    {
        winLossResultTextReferences.leftText.text = leftText;
        winLossResultTextReferences.rightText.text = rightText;
    }

    /// <summary>
    /// Activates the end game menu
    /// </summary>
    public void ActivateEndGameMenu()
    {
        // Checks if the game over menu exists
        if (gameOverMenu)
        {
            // Activates the menu
            gameOverMenu.SetActive(true);
        }
    }
}
