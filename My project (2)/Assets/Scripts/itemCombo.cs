using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItemCombo", menuName = "ItemCombo")]
public class itemCombo : ScriptableObject
{
    public List<itemClass> ingredients;
    public itemClass result;
}
