using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace EvolutionaryPerceptron
{
    [RequireComponent(typeof(Stickman))]
    public class NeuralStickman : BotHandler
    {
        public Rigidbody2D rb;
        public Stickman stickman;
        private int inputSize;
        private double[,] inputs;
        private double[,] outputs;
        private float lifespan = 1;
        private float lastX = 0;
        private float x;
        private float stillX;

        protected override void Start()
        {
            base.Start();
            stickman = GetComponent<Stickman>();
            inputSize = stickman.genomeInputs;
            inputs = new double[1, inputSize];
            stillX = stickman.torso.transform.position.x;
            StartCoroutine(CheckStill());
        }

        void FixedUpdate()
        {
            stickman.look();
            for(int i = 0; i < inputSize; i++)
            {
                inputs[0, i] = stickman.vision[i];
            }

            x = stickman.torso.transform.position.x;
            lifespan += 0.0001f;
            nb.AddFitness((x - lastX) / lifespan);
            //nb.AddFitness(rb.velocity.x / 50);
            if(Mathf.Abs(stickman.torsoRotation) > 170 && Mathf.Abs(stickman.torsoRotation) < 190)
            {
                nb.AddFitness(-1000);
            }
            lastX = x;

            outputs = nb.SetInput(inputs);
            stickman.think(GetRow(outputs, 0));
        }
        
        public static T[] GetRow<T>(T[,] array, int row)
        {
            if (!typeof(T).IsPrimitive)
                throw new InvalidOperationException("Not supported for managed types.");

            if (array == null)
                throw new ArgumentNullException("array");

            int cols = array.GetUpperBound(1) + 1;
            T[] result = new T[cols];

            int size;

            if (typeof(T) == typeof(bool))
                size = 1;
            else if (typeof(T) == typeof(char))
                size = 2;
            else
                size = Marshal.SizeOf<T>();

            Buffer.BlockCopy(array, row * cols * size, result, 0, cols * size);

            return result;
        }
        
        public void DestroySelf()
        {
            nb.Destroy();
        }

        private IEnumerator CheckStill()
        {
            yield return new WaitForSeconds(3);
            if(Mathf.Abs(x - stillX) < 1)
            {
                DestroySelf();
            }
            stillX = stickman.torso.transform.position.x;
            StartCoroutine(CheckStill());
        }
    }
}