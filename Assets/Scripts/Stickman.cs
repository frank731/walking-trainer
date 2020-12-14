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

    private Rigidbody2D torsoRB;
    private Rigidbody2D leftLegTopRB;
    private Rigidbody2D leftLegBottomRB;
    private Rigidbody2D rightLegTopRB;
    private Rigidbody2D rightLegBottomRB;

    private HingeJoint2D leftLegTopJoint;
    private HingeJoint2D leftLegBottomJoint;
    private HingeJoint2D rightLegTopJoint;
    private HingeJoint2D rightLegBottomJoint;

    private SpriteRenderer[] sprites;

    private GameObject stickmanPrefab;
    private Transform sp;

    public int genomeInputs = 15;
    public int genomeOutputs = 10;

    public float torsoRotation;
    public double[] vision;

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
        leftLegBottomJoint = leftLegBottom.GetComponent<HingeJoint2D>();
        leftLegTopJoint = leftLegTop.GetComponent<HingeJoint2D>();
        rightLegBottomJoint = rightLegBottom.GetComponent<HingeJoint2D>();
        rightLegTopJoint = rightLegTop.GetComponent<HingeJoint2D>();
        hide();
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
            show();
        }
    }
    public void look()
    {
        torsoRotation = torso.transform.rotation.eulerAngles.z;
        vision[0] = torsoRotation;
        vision[1] = torsoRB.velocity.x;
        vision[2] = torsoRB.velocity.y;
        vision[3] = leftLegTopRB.velocity.x;
        vision[4] = leftLegTopRB.velocity.y;
        vision[5] = leftLegBottomRB.velocity.x;
        vision[6] = leftLegBottomRB.velocity.y;
        vision[7] = rightLegTopRB.velocity.x;
        vision[8] = rightLegTopRB.velocity.y;
        vision[9] = rightLegBottomRB.velocity.x;
        vision[10] = rightLegBottomRB.velocity.y;
        vision[11] = leftLegTop.transform.eulerAngles.z;
        vision[12] = leftLegBottom.transform.eulerAngles.z;
        vision[13] = rightLegTop.transform.eulerAngles.z;
        vision[14] = rightLegBottom.transform.eulerAngles.z;
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
        //counter++;
        /*
        if (counter % 1 == 0 && isGrounded)
        {
            if (decision[0] > 0)
            {
                torsoRB.MoveRotation(decision[0] * 10);
            }
            if (decision[1] > 0)
            {
                leftLegTopRB.MoveRotation(decision[1] * 10);
            }
            if (decision[2] > 0)
            {
                leftLegBottomRB.MoveRotation(decision[2] * 10);
            }
            if (decision[3] > 0)
            {
                rightLegTopRB.MoveRotation(decision[3] * 10);
            }
            if (decision[4] > 0)
            {
                rightLegBottomRB.MoveRotation(decision[4] * 10);
            }
        }
        */
        /*
        if (counter % 10 == 0 && isGrounded)
        {
            Debug.Log(decision[0]);
            counter = 0;
            if (decision[4] > 0.5)
            {
                JointMotor2D motor = leftLegBottomJoint.motor;
                motor.motorSpeed = decision[0] * 10f;
                leftLegBottomJoint.motor = motor;
            }
            else
            {
                JointMotor2D motor = leftLegBottomJoint.motor;
                motor.motorSpeed = decision[0] * -10f;
                leftLegBottomJoint.motor = motor;
            }
            if (decision[5] > 0.5)
            {
                JointMotor2D motor = leftLegTopJoint.motor;
                motor.motorSpeed = decision[1] * 10f;
                leftLegTopJoint.motor = motor;
            }
            else
            {
                JointMotor2D motor = leftLegTopJoint.motor;
                motor.motorSpeed = decision[1] * -10f;
                leftLegTopJoint.motor = motor;
            }
            if (decision[6] > 0.5)
            {
                JointMotor2D motor = rightLegTopJoint.motor;
                motor.motorSpeed = decision[2] * 10f;
                rightLegTopJoint.motor = motor;
            }
            else
            {
                JointMotor2D motor = rightLegTopJoint.motor;
                motor.motorSpeed = decision[2] * -10f;
                rightLegTopJoint.motor = motor;
            }
            if (decision[7] > 0.5)
            {
                JointMotor2D motor = rightLegBottomJoint.motor;
                motor.motorSpeed = decision[3] * 10f;
                rightLegBottomJoint.motor = motor;
            }
            else
            {
                JointMotor2D motor = rightLegBottomJoint.motor;
                motor.motorSpeed = decision[3] * -10f;
                rightLegBottomJoint.motor = motor;
            }
        }
        */
        if (isGrounded)
        {
            torsoRB.AddTorque(Mathf.Clamp(decision[0], -10, 10));
            //Debug.Log(decision[0] + " " + decision[1] + " " + decision[2] + " " + decision[3]);
            leftLegTopRB.AddTorque(Mathf.Clamp(decision[1], -50, 50));
            leftLegBottomRB.AddTorque(Mathf.Clamp(decision[2], -50, 50));
            rightLegTopRB.AddTorque(Mathf.Clamp(decision[3], -50, 50));
            rightLegBottomRB.AddTorque(Mathf.Clamp(decision[4], -50, 50));
        }
        
        
        
    }
    private void OnDestroy()
    {
        if (isFarthest)
        {
            StickmanMendelMachine.Instance.farthest = 0;
        }
    }

}
