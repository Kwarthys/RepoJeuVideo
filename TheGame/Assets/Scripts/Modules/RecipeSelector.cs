using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSelector : AbstractInteract
{
    private Recipe recipe;

    public Text text;

    public RessourceProcessor module;

    public RessourceManager.Ressources input;
    public int nbInput;
    public RessourceManager.Ressources output;
    public int nbOutput;

    public int rawTimeToProcess;

    private void Start()
    {
        recipe = new Recipe(input, nbInput, output, nbOutput, rawTimeToProcess);
        text.text = recipe.getText();
    }

    public override void execute()
    {
        module.setRecipe(recipe);
    }
}
