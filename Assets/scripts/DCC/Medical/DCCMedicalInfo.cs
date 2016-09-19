[System.Serializable]
public class DCCMedicalInfo
{
    public enum PatientNames
    {
        Wall,
        Lori,
        Toshi,
        Suyin,
        Jonas
    }

    public PatientNames PatientName;
    public string Gender;
    public string DateOfBirth;
    public string Height;
    public string Weight;
    public string BloodType;
}
