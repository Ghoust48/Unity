using UnityEngine;

public class MaterialController : MonoBehaviour 
{
    public void SetMaterial(Transform obj, Material[] materials, int index)
    {
        obj.GetComponent<Renderer>().material = materials[index];
    }
    
    public void SetPhysicMaterial(Transform obj, PhysicMaterial[] physicMaterials, int index)
    {
        obj.GetComponent<Collider>().material = physicMaterials[index];
    }
}
