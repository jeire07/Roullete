using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public AttendanceData AttendanceData;

    [SerializeField] private Transform _ballParent;
    [SerializeField] private GameObject _ballPrefab;

    private List<GameObject> _ballPool = new List<GameObject>();
    private int poolSize = 200;
    [SerializeField] private int _totalBallCount;

    [SerializeField] private List<GameObject> _persons = new List<GameObject>();

    public int MaxWinnerCount;
    public List<string> _winners = new List<string>();

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        AttendanceData = Resources.Load<AttendanceData>("Datas/AttendanceData");
        _ballPrefab = Resources.Load<GameObject>("Prefabs/Ball");

        _ballPool.Capacity = poolSize;
        _winners.Capacity = MaxWinnerCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ball = Instantiate(_ballPrefab);
            ball.transform.parent = _ballParent;
            ball.SetActive(false);
            _ballPool.Add(ball);
        }
    }

    private GameObject GetBall()
    {
        foreach (GameObject ball in _ballPool)
        {
            if (!ball.activeInHierarchy)
            {
                ball.SetActive(true);
                return ball;
            }
        }
        return null;
    }

    private void ResetBall()
    {
        foreach (GameObject ball in _ballPool)
        {
            if (ball.GetComponent<Transform>().parent != _ballParent)
            {
                ball.GetComponent<Transform>().parent = _ballParent;
                ball.SetActive(false);
            }
        }
    }

    private void LoadCSVData()
    {
        CSVLoader csvLoader = transform.GetComponent<CSVLoader>();
        csvLoader.LoadCSVData();
        CalcBallAmount();
        _persons.Capacity = AttendanceData.Attendances.Length;
        CreatePersons();
        SetBalls();
    }

    public void CalcBallAmount()
    {
        foreach (PersonAttendance person in AttendanceData.Attendances)
        {
            _totalBallCount += person.Count;
        }
    }

    private void CreatePersons()
    {
        for (int i = 0; i < AttendanceData.Attendances.Length; i++)
        {
            GameObject person = new()
            {
                name = $"Person{i + 1}"
            };
            _persons.Add(person);
        }
    }

    private void ChangeColorRandomly(SpriteRenderer spriteRenderer)
    {
        Color[] colors = new Color[] { Color.red, Color.green, Color.blue, Color.white, Color.black, Color.yellow, Color.cyan, Color.magenta, Color.gray };

        if (spriteRenderer != null)
        {
            int randomIndex = Random.Range(0, colors.Length);
            spriteRenderer.color = colors[randomIndex];
        }
        else
        {
            Debug.LogError("SpriteRenderer component is missing!");
        }
    }

    private void SetBalls()
    {
        int index = 0;
        for(int i = 0; i < AttendanceData.Attendances.Length; i++)
        {
            for(int j = 0; j < AttendanceData.Attendances[i].Count; j++)
            {
                Transform _ballTransform = _ballPool[index].GetComponent<Transform>();
                _ballTransform.parent = _persons[i].GetComponent<Transform>();

                SpriteRenderer ballSprite = _ballPool[index].GetComponent<SpriteRenderer>();
                ChangeColorRandomly(ballSprite);

                TMP_Text nameText = _ballTransform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
                nameText.text = AttendanceData.Attendances[i].Name;

                _ballPool[index].transform.position = new Vector3(9f + 6f * index / _totalBallCount, 6, 0);
            }
        }
    }

    public void Shuffle()
    {
        for(int index = 0; index < _totalBallCount; index++)
        {
            float x = UnityEngine.Random.Range(9f, 15f);
            _ballPool[index].transform.position = new Vector3(x, 6, 0);
        }
    }

    public void OnLoadButton()
    {
        LoadCSVData();
    }

    public void OnStartButton()
    {
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    #region SetWinner
    public void DisableClone(string name)
    {
        if(int.TryParse(name.Substring(6), out int index))
        {
            _persons[index].SetActive(false);
            SetWinnerName(index);
        }
    }

    public List<string> SetWinnerName(int index)
    {
        string winnerName = AttendanceData.Attendances[index].Name;

        if(_winners.Count < MaxWinnerCount)
        {
            _winners.Add(winnerName);
        }
        return _winners;
    }
    #endregion
}
