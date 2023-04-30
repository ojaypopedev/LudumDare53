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

    Vector3 startScale;

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        startScale = transform.localScale;

        renderers.ToList().ForEach(e => e.materials.ToList().ForEach(m => m.SetColor("_HighlightColor", (Color.black))));

    }

    public void SetHighlight(bool active)
    {
        highlight = active;
        transform.localScale = startScale * ((highlight) ? highlightScale : 1);
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
