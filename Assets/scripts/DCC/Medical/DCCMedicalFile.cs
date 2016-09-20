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

        string vitalsText = " VITALS";

        for (int i = 0; i < NameFields.Length; i++)
        {
            if (i == 1)
                vitalsText = " VITALS";
            else
                vitalsText = "";

            switch (MedicalInfo.PatientName)
            {
                case DCCMedicalInfo.PatientNames.Wall:
                    NameFields[i].Text = Configuration.Get("patient-name-01", "Carter Lewis").ToUpper() + vitalsText;
                    break;
                case DCCMedicalInfo.PatientNames.Lori:
                    NameFields[i].Text = Configuration.Get("patient-name-02", "Lori Taylor").ToUpper() + vitalsText;
                    break;
                case DCCMedicalInfo.PatientNames.Toshi:
                    NameFields[i].Text = Configuration.Get("patient-name-03", "Toshi Ishida").ToUpper() + vitalsText;
                    break;
                case DCCMedicalInfo.PatientNames.Suyin:
                    NameFields[i].Text = Configuration.Get("patient-name-04", "Suyin Zhang").ToUpper() + vitalsText;
                    break;
                case DCCMedicalInfo.PatientNames.Jonas:
                    NameFields[i].Text = Configuration.Get("patient-name-05", "Jonas Taylor").ToUpper() + vitalsText;
                    break;
            }
        }
	}
}
