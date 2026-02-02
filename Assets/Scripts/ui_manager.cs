using UnityEngine;

public class ui_manager : MonoBehaviour
{

    // Variables for hp and stress bar
    Transform hp_bar;
    Transform stress_bar;

    private float max_hp = 100;
    public float cur_hp = 100;

    private float max_stress = 100;
    public float cur_stress = 0;

    void Start()
    {

        // Set up hp and stress bar code
        Transform maxHP = transform.Find("max_hp_bar");
        hp_bar = maxHP.Find("hp_bar");
        Debug.Log("HP Child Name: " + hp_bar.localScale);

        Transform maxStress = transform.Find("max_stress_bar");
        stress_bar = maxStress.Find("stress_bar");

    }

    void Update()
    {
        // update hp and stress
        if (hp_bar != null && stress_bar != null)
        {
            float hp_scale = Mathf.Clamp(-1 * (cur_hp / max_hp), -1, 0);
            hp_bar.localScale = new Vector3(hp_scale,1,1);

            float stress_scale = Mathf.Clamp((cur_stress / max_stress), 0, 1);
            stress_bar.localScale = new Vector3(stress_scale, 1, 1);
        }
    }
}
