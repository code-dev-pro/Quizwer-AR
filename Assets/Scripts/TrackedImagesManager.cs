using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class TrackedImagesManager : MonoBehaviour
{
   

    [SerializeField]
    private Button infoButton; 

    [SerializeField]
    private Text imageTrackedText;

    [SerializeField]
    private Text infoTrackedText;

    [SerializeField]
    private GameObject[] arObjectsToPlace;

    [SerializeField]
    private Vector3 scaleFactor = new Vector3(0.1f,0.1f,0.1f);

    private GameObject placedObject;
    private ARTrackedImageManager m_TrackedImageManager;
 
    private bool show = false;

    private bool applyRotatingPerObject = true;

    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();

    void Awake()
    {
        Application.targetFrameRate = 60;
  
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
        
       
        foreach(GameObject arObject in arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name;
            newARObject.SetActive(false);
            //newARObject.transform.Find("Info").gameObject.SetActive(false);
            arObjects.Add(arObject.name, newARObject);
            
        }
    }

    public void RotationChanged(float newValue)
    {
        if(applyRotatingPerObject){
            if(placedObject != null)
            {
                placedObject.transform.rotation = Quaternion.Euler(0,newValue*360,0);;
            }
        }
       
    }

    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    public void ShowInfo() { 
        Debug.Log($"clicked");
        show = !show;
        

    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)

    {

       foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
           
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
           
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            arObjects[trackedImage.name].SetActive(false);
            displayInfo("");
            Destroy(trackedImage.gameObject);

        } 
    }


    private void displayInfo (string txt="") {
          if(txt=="CarteAtomeCarbone") {
            imageTrackedText.text = "Atome de carbone";
            infoTrackedText.text = "C";
        } else if(txt=="CarteAtomeHydrogene") {
            imageTrackedText.text = "Atome d'hydrogène";
            infoTrackedText.text = "H";
        } else if(txt=="CartePlaneteSaturne") {
            imageTrackedText.text = "Planète Saturne";
            infoTrackedText.text = "";
        } else {
            imageTrackedText.text = "";
            infoTrackedText.text = ""; 
        } 

    }



    private void UpdateARImage(ARTrackedImage trackedImage)
    {
       
       
        AssignGameObject(trackedImage.referenceImage.name, trackedImage.transform.position);
        
        
    }

    void AssignGameObject(string name, Vector3 newPosition)
    {
        if(arObjectsToPlace != null)
        {
            GameObject goARObject = arObjects[name];
            
           
            foreach(GameObject go in arObjects.Values)
            {
                Debug.Log($"Go in arObjects.Values: {go.name}");
                if(go.name != name)
                {
                    go.SetActive(false);
                    go.transform.localScale = new Vector3(0, 0, 0);
                    displayInfo("");
               
                } else {

                }
            } 

            goARObject.SetActive(true);
            if(show==true) {
                // goARObject.transform.Find("Info").gameObject.SetActive(true);
            } else {
                // goARObject.transform.Find("Info").gameObject.SetActive(false);
            };
           
            displayInfo(name);
            placedObject=goARObject;
            goARObject.transform.position = newPosition;
            goARObject.transform.localScale = scaleFactor;
        } else {
            displayInfo("");
        }
    }
}
