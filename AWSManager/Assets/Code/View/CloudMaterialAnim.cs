using UnityEngine;

/// <summary>
/// Substance ProceduralMaterial animation script.
/// Support removed from Unity starting with Unity 2018.
/// </summary>
public class CloudMaterialAnim : MonoBehaviour {

// Script disabled for newer versions of the editor.
#if !UNITY_2018_1_OR_NEWER

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

#endif
}
