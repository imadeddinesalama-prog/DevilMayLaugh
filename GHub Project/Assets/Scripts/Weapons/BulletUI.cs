using UnityEngine;
using TMPro;

public class BulletUI : MonoBehaviour
{
    public TextMeshProUGUI bulletsText;
    public GameObject weaponPanel;

    private Revolver revolver;

    void Awake()
    {
        // Force hide before anything renders
        if (weaponPanel != null)
            weaponPanel.SetActive(false);
    }

    void Start()
    {
        revolver = FindFirstObjectByType<Revolver>();

        // Sync visibility immediately on start
        if (revolver != null && weaponPanel != null)
            weaponPanel.SetActive(revolver.HasWeapon());
    }

    void Update()
    {
        if (revolver == null) return;

        if (revolver.HasWeapon())
        {
            if (weaponPanel != null)
                weaponPanel.SetActive(true);
            if (bulletsText != null)
                bulletsText.text = BuildBulletDots(revolver.GetBullets());
        }
    }

    string BuildBulletDots(int remaining)
    {
        string result = "";
        for (int i = 0; i < 6; i++)
            result += i < remaining ? "● " : "○ ";
        return result.Trim();
    }
}