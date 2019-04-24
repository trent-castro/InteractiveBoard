using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AH_TextFloatUp : MonoBehaviour
{
    [SerializeField] float m_TimeToReachGoal = 1.0f;
    [SerializeField] float m_unitsToMoveUp = 1.0f;

    private Vector3 intialPos = Vector3.zero;
    private float m_timePassed = 0.0f;
    private TextMeshProUGUI text;
    private Color m_intialColor;

    // Start is called before the first frame update
    void Start()
    {
        intialPos = gameObject.transform.position;
        text = GetComponent<TextMeshProUGUI>();
        m_intialColor = text.color;
    }

    // Update is called once per frame
    void Update()
    {
        m_timePassed += Time.deltaTime;
        Color newColor = text.color;
        newColor.a = Mathf.Lerp(1, 0, m_timePassed / m_TimeToReachGoal);
        text.color = newColor;
        gameObject.transform.position = Vector3.Lerp(intialPos, intialPos + Vector3.up * m_unitsToMoveUp, m_timePassed / m_TimeToReachGoal);
        if (m_timePassed > m_TimeToReachGoal)
            gameObject.SetActive(false);

    }
    private void OnDisable()
    {
        gameObject.transform.position = intialPos;
        text.color = m_intialColor;
        m_timePassed = 0.0f;
    }

}
