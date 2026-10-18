using Godot;
using System;
using System.Collections.Generic;

public partial class Shop : Control
{
    [Export] private Control ticketWindowRef;
    [Export] private Control upgradeWindowRef;
    [Export] private BoxContainer upgradeItemContainer;
    [Export] private PackedScene upgradeItemScene;
    [Export] private ScrollContainer upgradesScrollContainerRef;

    private const int UPGRADES_SIZE_MARGIN = 90;
    private Tween _shopTween;

    public event Action<TicketSpotUpgradeButton.ShopUpgrades, IUpgradeGroup, Action<IUpgradeGroup>> OnAnyItemUpgradeRequested;

    public override void _Ready()
    {
        base._Ready();
        ConnectTabButtonsJuice();
    }

    private void ConnectTabButtonsJuice()
    {
        foreach (string btnName in new[] { "TicketsButton", "UpgradesButton" })
        {
            if (GetNodeOrNull<TextureButton>(btnName) is TextureButton btn)
            {
                btn.PivotOffset = btn.Size / 2f;
                btn.MouseEntered += () =>
                {
                    btn.PivotOffset = btn.Size / 2f;
                    Tween t = CreateTween();
                    t.TweenProperty(btn, "scale", new Vector2(1.06f, 1.06f), 0.1f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
                    t.Parallel().TweenProperty(btn, "modulate", new Color(1.15f, 1.15f, 1.15f), 0.1f);
                };
                btn.MouseExited += () =>
                {
                    Tween t = CreateTween();
                    t.TweenProperty(btn, "scale", Vector2.One, 0.1f);
                    t.Parallel().TweenProperty(btn, "modulate", Colors.White, 0.1f);
                };
            }
        }
    }

    // add a ticket spot with a array of upgrades
    public void AddSpotItem(TicketSpot pTicketSpotRef, Texture2D pItemTexture,  bool pIsUnlocked, Action<int, Action> pAction , params TicketSpotUpgradeButton.ShopUpgrades[] pUpgrades)
    {
        ShopUpgradeItem lItem = CreateAndBindShopItem();

        string lTitle = $"Ticket spot {pTicketSpotRef.spotId}";

        // Cast the params array in upgrades List

        lItem.Initialize(lTitle, pTicketSpotRef.ticketSpotUpgrades, new List<TicketSpotUpgradeButton.ShopUpgrades>(pUpgrades), pItemTexture, pIsUnlocked);
    }

    public void AddUniqueItem(string pName, IUpgradeGroup pUpgradeGroup,  Texture2D pItemTexture, bool pIsUnlocked, params TicketSpotUpgradeButton.ShopUpgrades[] pUpgrades)
    {
        ShopUpgradeItem lItem = CreateAndBindShopItem();

        lItem.Initialize(pName, pUpgradeGroup, new List<TicketSpotUpgradeButton.ShopUpgrades>(pUpgrades), pItemTexture, pIsUnlocked);
    }

    // Create new shop item
    private ShopUpgradeItem CreateAndBindShopItem()
    {
        ShopUpgradeItem lItem = (ShopUpgradeItem)upgradeItemScene.Instantiate();

        // Link buyout upgrade event
        lItem.OnUpgradeRequested += (augment, upgrades, visualCb) =>
        {
            OnAnyItemUpgradeRequested?.Invoke(augment, upgrades, visualCb);
        };

        upgradeItemContainer.AddChild(lItem);

        return lItem;
    }

    private void RefreshAllUpgrades()
    {
        foreach (Node child in upgradeItemContainer.GetChildren())
        {
            if (child is ShopUpgradeItem upgradeItem)
            {
                upgradeItem.UpdateVisual();
            }
        }
    }

    private void AnimateTabSwitch(Control tabToShow)
    {
        if (tabToShow == null) return;
        tabToShow.Modulate = new Color(1, 1, 1, 0);
        tabToShow.Scale = new Vector2(0.95f, 0.95f);
        Tween t = CreateTween();
        t.SetParallel(true);
        t.TweenProperty(tabToShow, "modulate:a", 1.0f, 0.15f);
        t.TweenProperty(tabToShow, "scale", Vector2.One, 0.2f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
    }

    private void ShowTicketsTab()
    {
        ticketWindowRef.Visible = true;
        upgradeWindowRef.Visible = false;
        AnimateTabSwitch(ticketWindowRef);
    }

    private void ShowUpgradesTab()
    {
        ticketWindowRef.Visible = false;
        upgradeWindowRef.Visible = true;
        RefreshAllUpgrades();
        AnimateTabSwitch(upgradeWindowRef);
    }

    private void OpenShopAnimated()
    {
        Visible = true;
        _shopTween?.Kill();
        Scale = new Vector2(0.15f, 0.15f);
        Modulate = new Color(1, 1, 1, 0);

        _shopTween = CreateTween();
        _shopTween.SetParallel(true);
        _shopTween.TweenProperty(this, "scale", Vector2.One, 0.26f)
                  .SetTrans(Tween.TransitionType.Back)
                  .SetEase(Tween.EaseType.Out);
        _shopTween.TweenProperty(this, "modulate:a", 1.0f, 0.18f);
    }

    private void CloseShopAnimated()
    {
        if (!Visible) return;
        _shopTween?.Kill();
        _shopTween = CreateTween();
        _shopTween.SetParallel(true);
        _shopTween.TweenProperty(this, "scale", new Vector2(0.15f, 0.15f), 0.16f)
                  .SetTrans(Tween.TransitionType.Back)
                  .SetEase(Tween.EaseType.In);
        _shopTween.TweenProperty(this, "modulate:a", 0.0f, 0.14f);
        _shopTween.SetParallel(false);
        _shopTween.TweenCallback(Callable.From(() =>
        {
            Visible = false;
            Scale = Vector2.One;
            Modulate = Colors.White;
        }));
    }

    private void _on_shop_button_pressed()
    {
        if (!Visible)
        {
            ShowTicketsTab();
            OpenShopAnimated();
        }
        else
        {
            CloseShopAnimated();
        }
    }

    private void _on_close_button_pressed() => CloseShopAnimated();
    private void _on_tickets_button_pressed() => ShowTicketsTab();
    private void _on_upgrades_button_pressed() => ShowUpgradesTab();

    public async void OpenSpecificWindow(int pSpotNumber)
    {
        OpenShopAnimated();
        ShowUpgradesTab();

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        int childIndex = pSpotNumber - 1;
        if (childIndex >= 0 && childIndex < upgradeItemContainer.GetChildCount())
        {
            if (upgradeItemContainer.GetChild(childIndex) is Control targetItem)
            {
                upgradesScrollContainerRef.ScrollVertical = (int)targetItem.Position.Y;
            }
        }
    }

}