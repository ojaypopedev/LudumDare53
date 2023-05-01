using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CustomCharacter : MonoBehaviour
{
    public RendererMaterialIndex[] HairRenderers;
    public RendererMaterialIndex[] SkinRenderers;
    public RendererMaterialIndex[] ShirtPrimaryRenderers;
    public RendererMaterialIndex[] ShirtSecondaryRenderers;
    public RendererMaterialIndex[] TrouserRenderers;

    public GameObject[] optionalComponents;
    public Animator Animator => GetComponent<Animator>();
    public Highlight Highlight=> GetComponent<Highlight>();

    public void Setup(CharacterCustomization customization)
    {
        HairRenderers.ToList().ForEach(r => r.SetColor(customization.HairColour));
        SkinRenderers.ToList().ForEach(r => r.SetColor(customization.SkinTone));

        ShirtPrimaryRenderers.ToList().ForEach(r => r.SetColor(customization.ShirtPrimary));
        ShirtSecondaryRenderers.ToList().ForEach(r => r.SetColor(customization.ShirtSecondary));

        TrouserRenderers.ToList().ForEach(r => r.SetColor(customization.Trousers));

        optionalComponents.ToList().ForEach(e => e.SetActive(Random.value > 0.5f));
    }

    public void SetIdleAnimation()
    {
        Animator.Play("Sitting", -1, Random.Range(0f, 1f));
    }

    public void SetOrderAnimation()
    {
        Animator.Play("Order"); //, -1, Random.Range(0f, 1f));
        //Order
    }

    public void SetSadAnimation()
    {
        Animator.Play("Sad");
    }

    public void SetHappyAnimation()
    {
        Animator.Play("Happy");
    }
}

[System.Serializable]
public class RendererMaterialIndex
{
    public Renderer rend;
    public int MaterialIndex;
    public void SetColor(Color c)
    {
        rend.materials[MaterialIndex].color = c;
    }
}
