using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWand : MonoBehaviour
{
    void Update()
    {   
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                ChangeShaderObject changeShaderObject = hit.collider.GetComponent<ChangeShaderObject>();
                if(changeShaderObject != null) 
                {
                    changeShaderObject.changeShader();
                }
            }
        }
    }
}
