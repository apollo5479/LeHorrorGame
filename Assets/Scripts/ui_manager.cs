using UnityEngine;
using UnityEngine.SceneManagement;

public class ui_manager : MonoBehaviour
{
    /*
     * public void change_constant_oxygen_loss_multiplier_value(float new_num)
     * 
     * public void lose_stress(float stress_value)
     * public void gain_stress(float stress_value)
     * 
     * public void lose_hp(float damage)
     * public void gain_hp(float heal)
     * 
     * completed_task()
     * new_task() // call when new task objects are created
     * 
     */

    [Header("Tasks")]
    public int tasks_completed = 0;
    public int tasks_pending = 0;

    // lose breath speed muliplyer
    public float constant_oxygen_loss_multiplier = 1;

    // Variables for hp and stress bar
    Transform hp_bar;
    Transform stress_bar;

    [Header("Current Values")]
    private float max_hp = 100;
    public float cur_hp = 100;
    private float max_stress = 100;
    public float cur_stress = 0;
    public float constant_stress_value = 5;
    public bool gracePeriod = true;

    private float displayed_hp = 100f;
    private float displayed_stress = 0f;

    // Variables for o2 bar
    [Header("O2 Bar")]
    Transform o2_bar;
    private float max_o2 = 100;
    public float cur_o2 = 50;

    void Start()
    {
        Invoke(nameof(gracePeriodOff), 3f);
        // Set up hp and stress bar and o2
        locate_bars();
        // find number of tasks in the world.
        locate_tasks();

        //cur_stress = max_stress;
        displayed_stress = cur_stress;

        cur_hp = max_hp;
        displayed_hp = cur_hp;
    }

    void Update()
    {
        float uiLerpSpeed = 5f;
        // Smooth UI toward real values
        displayed_stress = Mathf.Lerp(displayed_stress, cur_stress, uiLerpSpeed * Time.deltaTime);
        //displayed_stress = Mathf.MoveTowards(displayed_stress,cur_stress,uiSpeed * Time.deltaTime);
        displayed_hp = Mathf.Lerp(displayed_hp, cur_hp, uiLerpSpeed * Time.deltaTime);

        // Update hp and stress and o2 meters
        if (hp_bar != null && stress_bar != null && o2_bar != null)
        {
            //float hp_scale = Mathf.Clamp(-1 * (cur_hp / max_hp), -1, 0);
            float hp_scale = Mathf.Clamp(-1 * (displayed_hp / max_hp), -1, 0);
            hp_bar.localScale = new Vector3(hp_scale,1,1);

            //float stress_scale = Mathf.Clamp((cur_stress / max_stress), 0, 1);
            float stress_scale = Mathf.Clamp((displayed_stress / max_stress), 0, 1);
            stress_bar.localScale = new Vector3(stress_scale, 1, 1);

            float o2_scale = Mathf.Clamp((cur_o2 / max_o2), 0, 1);
            o2_bar.localScale = new Vector3(o2_scale, 1, 1);
        }
        // Breath in and out using Q
        if (Input.GetKey(KeyCode.Q))
        {
            breath_in();
        }
        else
        {
            lose_breath();
            constant_stress_gain(3);
        }
    }

    void locate_tasks()
    {
        // find number of objects with task tag in the world
        // TO DO

    }

    public void completed_task(float stressDecrease)
    {
        tasks_completed++;
        tasks_pending--;
        lose_stress(stressDecrease);
    }
    public void new_task(float stressIncrease)
    {
        tasks_pending++;
        gain_stress(stressIncrease);
    }

    // Get the scale bars from the children list
    void locate_bars()
    {
        Transform maxHP = transform.Find("max_hp_bar");
        hp_bar = maxHP.Find("hp_bar");
        Debug.Log("HP Child Name: " + hp_bar.localScale);

        Transform maxStress = transform.Find("max_stress_bar");
        stress_bar = maxStress.Find("stress_bar");

        Transform maxO2 = transform.Find("max_o2_bar");
        o2_bar = maxO2.Find("o2_bar");
    }

    // Oxygen functions
    void breath_in()
    {
        cur_o2 = Mathf.Clamp(cur_o2 + (50 * Time.deltaTime), 0, 100);
    }
    void lose_breath()
    {
        cur_o2 = Mathf.Clamp(cur_o2 - (10 * Time.deltaTime * constant_oxygen_loss_multiplier), 0,100);
        check_o2_level();
    }
    public void change_constant_oxygen_loss_multiplier_value(float new_num)
    {
        constant_oxygen_loss_multiplier = new_num;
    }
    void check_o2_level()
    {
        if (cur_o2 <= 1)
        {
            lose_hp(0.3f);
        }
    }

    // Stress functions
    void constant_stress_gain(float constant_stress_value)
    {
        constant_stress_value = constant_stress_value / 6;
        cur_stress = Mathf.Clamp(cur_stress + (constant_stress_value * Time.deltaTime), 0, 100);
        check_stress_level();
    }
    public void lose_stress(float stress_value)
    {
        cur_stress = Mathf.Clamp(cur_stress - stress_value, 0, max_stress);
    }
    public void gain_stress(float stress_value)
    {
        cur_stress = Mathf.Clamp(cur_stress + stress_value, 0, max_stress);
        check_stress_level();
    }

    void check_stress_level()
    {
        if (cur_stress >= 99)
        {
            lose_hp(0.1f);
        }
    }

    // Health functions
    public void lose_hp(float damage)
    {
        if (!gracePeriod) { 
            cur_hp = Mathf.Clamp(cur_hp - damage, 0, max_hp);
            check_hp_level();
        }
    }
    public void gain_hp(float heal)
    {
        cur_hp = Mathf.Clamp(cur_hp + heal, 0, max_hp);
    }
    void check_hp_level()
    {
        if (cur_hp <= 1)
        {
            //SceneManager.LoadScene("EndGame"); UN COMMENT WHEN DONE BUG FIXING
        }
    }
    void gracePeriodOff() {
        gracePeriod = false;
    }
}