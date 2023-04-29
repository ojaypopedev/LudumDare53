using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IconRenderer : MonoBehaviour
{
    
   Camera camera => GetComponent<Camera>();
    public RenderTexture rt => camera.targetTexture;

    [SerializeField] int index = 0;

    [ContextMenu("Create Icon")]
    public void CreateIcon()
    {
      

        //Create the directory if necessary
        //Create a folder "Art/2D/Icons"
        //Check if the Name is already in use, if it is we can append (1)... (2).. to it

        if (!AssetDatabase.IsValidFolder("Assets/Art"))
            AssetDatabase.CreateFolder("Assets","Art");

        if(!AssetDatabase.IsValidFolder("Assets/Art/2D"))
            AssetDatabase.CreateFolder("Assets/Art", "2D");

        if (!AssetDatabase.IsValidFolder("Assets/Art/2D/Icons"))
            AssetDatabase.CreateFolder("Assets/Art/2D", "Icons");


        AssetDatabase.CreateAsset(toTexture2D(rt), $"Assets/Art/2D/Icons/Icon_{index}.asset");
        index += 1;
       
    }

    
    // Use this for initialization
    

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBAHalf, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
