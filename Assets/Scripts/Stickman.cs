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
    public bool isGrounded = false;
    public int feetOnGround = 0;
    private bool isFarthest;

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

    public int genomeInputs = 5;
    public int genomeOutputs = 7;

    public float torsoRotation;
    public double[] vision;
    private int counter = 0;

    private void Awake()
    {
        vision = new double[genomeInputs];
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
        rbs = new Rigidbody2D[]{torsoRB, leftLegTopRB, leftLegBottomRB, rightLegTopRB, rightLegBottomRB, leftArmRB, rightArmRB };
        leftLegBottomJoint = leftLegBottom.GetComponent<HingeJoint2D>();
        leftLegTopJoint = leftLegTop.GetComponent<HingeJoint2D>();
        rightLegBottomJoint = rightLegBottom.GetComponent<HingeJoint2D>();
        rightLegTopJoint = rightLegTop.GetComponent<HingeJoint2D>();
        //hide();
    }

    public float getXPos()
    {
        return transform.localPosition.x;
    }
    private void Update()
    {
        if (StickmanMendelMachine.Instance.CheckFarthest(transform))
        {
            isFarthest = true;
            //show();
        }
    }
    public void look()
    {
        
        torsoRotation = torso.transform.rotation.eulerAngles.z / 360;//ensure between 0 and 1
        vision[0] = torsoRotation;
        //vision[1] = 1 / (1 + Mathf.Pow((float)Math.E, torsoRB.velocity.x)); //normalizes the input to be between 0 and 1, called a sigmoid cool
        //vision[2] = 1 / (1 + Mathf.Pow((float)Math.E, torsoRB.velocity.y));
        //vision[3] = 1 / (1 + Mathf.Pow((float)Math.E, leftLegTopRB.velocity.x));
        //vision[4] = 1 / (1 + Mathf.Pow((float)Math.E, leftLegTopRB.velocity.y));
        //vision[5] = 1 / (1 + Mathf.Pow((float)Math.E, leftLegBottomRB.velocity.x));
        //vision[6] = 1 / (1 + Mathf.Pow((float)Math.E, leftLegBottomRB.velocity.y));
        //vision[7] = 1 / (1 + Mathf.Pow((float)Math.E, rightLegTopRB.velocity.x));
        //vision[8] = 1 / (1 + Mathf.Pow((float)Math.E, rightLegTopRB.velocity.y));
        //vision[9] = 1 / (1 + Mathf.Pow((float)Math.E, rightLegBottomRB.velocity.x));
        //vision[10] = 1 / (1 + Mathf.Pow((float)Math.E, rightLegBottomRB.velocity.y));
        vision[1] = leftLegTop.transform.eulerAngles.z / 360;
        vision[2] = leftLegBottom.transform.eulerAngles.z / 360;
        vision[3] = rightLegTop.transform.eulerAngles.z / 360;
        vision[4] = rightLegBottom.transform.eulerAngles.z / 360;
        //vision[5] = Convert.ToInt32(isGrounded);
        //vision[6] = 1 / (1 + Mathf.Pow((float)Math.E, leftDistCheck.position.y));
        //vision[7] = 1 / (1 + Mathf.Pow((float)Math.E, rightDistCheck.position.y));
        //TODO check for feet distnace from ground
        //Debug.Log(vision[0] + " " + vision[1] + " " + vision[2] + " " + vision[3]);
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
        float[] decision = new float[doubleDecision.Length];
        for (int i = 0; i < doubleDecision.Length; i++)
        {
            decision[i] = (float)doubleDecision[i];
        }
        //get the output of the neural network
        /*
        if (!isGrounded)
        {
            torsoRB.AddForce(new Vector2(decision[0], 0) *  0.5f);
            leftLegTopRB.AddForce(new Vector2(decision[2], 0) *  0.5f);
            leftLegBottomRB.AddForce(new Vector2(decision[4], 0) *  0.5f);
            rightLegTopRB.AddForce(new Vector2(decision[6], 0) *  0.5f);
            rightLegBottomRB.AddForce(new Vector2(decision[8], 0) *  0.5f);
        }
        else
        {
            torsoRB.AddForce(new Vector2(decision[0], decision[1]) *  0.5f);
            leftLegTopRB.AddForce(new Vector2(decision[2], decision[3]) *  0.5f);
            leftLegBottomRB.AddForce(new Vector2(decision[4], decision[5]) *  0.5f);
            rightLegTopRB.AddForce(new Vector2(decision[6], decision[7]) *  0.5f);
            rightLegBottomRB.AddForce(new Vector2(decision[8], decision[9]) *  0.5f);
        }
        */
        counter++;
        /*
        if (counter % 5 == 0) //remove jitter
        {
            
            torsoRB.MoveRotation(Mathf.Clamp(decision[0] * 10, -5, 5));
            leftLegTopRB.MoveRotation(Mathf.Clamp(decision[1] * 10, -50, 50));
            leftLegBottomRB.MoveRotation(Mathf.Clamp(decision[2] * 10, -50, 50));
            rightLegTopRB.MoveRotation(Mathf.Clamp(decision[3] * 10, -50, 50));
            rightLegBottomRB.MoveRotation(Mathf.Clamp(decision[4] * 10, -50, 50));
            leftArmRB.MoveRotation(Mathf.Clamp(decision[5] * 10, -50, 50));
            rightArmRB.MoveRotation(Mathf.Clamp(decision[6] * 10, -50, 50));
            
            
            for (int i = 0; i < genomeOutputs; i++)
            {
                if (Mathf.Abs((float)Math.Sin(decision[i])) > 0.5)
                {
                    rbs[i].MoveRotation(Mathf.Clamp(decision[i] * 30, -50, 50));
                }
                else
                {
                    rbs[i].MoveRotation(Mathf.Clamp(decision[i] * -30, -50, 50));
                }
            }
            
        }
        */
        
        if (counter % 10 == 0) //remove jitter
        {
            for(int i = 0; i < genomeOutputs; i++)
            {
                decision[i] = (float)Math.Sin(decision[i]);
                if (Mathf.Abs(decision[i]) > 0.5f)
                {
                    rbs[i].AddTorque(Mathf.Clamp(decision[i] * 1000, -10000, 10000));
                }
                
                else
                {
                    rbs[i].AddTorque(Mathf.Clamp(decision[i] * -1000, -10000, 10000));
                }
                
            }
        }
        
        /*
        if (counter % 20 == 0)
        {
            torsoRB.angularVelocity = Mathf.Clamp(decision[0], -10, 10);
            //Debug.Log(decision[0] + " " + decision[1] + " " + decision[2] + " " + decision[3]);
            leftLegTopRB.angularVelocity = Mathf.Clamp(decision[1], -1000, 1000);
            leftLegBottomRB.angularVelocity = Mathf.Clamp(decision[2], -1000, 1000);
            rightLegTopRB.angularVelocity = Mathf.Clamp(decision[3], -1000, 1000);
            rightLegBottomRB.angularVelocity = Mathf.Clamp(decision[4], -1000, 1000);
        }
        8?
        /*
        if (counter % 5 == 0 && isGrounded)
        {
            if (decision[4] > 0.5)
            {
                JointMotor2D motor = leftLegBottomJoint.motor;
                motor.motorSpeed = decision[0] * 3f;
                leftLegBottomJoint.motor = motor;
            }
            else
            {
                JointMotor2D motor = leftLegBottomJoint.motor;
                motor.motorSpeed = decision[0] * -3f;
                leftLegBottomJoint.motor = motor;
            }
            if (decision[5] > 0.5)
            {
                JointMotor2D motor = leftLegTopJoint.motor;
                motor.motorSpeed = decision[1] * 3f;
                leftLegTopJoint.motor = motor;
            }
            else
            {
                JointMotor2D motor = leftLegTopJoint.motor;
                motor.motorSpeed = decision[1] * -3f;
                leftLegTopJoint.motor = motor;
            }
            if (decision[6] > 0.5)
            {
                JointMotor2D motor = rightLegTopJoint.motor;
                motor.motorSpeed = decision[2] * 3f;
                rightLegTopJoint.motor = motor;
            }
            else
            {
                JointMotor2D motor = rightLegTopJoint.motor;
                motor.motorSpeed = decision[2] * -3f;
                rightLegTopJoint.motor = motor;
            }
            if (decision[7] > 0.5)
            {
                JointMotor2D motor = rightLegBottomJoint.motor;
                motor.motorSpeed = decision[3] * 3f;
                rightLegBottomJoint.motor = motor;
            }
            else
            {
                JointMotor2D motor = rightLegBottomJoint.motor;
                motor.motorSpeed = decision[3] * -3f;
                rightLegBottomJoint.motor = motor;
            }
        }
        */
        /*
        if (counter % 5 == 0)
        {
            torsoRB.AddTorque(Mathf.Clamp((float)Math.Sin(decision[0]), -5, 5));
            Debug.Log((float)Math.Sin(decision[0]) + " " + (float)Math.Sin(decision[1]) + " " + (float)Math.Sin(decision[2]) + " " + (float)Math.Sin(decision[3]) + " " + decision[4]);
            leftLegTopRB.AddTorque(Mathf.Clamp((float)Math.Sin(decision[1]), -50, 50));
            leftLegBottomRB.AddTorque(Mathf.Clamp((float)Math.Sin(decision[2]), -50, 50));
            rightLegTopRB.AddTorque(Mathf.Clamp((float)Math.Sin(decision[3]), -50, 50));
            rightLegBottomRB.AddTorque(Mathf.Clamp((float)Math.Sin(decision[4]), -50, 50));
        }
        */
        /*
        if (counter % 5 == 0)
        {
            torsoRB.AddTorque(Mathf.Clamp(decision[0], -5, 5));
            //Debug.Log((float)Math.Sin(decision[0]) + " " + (float)Math.Sin(decision[1]) + " " + (float)Math.Sin(decision[2]) + " " + (float)Math.Sin(decision[3]) + " " + decision[4]);
            leftLegTopRB.AddTorque(Mathf.Clamp(decision[1], -50, 50));
            leftLegBottomRB.AddTorque(Mathf.Clamp(decision[2], -50, 50));
            rightLegTopRB.AddTorque(Mathf.Clamp(decision[3], -50, 50));
            rightLegBottomRB.AddTorque(Mathf.Clamp(decision[4], -50, 50));
        }
        /*
        if (counter % 10 == 0)
        {
            torsoRB.angularVelocity = Mathf.Clamp(decision[0], -5, 5);
            //Debug.Log(decision[0] + " " + decision[1] + " " + decision[2] + " " + decision[3]);
            leftLegTopRB.angularVelocity =Mathf.Clamp(decision[1], -50, 50);
            leftLegBottomRB.angularVelocity =Mathf.Clamp(decision[2], -50, 50);
            rightLegTopRB.angularVelocity =Mathf.Clamp(decision[3], -50, 50);
            rightLegBottomRB.angularVelocity =Mathf.Clamp(decision[4], -50, 50);
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
