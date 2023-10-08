using UnityEngine.UI;

public class FlexibleSelectible : FlexibleUI
{
    private Selectable selectable;

    protected override void OnSkinUI()
    {
        base.OnSkinUI();
        selectable = GetComponent<Selectable>();
        selectable.colors = skinData.colorBlock;

    }
}
