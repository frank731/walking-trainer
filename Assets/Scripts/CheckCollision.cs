using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionaryPerceptron
{
    public class CheckCollision : MonoBehaviour
    {
        public Stickman stickman;
        public NeuralStickman neuralStickman;
        public bool killOnGround;
        public bool leftFoot;
        public bool rightFoot;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Stickman") && collision.gameObject != gameObject)
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            }
            else if (collision.collider.CompareTag("Ground"))
            {
                if (killOnGround)
                {
                    stickman.dead = true;
                    neuralStickman.DestroySelf();
                }
                else if (leftFoot) stickman.leftOnGround = true;
                else if (rightFoot) stickman.rightOnGround = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                if (leftFoot) stickman.leftOnGround = false;
                else if (rightFoot) stickman.rightOnGround = false;
            }
        }
    }
}