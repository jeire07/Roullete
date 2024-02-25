using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CSVLoader : MonoBehaviour
{
    private AttendanceData _attendanceData;

    void Start()
    {
        _attendanceData = GameManager.Instance.AttendanceData;
    }

    public void LoadCSVData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "attendance.csv");

        if (File.Exists(filePath))
        {
            string[] data = File.ReadAllLines(filePath);

            List<PersonAttendance> personAttendances = new List<PersonAttendance>();

            for (int i = 1; i < data.Length; i++)
            {
                string[] row = data[i].Split(',');

                PersonAttendance personAttendance = new PersonAttendance();
                personAttendance.Name = row[0];
                personAttendance.Count = int.Parse(row[1]);

                personAttendances.Add(personAttendance);
            }

            _attendanceData.Attendances = personAttendances.ToArray();
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }
    }
}
