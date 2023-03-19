using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour, IDragHandler
{
    [Header("Drag Area")]
    public EventTrigger dragTrigger;

    [Header("Cameras")]
    public CinemachineFreeLook freelook;
    public GameObject[] cameras;

    [Header("Switch Camera Buttons")]
    public Button[] switchButtons;
    public GameObject switchCameraOptions;
    public Toggle cameraOptionsToggle;

    [Header("Camera View Text")]
    public string[] textOptions;
    public TMP_Text viewText;

    [Header("Options")]
    public float swipeSpeed = 0.5f;
    public Slider fovSlider;
    public Slider aimHeightSlider;
    public Transform aimHeightTarget;

    [Header("More")]
    public GameObject modelInfo;

    private void Start()
    {
        foreach(var cam in cameras)
        {
            if(cam.activeInHierarchy)
            {
                activeCam = cam;
            }
        }

        // Add events to the event listner

        // Create a new entry for the Drag event
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;

        // Set the callback method for the event
        entry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });

        // Add the event entry to the Event Trigger component
        dragTrigger.triggers.Add(entry);

        for(int i = 0; i < switchButtons.Length; i++)
        {
            int num = i;
            switchButtons[i].onClick.AddListener(delegate
            {
                SwitchCamera(num);
            });
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!cameras[0].activeInHierarchy)
            return;

        freelook.m_XAxis.Value += eventData.delta.x * swipeSpeed;
        freelook.m_YAxis.Value -= eventData.delta.y * swipeSpeed / 250f;
    }

    GameObject activeCam;
    public void SwitchCamera(int index)
    {
        InterfaceManager.sounds[0].Play();

        foreach(GameObject cam in cameras)
        {
            cam.SetActive(false);
        }

        dragTrigger.gameObject.SetActive(index == 0);

        modelInfo.SetActive(index == 3);

        aimHeightSlider.gameObject.SetActive(index == 0);

        cameras[index].SetActive(true);

        activeCam = cameras[index];

        if (activeCam.GetComponent<CinemachineFreeLook>() != null)
        {
            fovSlider.value = activeCam.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView;
        }
        else
        {
            fovSlider.value = activeCam.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;
        }

        viewText.text = textOptions[index];
    }

    public void ToggleOptions()
    {
        InterfaceManager.sounds[1].Play();
        switchCameraOptions.SetActive(cameraOptionsToggle.isOn);
    }

    public void SetFieldOfView()
    {
        if(activeCam.GetComponent<CinemachineFreeLook>() != null)
        {
            activeCam.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = fovSlider.value;
        }
        else
        {
            activeCam.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = fovSlider.value;
        }
    }

    public void SetFreelookAim()
    {
        aimHeightTarget.position = new Vector3(0, aimHeightSlider.value, 0);
    }
}
