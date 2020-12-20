using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RecycleObjects : MonoBehaviour 
{
    public abstract void Recycle();
    public abstract Vector3 SetScale();
    public abstract Vector3 SetPosition(Vector3 nextPosition, Vector3 scale);
}
