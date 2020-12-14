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
        public bool feet;

        private void Start()
        {
            if (feet)
            {
                stickman.feetOnGround++;
            }
        }
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
                else if (feet)
                {
                    stickman.feetOnGround++;
                    stickman.isGrounded = true;
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Ground") && feet)
            {
                stickman.feetOnGround--;
                if (stickman.feetOnGround <= 0)
                {
                    stickman.isGrounded = false;
                }
            }
        }
    }
}