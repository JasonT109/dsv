using UnityEngine;
using Meg.Networking;

public class DCCSchematicSwitch : MonoBehaviour
{
    public buttonGroup ToggleGroup;
    public GameObject SubButton;
    public GameObject GliderButton;
    public int CurrentState;
    public bool IsSetter = false;
    public float UpdateTick = 0.2f;

    private float _updateTimer;

    void ToggleSchematics(int state)
    {
        if (state == 0 && SubButton.GetComponent<buttonControl>().active == false)
            ToggleGroup.toggleButtons(SubButton, true);
        
        if (state == 1 && GliderButton.GetComponent<buttonControl>().active == false)
            ToggleGroup.toggleButtons(GliderButton, true);
    }

    void Start ()
    {
        if (!IsSetter)
        {
            CurrentState = (int)serverUtils.GetServerData("DCCschematicsToggle");
            ToggleSchematics(CurrentState);
        }
        else
        {
            if (SubButton.GetComponent<buttonControl>().active)
                serverUtils.SetServerData("DCCschematicsToggle", 0);
            else
                serverUtils.SetServerData("DCCschematicsToggle", 1);
        }
	}

	void Update ()
    {
        _updateTimer += Time.deltaTime;
        if (_updateTimer < UpdateTick)
            return;

        _updateTimer = 0;

        if (!IsSetter)
        {
            CurrentState = (int)serverUtils.GetServerData("DCCschematicsToggle");
            ToggleSchematics(CurrentState);
        }
        else
        {
            if (SubButton.GetComponent<buttonControl>().active)
                serverUtils.SetServerData("DCCschematicsToggle", 0);
            else
                serverUtils.SetServerData("DCCschematicsToggle", 1);
        }
    }
}
