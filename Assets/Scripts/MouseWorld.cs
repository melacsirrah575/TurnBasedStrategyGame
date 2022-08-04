using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;

    private void Awake() 
    {
        //Turning this into a singleton to persist between scenes should I use multiple scenes for levels
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private LayerMask mousePlaneLayerMask;
    void Update() 
    {
        transform.position = MouseWorld.GetPosition();
    }

    public static Vector3 GetPosition() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, instance.mousePlaneLayerMask);
        return hit.point;
    }
}
