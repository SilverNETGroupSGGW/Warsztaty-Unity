using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int points;
    int pointsCount;

    public Text scoreText;
    public GameObject winScreen;
    
    void Start()
    {
        pointsCount = GameObject.FindGameObjectsWithTag( "Point" ).Length;

        UpdatePointsText();
    }

    public void OnPointCollect()
    {
        points++;
        UpdatePointsText();

        if( points == pointsCount )
        {
            OnWin();
        }
    }

    void UpdatePointsText()
    {
        scoreText.text = $"Points: {points}/{pointsCount}";
    }

    void OnWin()
    {
        Debug.Log( "WYGRALES" );
        winScreen.SetActive( true );
    }

    public void RestartGame()
    {
        SceneManager.LoadScene( 0 );
    }
}
