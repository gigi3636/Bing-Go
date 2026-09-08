public class UpgradeableStat
{
    public int level { get; private set; }
    public string name { get; private set; }

    public int[] augmentPrice;

    public UpgradeableStat (string pName, int[] pAugmentPrice)
    {
        name = pName;
        augmentPrice = pAugmentPrice;
    }

    public void Upgrade()
    {
        level++;
    }
}