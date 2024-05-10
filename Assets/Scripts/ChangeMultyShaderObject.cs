using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeMultyShaderObject : MonoBehaviour
{
    //Inspector
    [SerializeField]
    MultyShader [] shaders;
    
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
            List<Material>materiales = new List<Material>();
            if (shaders[index].keepMaterial) 
            {
                foreach(Material mat in  defaultMaterials) 
                {
                    materiales.Add(mat);
                }
            }

            foreach(Material mat in  shaders[index].materials) 
            {
                materiales.Add(mat);
            }
                
    
            GetComponent<Renderer>().materials = materiales.ToArray();
        }
        else 
        {   
            GetComponent<Renderer>().materials = defaultMaterials.ToArray();
        }
    }


}
