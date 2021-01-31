using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{

    public float health = 100f;
    public float regenRate = 0.5f;
    public GameObject healthBarPrefab;
    public bool regenActive = true;

    private float maxHealth;
    private HealthBarController _healthBar;

    private void Start()
    {
        maxHealth = health;
        GameObject healthBarObject = Instantiate(healthBarPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
        healthBarObject.transform.SetSiblingIndex(0);
        _healthBar = healthBarObject.transform.GetComponent<HealthBarController>();
        if (regenActive)
        {
            InvokeRepeating(nameof(RegenRepeatingHealth), 0.2f, 0.2f);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthBar();

        SoundManager.Instance.PlaySound(SoundManager.Sound.PlayerHurt, 1f);

        if(health <= 0)
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.PlayerDead, 1f);
            StartCoroutine(WaitForSound(0.35f));
        }
    }

    private IEnumerator WaitForSound(float seconds) {
        yield return new WaitForSeconds(seconds);
        StartCoroutine(LevelManager.Instance.ResetScene());
    }
    private void RegenRepeatingHealth()
    {
        RegenHealth(this.regenRate);
    }

    public void RegenHealth(float ammount)
    {
        health += ammount;
        health = health > maxHealth ? maxHealth : health;

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float percentage = health / maxHealth;
        percentage = percentage < 0 ? 0 : percentage;
        _healthBar.UpdateHealthBar(percentage);
    }
}
