using UnityEngine;

[CreateAssetMenu(fileName = "new MultyShader")]
public class MultyShader : ScriptableObject
{
   public Material[] materials;
   public bool keepMaterial;
}
