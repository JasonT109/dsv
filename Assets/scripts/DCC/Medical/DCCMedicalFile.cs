using UnityEngine;
using System.Collections;

public class DCCMedicalFile : MonoBehaviour
{
    [Header ("Patient Information")]
    public DCCMedicalInfo MedicalInfo;
    public Texture2D[] PhotoIDs;

    [Header("Configuration")]
    public widgetText Gender;
    public widgetText DOB;
    public widgetText Height;
    public widgetText Weight;
    public widgetText BloodType;
    public Renderer PhotoRenderer;
    public widgetText[] NameFields;

    private Texture2D PhotoID;

	void Start ()
    {
        PhotoID = PhotoIDs[(int)MedicalInfo.PatientName];
        Gender.Text = MedicalInfo.Gender;
        DOB.Text = MedicalInfo.DateOfBirth;
        Height.Text = MedicalInfo.Height;
        Weight.Text = MedicalInfo.Weight;
        BloodType.Text = MedicalInfo.BloodType;
        PhotoRenderer.material.mainTexture = PhotoID;

        foreach (widgetText n in NameFields)
        {
            switch (MedicalInfo.PatientName)
            {
                case DCCMedicalInfo.PatientNames.Wall:
                    n.Text = Configuration.Get("patient-name-01", "Carter Lewis");
                    break;
                case DCCMedicalInfo.PatientNames.Lori:
                    n.Text = Configuration.Get("patient-name-02", "Lori Taylor");
                    break;
                case DCCMedicalInfo.PatientNames.Toshi:
                    n.Text = Configuration.Get("patient-name-03", "Toshi Ishida");
                    break;
                case DCCMedicalInfo.PatientNames.Suyin:
                    n.Text = Configuration.Get("patient-name-04", "Suyin Zhang");
                    break;
                case DCCMedicalInfo.PatientNames.Jonas:
                    n.Text = Configuration.Get("patient-name-05", "Jonas Taylor");
                    break;
            }
        }
	}
}
