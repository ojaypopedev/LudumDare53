using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    public Renderer[] renderers;
    public bool highlight = false;
    public Color HighlightColor = Color.blue;
    public float highlightScale;
   
    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        renderers.ToList().ForEach(e => e.materials.ToList().ForEach(m => m.SetColor("_HighlightColor", (Color.black))));

    }

    public void SetHighlight(bool active)
    {
        highlight = active;
        transform.localScale = Vector3.one * ((highlight) ? highlightScale : 1f);
        renderers.ToList().ForEach(e => e.materials.ToList().ForEach(m => m.SetColor("_HighlightColor", (highlight ? HighlightColor : Color.black))));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            SetHighlight(!highlight);
        }
    }
}
