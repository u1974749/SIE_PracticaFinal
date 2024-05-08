using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShaderObject : MonoBehaviour
{
    //Inspector
    [SerializeField]
    Material [] shaders;
    
    //Variables privadas
    int index = 0;
    List<Material> defaultMaterials = new List<Material>();
    void Start()
    {
        foreach(Material mat in GetComponent<Renderer>().materials)
        {
            defaultMaterials.Add(mat);
        }
    }

    public void changeShader() 
    {
        index++;
        if (index >= shaders.Length) 
        {
            index = -1;
        }
        
        if (index < shaders.Length && index > -1) 
        {
            GetComponent<Renderer>().material = shaders[index];
        }
        else 
        {
            GetComponent<Renderer>().materials = defaultMaterials.ToArray();
        }
    }


}
