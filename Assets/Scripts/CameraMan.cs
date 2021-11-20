using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Camera))]
public class CameraMan : MonoBehaviour
{

    private static CameraMan cela;

    public static CameraMan Singleton
    {
        get
        {
            if (!cela) cela = FindObjectOfType<CameraMan>();
            return cela;
        }
    }
    [Header("Refs")]
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject objetASuivre;
    [Header("Propriétés Caméra")]
    [SerializeField] private float tmpsPrAttendreObjet;
    [SerializeField] private float vitesseMax;
    [SerializeField] private float decalageVertical;
    [SerializeField] private float decalageHorizontal;
    [Header("Rendu")]
    [SerializeField] private float dezoom;
    [SerializeField] private FileRendu fileRendu;
    private enum FileRendu
    {
        Update,
        FixedUpdate
    }
    [Header("Limitations")] 
    [SerializeField] private Vector2 etenduesLimites;
    [SerializeField] private Vector2 positionLimites;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(positionLimites,etenduesLimites);
    }

    public float Dezoom
    {
        get => dezoom;
        set => camera.orthographicSize = value;
    }



    

    private void OnValidate()
    {
        if (!camera) TryGetComponent(out camera);
        camera.orthographicSize = dezoom;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (objetASuivre && fileRendu == FileRendu.Update)
        {
            SuivreObjetASuivre();
            LimiterPosCam();
        }
    }

    private void FixedUpdate()
    {
        if (objetASuivre && fileRendu == FileRendu.FixedUpdate)
        {
            SuivreObjetASuivre();
            LimiterPosCam();
        }
    }

    private void SuivreObjetASuivre()
    {
        var position = transform.position;
        Vector3 posAAttendre = objetASuivre.transform.position;
        posAAttendre.x += decalageHorizontal;
        posAAttendre.y += decalageVertical;

        Vector2 velocite = new Vector2();

        float diffTmps = fileRendu == FileRendu.Update ? Time.deltaTime : Time.fixedDeltaTime;
        
        Vector3 nvllePos = Vector2.SmoothDamp(position,posAAttendre,ref velocite
            ,tmpsPrAttendreObjet, vitesseMax, diffTmps );

        nvllePos.z = position.z;
        position = nvllePos;
        transform.position = position;
    }

    public bool EstDansCamera(Vector3 position)
    {
        Vector2 pos = camera.WorldToScreenPoint(position);
        return !(pos.x < 0 || pos.y < 0 ||
                 pos.x > camera.scaledPixelWidth || pos.y > camera.scaledPixelHeight);
    }
    
    private void LimiterPosCam()
    {
        
        //en partant du bord gauche, dans le sens horaire
        var position = camera.transform.position;
        Vector4 bordsCam = new Vector4
        {
            x = position.x - camera.orthographicSize * camera.aspect,
            y = position.y + camera.orthographicSize,
            z = position.x + camera.aspect * camera.orthographicSize,
            w = position.y - camera.orthographicSize
        };
        Vector4 limites = new Vector4
        {
            x = positionLimites.x - etenduesLimites.x / 2,
            y = positionLimites.y + etenduesLimites.y / 2,
            z = positionLimites.x + etenduesLimites.x / 2,
            w = positionLimites.y - etenduesLimites.y / 2
        };
        
        Vector3 nvllePosition = transform.position;

        if (bordsCam.x < limites.x)
        {
            nvllePosition.x += limites.x - bordsCam.x;
        }
        else if (bordsCam.z > limites.z)
        {
            nvllePosition.x -= bordsCam.z - limites.z;
        }

        if (bordsCam.y > limites.y)
        {
            nvllePosition.y -= bordsCam.y - limites.y;
        }
        else if (bordsCam.w < limites.w)
        {
            nvllePosition.y += limites.w - bordsCam.w;
        }

        transform.position = nvllePosition;
    }
    
    private IEnumerator screenShake;
    public void ScreenShake(float force, float duree)
    {
        if(screenShake != null) StopCoroutine(screenShake);
        screenShake = ScreenShakeRoutine(force, duree);
        StartCoroutine(screenShake);
    }

    private IEnumerator ScreenShakeRoutine(float force, float duree)
    {
        float tmps = duree;
        while (tmps > 0)
        {
            Vector3 nvllePos = transform.position;
            nvllePos.x += Random.Range(-1f, 1f) * force;
            nvllePos.y += Random.Range(-1f, 1f) * force;
            transform.position = nvllePos;
            
            float diffTmps = fileRendu == FileRendu.Update ? Time.deltaTime : Time.fixedDeltaTime;

            if (fileRendu == FileRendu.Update) yield return new WaitForEndOfFrame();
            else yield return new WaitForFixedUpdate();

            tmps -= diffTmps;
        }

        screenShake = null;
    }
}
