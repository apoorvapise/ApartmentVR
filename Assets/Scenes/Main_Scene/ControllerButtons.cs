using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ControllerButtons : MonoBehaviour
{
    private GameObject searchMenu;
    private GameObject apartmentsMenu;
    private GameObject floorPlanMenu;
    private GameObject character;
    private CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        if (OVRManager.display.displayFrequenciesAvailable.Contains(90f))
        {
            OVRManager.display.displayFrequency = 90f;
        }
        if (Unity.XR.Oculus.Performance.TryGetDisplayRefreshRate(out var rate))
        {
            float newRate = 90f; // fallback to this value if the query fails.
            if (Unity.XR.Oculus.Performance.TryGetAvailableDisplayRefreshRates(out var rates))
            {
                newRate = rates.Max();
            }
            if (rate < newRate)
            {
                if (Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(newRate))
                {
                    Time.fixedDeltaTime = 1f / newRate;
                    Time.maximumDeltaTime = 1f / newRate;
                }
            }
        }
        character = GameObject.Find("/OurOVRPlayerController");
        CharacterController cc = character.GetComponent<CharacterController>();
        searchMenu = GameObject.Find("/Main UI Group/MainMenuCanvas/Search UI");
        apartmentsMenu = GameObject.Find("/Main UI Group/MainMenuCanvas/Apartments UI");
        floorPlanMenu = GameObject.Find("/Main UI Group/FloorPlanCanvas/Floor Plan UI");
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {  //B: Go back
            
            if (!searchMenu.active)
            {
                if (!apartmentsMenu.active)
                {
                    if (!floorPlanMenu.active)
                    {      //Nothing is open, go to floorplan
                        searchMenu.SetActive(false);
                        apartmentsMenu.SetActive(false);
                        floorPlanMenu.SetActive(true);
                        cc.enabled = false;
                        character.transform.position = new Vector3(32, 2, 0);
                        character.transform.rotation = Quaternion.Euler(0, -90, 0);
                        cc.enabled = true;
                    }
                    else
                    {   //Floorplan open, go to apartments
                        searchMenu.SetActive(false);
                        apartmentsMenu.SetActive(true);
                        floorPlanMenu.SetActive(false);
                    }
                }
                else
                {  //Apartments open, go to main menu (search)
                    searchMenu.SetActive(true);
                    apartmentsMenu.SetActive(false);
                    floorPlanMenu.SetActive(false);
                }
            }
        }
        else if (OVRInput.GetDown(OVRInput.Button.One))
        {  //A: Go to home
            if (!searchMenu.active && !apartmentsMenu.active && !floorPlanMenu.active)
            {      //Teleport to Main room
                cc.enabled = false;
                character.transform.position = new Vector3(32, 2, 0);
                character.transform.rotation = Quaternion.Euler(0, -90, 0);
                cc.enabled = true;
            }
            searchMenu.SetActive(true);
            apartmentsMenu.SetActive(false);
            floorPlanMenu.SetActive(false);
        }
    }
}
