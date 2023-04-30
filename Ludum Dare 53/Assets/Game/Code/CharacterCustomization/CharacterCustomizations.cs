using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HotDogCannon/CharacterCustomizations", fileName = "New Character Customizations")]
public class CharacterCustomizations : ScriptableObject
{
    public CharacteracterColor SkinTones;
    public CharacteracterColor ShirtColourPrimary;
    public CharacteracterColor ShirtColorSecondary;
    public CharacteracterColor TrouserColour;
    public CharacteracterColor HairColor;

    public CharacterCustomization GetRandom()
    {
        return new CharacterCustomization()
        {
            HairColour = HairColor.GetRandom(),
            ShirtPrimary = ShirtColourPrimary.GetRandom(),
            ShirtSecondary = ShirtColorSecondary.GetRandom(),
            SkinTone = SkinTones.GetRandom(),
            Trousers = TrouserColour.GetRandom(),
        };
    }


}

public struct CharacterCustomization
{
    public Color HairColour;
    public Color ShirtPrimary;
    public Color ShirtSecondary;
    public Color Trousers;
    public Color SkinTone;
}

