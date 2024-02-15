using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// UNUSED From older helix jump prototype
/// </summary>
public class Platform : MonoBehaviour
{
    public GameObject platformPartPrefab;
    public Mesh platformPartMesh;
    public Mesh platformPartTransparentMesh;
    public Mesh platformPartColliderMesh;
    public List<PlatformPart> platformParts = new List<PlatformPart>();

    private int platformPartsCount = 20;
    private int partsMaxSize = 4;
    private Vector3 triggerOffset = new Vector3(0f, -0.1f, 0f);

    private class PlatformPartSpawnData
    {
        public PlatformType platformType = PlatformType.Basic;
        public int platformValue = 1;

        public PlatformPartSpawnData()
        {

        }

        public PlatformPartSpawnData(PlatformType platformType, int platformValue = 1)
        {
            this.platformType = platformType;
            this.platformValue = platformValue;
        }
    }

    void Awake()
    {
    }

    public void Reset()
    {
        foreach (PlatformPart part in platformParts)
        {
            part.Reset();
        }
    }

    public void Random()
    {
        PlatformPartSpawnData[] spawnData = new PlatformPartSpawnData[platformPartsCount];
        spawnData.FillArray();

        spawnData[0].platformType = PlatformType.Trigger;
        //spawnData[18].platformType = PlatformType.Trigger;
        spawnData[19].platformType = PlatformType.Trigger;
        /*
        int i = UnityEngine.Random.Range(0, platformPartsCount);

        if(i < platformPartsCount - 1)
        {
            spawnData[i].platformType = PlatformType.Trigger;
            spawnData[i+1].platformType = PlatformType.Trigger;
        }
        else
        {
            spawnData[i].platformType = PlatformType.Trigger;
            spawnData[0].platformType = PlatformType.Trigger;
        }

        int b = UnityEngine.Random.Range(0, platformPartsCount);
        if (b == i || b == i + 1)
        {
            b -= 1;
            if (b < 0)
            {
                b = platformPartsCount - 1;
            }
        }

        spawnData[b].platformType = PlatformType.Bad;
        */

        GeneratePlatform(spawnData);
    }

    //Make platform and merge the part meshes to 1
    private void GeneratePlatform(PlatformPartSpawnData[] spawnData)
    {
        float angles = 360f / platformPartsCount;
        List<CombineInstance> instances = new List<CombineInstance>();
        List<CombineInstance> colliderInstances = new List<CombineInstance>();
        //CombineInstance combineInstance;

        int startPos = 0;
        int size = 50;
        PlatformType platformType = PlatformType.Basic;

        for(int i = 0; i < spawnData.Length; i++)
        {
            if (size < partsMaxSize && platformType == spawnData[i].platformType)// && i < spawnData.Length -1)
            {
            }
            else //Create new part
            {
                if (i != 0)
                    CreatePlatformPart(spawnData[startPos], instances.ToArray(), colliderInstances.ToArray(), startPos * angles);

                startPos = i;
                size = 0;
                //if (i < spawnData.Length - 1)
                {
                    instances = new List<CombineInstance>();
                    colliderInstances = new List<CombineInstance>();
                    platformType = spawnData[i].platformType;
                }
            }

            /*combineInstance = new CombineInstance();
            combineInstance.mesh = (platformType == PlatformType.Finish || platformType == PlatformType.Trigger) ? platformPartTransparentMesh : platformPartMesh;
            combineInstance.transform = Matrix4x4.TRS(transform.position, Quaternion.Euler(0f, (i - startPos) * angles, 0f), Vector3.one);

            instances.Add(combineInstance);

            combineInstance = new CombineInstance();
            combineInstance.mesh = platformPartColliderMesh;
            combineInstance.transform = Matrix4x4.TRS(transform.position, Quaternion.Euler(0f, (i - startPos) * angles, 0f), Vector3.one);

            colliderInstances.Add(combineInstance);
            */
            CombineInstances(transform.position, Quaternion.Euler(0f, (i - startPos) * angles, 0f), platformType, ref instances);//mesh for renderer
            CombineInstances(transform.position + (platformType == PlatformType.Trigger ? triggerOffset : Vector3.zero), Quaternion.Euler(0f, (i - startPos) * angles, 0f), platformPartColliderMesh, ref colliderInstances);//collider

            size++;
        }

        //Create Last part or add to first part
        if (spawnData[startPos].platformType == PlatformType.Trigger && spawnData[0].platformType == PlatformType.Trigger)
        {
            AddToPlatformPart(platformParts[0], instances, colliderInstances);
        }
        else
        {
            CreatePlatformPart(spawnData[startPos], instances.ToArray(), colliderInstances.ToArray(), startPos * angles);
        }
    }

    private void CreatePlatformPart(PlatformPartSpawnData spawnData, CombineInstance[] instances, CombineInstance[] colliderInstances, float angles)
    {
        PlatformPart part = Instantiate(platformPartPrefab, Vector3.zero, Quaternion.Euler(0f, angles, 0f), transform).GetComponent<PlatformPart>();

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(instances);
        part.GetComponent<MeshFilter>().sharedMesh = mesh;

        mesh = new Mesh(); //create new mesh for collider
        mesh.CombineMeshes(colliderInstances);
        part.GetComponent<MeshCollider>().sharedMesh = mesh;

        platformParts.Add(part);
        part.SetPart(spawnData.platformType, spawnData.platformValue);
    }

    private void AddToPlatformPart(PlatformPart platformPart, List<CombineInstance> instances, List<CombineInstance> colliderInstances)
    {
        float angles = 360f / platformPartsCount;

        CombineInstance ci = new CombineInstance();
        ci.mesh = platformPart.GetComponent<MeshFilter>().sharedMesh;
        ci.transform = Matrix4x4.TRS(Vector3.zero, platformPart.transform.rotation, Vector3.one);
        instances.Add(ci);

        ci.mesh = platformPart.GetComponent<MeshCollider>().sharedMesh;
        ci.transform = Matrix4x4.TRS(Vector3.zero, platformPart.transform.rotation, Vector3.one);
        colliderInstances.Add(ci);

        for (int i = 0; i < instances.Count-1; i++)//skip last cause it is part we will be adding to
        {
            ci = instances[i];
            ci.transform = Matrix4x4.TRS(transform.position, Quaternion.Euler(0f, -angles * (i + 1), 0f), Vector3.one);
            instances[i] = ci;

            ci = colliderInstances[i];
            ci.transform = Matrix4x4.TRS(transform.position + (platformPart.type == PlatformType.Trigger ? triggerOffset : Vector3.zero), Quaternion.Euler(0f, -angles * (i + 1), 0f), Vector3.one);
            colliderInstances[i] = ci;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(instances.ToArray());
        platformPart.GetComponent<MeshFilter>().sharedMesh = mesh;

        mesh = new Mesh(); //create new mesh for collider
        mesh.CombineMeshes(colliderInstances.ToArray());
        platformPart.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void CombineInstances(Vector3 position, Quaternion rotation, PlatformType platformType, ref List<CombineInstance> instances)
    {
        CombineInstances(position, rotation, (platformType == PlatformType.Finish || platformType == PlatformType.Trigger) ? platformPartTransparentMesh : platformPartMesh, ref instances);
    }

    private void CombineInstances(Vector3 position, Quaternion rotation, Mesh mesh, ref List<CombineInstance> instances)
    {
        CombineInstance combineInstance = new CombineInstance();
        combineInstance.mesh = mesh;
        combineInstance.transform = Matrix4x4.TRS(position, rotation, Vector3.one);

        instances.Add(combineInstance);
    }

    public void Finish()
    {
        /*foreach (PlatformPart part in platformParts)
        {
            part.SetPartAsFinish();
        }*/
        float angles = 360f / platformPartsCount;
        PlatformPartSpawnData[] spawnData = new PlatformPartSpawnData[] { new PlatformPartSpawnData(PlatformType.Finish) };

        CombineInstance combineInstance;
        List<CombineInstance> combinedMeshes = new List<CombineInstance>();
        List<CombineInstance> combinedColliderMeshes = new List<CombineInstance>();

        for (int i = 0; i < platformPartsCount; i++)
        {
            combineInstance = new CombineInstance();
            combineInstance.mesh = platformPartTransparentMesh;
            combineInstance.transform = Matrix4x4.TRS(transform.position, Quaternion.Euler(0f, i * angles, 0f), Vector3.one);

            combinedMeshes.Add(combineInstance);

            combineInstance = new CombineInstance();
            combineInstance.mesh = platformPartColliderMesh;
            combineInstance.transform = Matrix4x4.TRS(transform.position, Quaternion.Euler(0f, i  * angles, 0f), Vector3.one);

            combinedColliderMeshes.Add(combineInstance);
        }
        CreatePlatformPart(spawnData[0], combinedMeshes.ToArray(), combinedColliderMeshes.ToArray(), 0f);
    }

    public void PassedByBall()
    {
        foreach (PlatformPart part in platformParts)
        {
            part.PlatformPassedByBall();
        }
    }

    public void BreakByBall()
    {
        foreach (PlatformPart part in platformParts)
        {
            part.PlatformPassedByBall();
        }
    }
}
