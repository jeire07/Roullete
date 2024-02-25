using UnityEngine;

[CreateAssetMenu(fileName = "AttendanceData", menuName = "New Attend")]
public class AttendanceData : ScriptableObject
{
    public PersonAttendance[] Attendances;
}

[System.Serializable]
public class PersonAttendance
{
    public string Name = "";
    public int Count = 0;
}
