using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneManager : MonoBehaviour
{

    [SerializeField] private ARPlaneManager aRPlaneManager; //crear el plano
    [SerializeField] private GameObject model3DPrefab; //crear campo para referenciar el plano detectado
    private List<ARPlane> planes = new List<ARPlane>();// Lista para almacenar los planos detectados
    private GameObject model3DPlaced;//Asignar el modelo creado

    //evento para cuando se detecta un plano
    private void OnEnable()
    {
        aRPlaneManager.planesChanged += PlanesFound;
    }

    private void OnDisable()// desactivar la detección de planos
    {
        aRPlaneManager.planesChanged -= PlanesFound;
    }

    private void PlanesFound(ARPlanesChangedEventArgs planeData) //para crear el modelo 3D sobre el plano que cumple con las características necesarias
    {
        if (planeData.added != null && planeData.added.Count > 0)//comprobar que detectó el plano. Si no es nulo y la cuenta de planos es > 0
        {
            planes.AddRange(planeData.added);
        }

        foreach(var plane in planes)
        {
            if (plane.extents.x * plane.extents.y > 0.4 && model3DPlaced == null) //si el area del plano tomado es > a 0.4 m"2 y no ha creado el plano
            {
                model3DPlaced = Instantiate(model3DPrefab);
                float yOffset = model3DPlaced.transform.localScale.y / 2; //Ubicación y de donde se ubica el modelo con respecto al plano
                model3DPlaced.transform.position = new Vector3(plane.center.x, plane.center.y + yOffset, plane.center.z);
                model3DPlaced.transform.forward = plane.normal;// ubicar el modelo perpendicular al plano
                StopPlaneDetection();
            }
        }
    }

    public void StopPlaneDetection()//para que deje de detectar planos
    {
        //aRPlaneManager.requestedDetectionMode = PlaneDetectionMode.None;//dejar de usar PlaneDetection
        foreach (var plane in planes)//para todos los elementos de la lista de planos
        {
            plane.gameObject.SetActive(false);//cambiar a falso (desactivar)
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
