using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickman : MonoBehaviour
{
    public float fitness;
    public bool dead;
    public bool replay;
    public int gen = 0;
    public int score;
    public int bestScore = 0;
    public bool showing;
    public bool leftOnGround = false;
    public bool rightOnGround = false;
    private bool isFarthest;
    public float speed = 300;

    public GameObject torso;
    public GameObject leftLegTop;
    public GameObject leftLegBottom;
    public GameObject rightLegTop;
    public GameObject rightLegBottom;
    public GameObject leftArm;
    public GameObject rightArm;

    public Transform leftDistCheck;
    public Transform rightDistCheck;

    private Rigidbody2D torsoRB;
    private Rigidbody2D leftLegTopRB;
    private Rigidbody2D leftLegBottomRB;
    private Rigidbody2D rightLegTopRB;
    private Rigidbody2D rightLegBottomRB;
    private Rigidbody2D leftArmRB;
    private Rigidbody2D rightArmRB;
    private Rigidbody2D[] rbs;

    private HingeJoint2D leftLegTopJoint;
    private HingeJoint2D leftLegBottomJoint;
    private HingeJoint2D rightLegTopJoint;
    private HingeJoint2D rightLegBottomJoint;

    private SpriteRenderer[] sprites;

    private GameObject stickmanPrefab;
    private Transform sp;

    public int genomeInputs = 11;
    public int genomeOutputs = 5;

    public float torsoRotation;
    public double[] vision;
    private int counter = 0;
    private StickmanMendelMachine stickmanMendelMachine;

    [SerializeField]
    EvolutionaryPerceptron.NeuralStickman ns;

    private void Awake()
    {
        vision = new double[genomeInputs];
        stickmanMendelMachine = StickmanMendelMachine.Instance;
    }
    void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        torsoRB = torso.GetComponent<Rigidbody2D>();
        leftLegTopRB = leftLegTop.GetComponent<Rigidbody2D>();
        leftLegBottomRB = leftLegBottom.GetComponent<Rigidbody2D>();
        rightLegTopRB = rightLegTop.GetComponent<Rigidbody2D>();
        rightLegBottomRB = rightLegBottom.GetComponent<Rigidbody2D>();
        leftArmRB = leftArm.GetComponent<Rigidbody2D>();
        rightArmRB = rightArm.GetComponent<Rigidbody2D>();
        rbs = new Rigidbody2D[] { torsoRB, leftLegTopRB, leftLegBottomRB, rightLegTopRB, rightLegBottomRB, leftArmRB, rightArmRB };
        leftLegBottomJoint = leftLegBottom.GetComponent<HingeJoint2D>();
        leftLegTopJoint = leftLegTop.GetComponent<HingeJoint2D>();
        rightLegBottomJoint = rightLegBottom.GetComponent<HingeJoint2D>();
        rightLegTopJoint = rightLegTop.GetComponent<HingeJoint2D>();
        //hide();
    }

    private void Update()
    {
        if (stickmanMendelMachine.CheckFarthest(transform, ns))
        {
            isFarthest = true;
            //show();
        }
    }
    public void look()
    {
        
        torsoRotation = torso.transform.rotation.eulerAngles.z / 360;//ensure between 0 and 1
        vision[0] = torsoRotation;
        vision[1] = leftLegTop.transform.rotation.eulerAngles.z / 360;
        vision[2] = leftLegBottom.transform.rotation.eulerAngles.z / 360;
        vision[3] = rightLegBottom.transform.rotation.eulerAngles.z / 360;
        vision[4] = rightLegTop.transform.rotation.eulerAngles.z / 360;
        vision[5] = 1 / (1 + Mathf.Pow((float)Math.E, torsoRB.angularVelocity)); //normalizes the input to be between 0 and 1, called a sigmoid cool
        vision[6] = 1 / (1 + Mathf.Pow((float)Math.E, leftLegBottomRB.angularVelocity));
        vision[7] = 1 / (1 + Mathf.Pow((float)Math.E, rightLegBottomRB.angularVelocity));
        //vision[8] = Convert.ToInt32(leftOnGround);
        //vision[9] = Convert.ToInt32(rightOnGround);
        vision[8] = Mathf.Clamp(torso.transform.position.y + 0.5f, -1, 1);//height of stickman 
    }

    public void show()
    {
        showing = true;
        foreach (SpriteRenderer spriteRenderer in sprites)
        {
            spriteRenderer.enabled = true;
        }
    }

    public void hide()
    {
        showing = false;
        foreach(SpriteRenderer spriteRenderer in sprites)
        {
            spriteRenderer.enabled = false;
        }
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------------------
    //gets the output of the brain then converts them to actions
    public void think(double[] doubleDecision)
    {
        //get the output of the neural network
        float[] decision = new float[doubleDecision.Length];
        for (int i = 0; i < doubleDecision.Length; i++)
        {
            decision[i] = (float)Math.Tanh(doubleDecision[i]);
            //
        }
        //set motors and torque according to outputs
        torsoRB.AddTorque(decision[0] * 4000 * Time.fixedDeltaTime);
        //joints
        JointMotor2D llm = leftLegBottomJoint.motor;
        llm.motorSpeed = decision[1] * speed;
        leftLegBottomJoint.motor = llm;

        JointMotor2D ltm = leftLegTopJoint.motor;
        ltm.motorSpeed = decision[2] * speed;
        leftLegTopJoint.motor = ltm;

        JointMotor2D rlm = rightLegBottomJoint.motor;
        rlm.motorSpeed = decision[3] * speed;
        rightLegBottomJoint.motor = rlm;

        JointMotor2D rtm = rightLegTopJoint.motor;
        rtm.motorSpeed = decision[4] * speed;
        rightLegTopJoint.motor = rtm;
      /*
        if(counter % 10 == 0)
        {
            for (int i = 0; i < genomeOutputs; i++)
            {
                decision[i] = (float)Math.Tanh(decision[i]);
                ns.AddFitness(Math.Abs(decision[i]) * -0.1f);
                rbs[i].MoveRotation(decision[i] * 50);
            }
            //TODO switch to motors like dani vid
        }
        */
    }
    private void OnDestroy()
    {
        if (isFarthest)
        {
            StickmanMendelMachine.Instance.farthest = 0;
        }
    }

}
