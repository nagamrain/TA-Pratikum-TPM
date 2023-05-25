using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;
using TMPro;
using UnityEngine.SceneManagement;

public class HighScoreManager : MonoBehaviour
{
    private string connectionString;
    private List<HighScore> highScores = new List<HighScore>();
    public GameObject scorePrefab;
    public Transform scoreParent;
    [SerializeField] int topRanks;
    [SerializeField] int saveScore;
    public TMP_InputField enterName;
    private int finalScore;

    // Start is called before the first frame update
    void Start()
    {
        connectionString = "URI = file:" + Application.dataPath + "/HighscoreDB.sqlite";
        DeleteExtraScore();
        ShowScores();
    }

    public void UpdateScore(int FScore)
    {
        finalScore = FScore;
    }
    public void EnterName()
    {
        if (enterName.text != string.Empty)
        {
            int score = finalScore;
            InsertScore(enterName.text, score);
            // enterName.text = string.Empty;
        }
    }


    private void InsertScore(string name, int newScore)
    {
        GetScores();
        int highScoreHelper = highScores.Count;

        if (highScores.Count > 0)
        {
            HighScore lowestScore = highScores[highScores.Count - 1];
            if (lowestScore != null && saveScore > 0 && highScores.Count >= saveScore && newScore > lowestScore.Score)
            {
                DeleteScore(lowestScore.ID);
                highScoreHelper--;
            }
        }
        if (highScoreHelper < saveScore)
        {
            using(IDbConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                using(IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    string sqlQuery = String.Format("INSERT INTO HighScores(Name,Score) VALUES(\"{0}\", \"{1}\")", name, newScore);
                    dbCmd.CommandText = sqlQuery;
                    dbCmd.ExecuteScalar();
                    dbConnection.Close();
                }
            }
        }
    }
    private void GetScores()
    {
        highScores.Clear();

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM HighScores";

                dbCmd.CommandText = sqlQuery;

                using(IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        highScores.Add(new HighScore(reader.GetInt32(0), reader.GetString(1), (reader.GetInt32(2))));
                    }

                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        highScores.Sort();
    }

    private void DeleteScore(int id)
    {
        using(IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using(IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("DELETE FROM HighScores WHERE PlayerID = \"{0}\"", id);
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }

    private void ShowScores()
    {
        GetScores();
        for (int i = 0; i < topRanks; i++)
        {
            if (i <= highScores.Count - 1)
            {
                GameObject tmpObject = Instantiate(scorePrefab);
                HighScore tmpScore = highScores[i];
                tmpObject.GetComponent<HighScoreScript>().SetScore("#" + (i + 1).ToString(), tmpScore.Name, tmpScore.Score.ToString());
                tmpObject.transform.SetParent(scoreParent);
                tmpObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void DeleteExtraScore()
    {
        GetScores();

        if (saveScore <= highScores.Count)
        {
            int deleteCount = highScores.Count - saveScore;
            highScores.Reverse();
            using(IDbConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                using(IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    for (int i = 0; i < deleteCount; i++)
                    {
                        string sqlQuery = String.Format("DELETE FROM HighScores WHERE PlayerID = \"{0}\"", highScores[i].ID);
                        dbCmd.CommandText = sqlQuery;
                        dbCmd.ExecuteScalar();
                    }
                    dbConnection.Close();
                }
            }
        }
    }
    public void Play()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void Quit()
    {
        SceneManager.LoadScene("Main");
    }
}
