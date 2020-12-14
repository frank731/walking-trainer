using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvolutionaryPerceptron.MendelMachine;

public class StickmanMendelMachine : MendelMachine
{
    private int index = 15;
    public Transform startPoint;
    public float farthest = 0;
    public CameraFollowBest cameraFollow;

    private static StickmanMendelMachine _instance;
    public static StickmanMendelMachine Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(InstantiateBotCoroutine());
    }

    public override void NeuralBotDestroyed(Brain neuralBot)
    {
        base.NeuralBotDestroyed(neuralBot);

        Destroy(neuralBot.gameObject);

        index--;

        if (index <= 0)
        {
            Save();
            population = Mendelization();
            generation++;

            StartCoroutine(InstantiateBotCoroutine());
        }
    }
    private IEnumerator InstantiateBotCoroutine()
    {
        yield return null;
        index = individualsPerGeneration;

        for (int i = 0; i < population.Length; i++)
        {
            InstantiateBot(population[i], 30, startPoint, i);
        }
    }

    public bool CheckFarthest(Transform pos)
    {
        if (pos.position.x > farthest)
        {
            farthest = pos.position.x;
            cameraFollow.farthestStickman = pos;
            return true;
        }
        return false;
    }
}
