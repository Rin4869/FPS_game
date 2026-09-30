using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    public EnemySpawner spawner;
    public TMP_Text waveText;
    public TMP_Text enemiesText;

    void Update()
    {
        if (spawner == null)
            return;

        waveText.text = "WAVE " + spawner.CurrentWave;
        enemiesText.text = "ENEMIES: " + spawner.EnemiesAlive;
    }
}