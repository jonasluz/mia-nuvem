using UnityEngine;
using System.Collections;

public class CloudMaterialAnim : MonoBehaviour {

    [Range(0,1)] public float animSpeed = .1f;

    ProceduralMaterial substance;

    void Awake()
    {
        substance = gameObject.GetComponent<MeshRenderer>().sharedMaterial as ProceduralMaterial;
    }
	
	// Update is called once per frame
	void Update ()
    {
	if (substance)
        {
            float offset = substance.GetProceduralFloat("offset") + animSpeed;
            if (offset > .5) offset = -.5f;
            substance.SetProceduralFloat("offset", offset);
            substance.RebuildTextures();
        }
	}
}
