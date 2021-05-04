using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualsHandler : MonoBehaviour
{
    public Transform farthestStickman;
    public EvolutionaryPerceptron.NeuralStickman neuralStickman;
    public TMPro.TextMeshProUGUI dist;
    public TMPro.TextMeshProUGUI fitness;
    private float farthest, fittest;
    void Update()
    {
        try
        {
            transform.position = new Vector3(farthestStickman.position.x, 0,  farthestStickman.position.z -10);
            farthest = Mathf.Max(farthestStickman.position.x, farthest);
            fittest = Mathf.Max(neuralStickman.GetFitness(), fittest);
            dist.text = "Farthest Distance: " + farthest.ToString("F2");
            fitness.text = "Max Fitness: " + fittest.ToString("F2");
        }
        catch
        {
           
        }
    }
}
