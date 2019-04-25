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
    [Tooltip("A reference to the left score text transform")]
    [SerializeField]
    private Transform scoreAnimationAnchorLeft = null;
    [Tooltip("A reference to the right score text transform")]
    [SerializeField]
    private Transform scoreAnimationAnchorRight = null;
    [Tooltip("A reference to the score text animation object pool")]
    [SerializeField]
    private ObjectPool scoreIncreaseObjectPool;
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
    public void OnPointEarned(bool scoredGoalIsRight, ref int m_PlayerOneScore, ref int m_PlayerTwoScore)
    {
        //Locate Proper spawning point
        Vector3 spawnPosition = ((scoredGoalIsRight) ? scoreAnimationAnchorLeft : scoreAnimationAnchorRight).position;
        Vector3 spawnOffset = scoredGoalIsRight ? scoreIncreaseSpawnOffsetLeft : scoreIncreaseSpawnOffsetRight;
        spawnPosition += spawnOffset;

        // Activate score animation
        ActivateAnimation(spawnPosition);

        // Iterate and update score texts
        if (scoredGoalIsRight)
        {
            ++m_PlayerOneScore;
            scoreTextReferences.leftText.text = "" + m_PlayerOneScore;
        }
        else
        {
            ++m_PlayerTwoScore;
            scoreTextReferences.rightText.text = "" + m_PlayerTwoScore;
        }
    }

    /// <summary>
    /// Activates the Animation
    /// </summary>
    /// <param name="spawnPosition"></param>
    private void ActivateAnimation(Vector3 spawnPosition)
    {
        //fix locaion of animation
        GameObject scoreAnimation = scoreIncreaseObjectPool.GetNextPooledObject();

        //activate animaion
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
