using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Acteurs;
using UnityEngine;

public class SpawnerScientifique : MonoBehaviour
{
    [System.Serializable]
    private struct ZoneSpawn
    {
        public Vector2 position;
        public Vector2 etendue;

        public ZoneSpawn(Vector2 _position, Vector2 _etendue)
        {
            position = _position;
            etendue = _etendue;
        }
    }

    private static SpawnerScientifique cela;

    public static SpawnerScientifique Singleton
    {
        get
        {
            if (!cela) cela = FindObjectOfType<SpawnerScientifique>();
            return cela;
        }
    }
    
    [SerializeField] private Scientifique scientiBase;
    [SerializeField] private List<ZoneSpawn> zonesSpawn = new List<ZoneSpawn>();
    [SerializeField] private float tmpsMinEntreSpawn;
    [SerializeField] private float tmpsMaxEntreSpawn;
    public float multTempsSpawn = 1;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var zoneSpawn in zonesSpawn)
        {
            Gizmos.DrawWireCube(zoneSpawn.position,zoneSpawn.etendue);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnCoolDown());
    }

    private IEnumerator SpawnCoolDown()
    {
        yield return new WaitForSeconds(Random.Range(tmpsMinEntreSpawn * multTempsSpawn, tmpsMaxEntreSpawn * multTempsSpawn));

        SpawnScienti();
        StartCoroutine(SpawnCoolDown());
    }

    private void SpawnScienti()
    {
        if (!scientiBase)
        {
            Debug.LogError("Wesh t'as pas assigner de Scientique par defo pour le spawner");
            return;
        }

        ZoneSpawn zoneSpawnAlea = zonesSpawn[Random.Range(0, zonesSpawn.Count)];
        Vector3 posAlea = new Vector3
        {
            x =Random.Range(zoneSpawnAlea.position.x - zoneSpawnAlea.etendue.x / 2,
                zoneSpawnAlea.position.x + zoneSpawnAlea.etendue.x / 2),

            y = Random.Range(zoneSpawnAlea.position.y - zoneSpawnAlea.etendue.y / 2,
                zoneSpawnAlea.position.y + zoneSpawnAlea.etendue.y / 2),
            z = transform.position.z
        };

        if (Instantiate(scientiBase.gameObject, posAlea, new Quaternion(), transform).TryGetComponent(out Scientifique nvScientifique))
        {
            
        }
    }
}
