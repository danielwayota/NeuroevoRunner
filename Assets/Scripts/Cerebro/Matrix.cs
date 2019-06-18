using System;
using System.Collections.Generic;
using System.Text;
using CerebroML.Util;

namespace CerebroML
{
    public class Matrix
    {
        private float[] values;
        public float[] Values
        {
            get { return this.values; }
        }

        public int Rows
        {
            get; protected set;
        }

        public int Cols
        {
            get; protected set;
        }

        public int LinearSize
        {
            get { return this.Rows * this.Cols; }
        }

        /// =================================================
        /// <summary>
        /// Default Matrix constructor.
        /// The Matrix will have a 1D array with zeros at the begining.
        /// </summary>
        ///
        /// <param name="r">Row count</param>
        /// <param name="c">Column count</param>
        public Matrix(int r, int c)
        {
            this.Rows = r;
            this.Cols = c;

            this.values = new float[r * c];
        }

        /// =================================================
        /// <summary>
        /// Creates a Matrix based on a 1D float array
        /// </summary>
        ///
        /// <param name="values"></param>
        public Matrix(float[] values, int r, int c)
        {
            this.Rows = r;
            this.Cols = c;

            this.values = values;
        }

        /// =================================================
        /// <summary>
        /// Resets all the values with random values between -amplitude and amplitude
        /// </summary>
        ///
        /// <param name="amplitude"></param>
        public void Randomize(float amplitude = 1.0f)
        {
            for (int i = 0; i < this.values.Length; i++)
            {
                this.values[i] = StaticRandom.NextBilinear(amplitude);
            }
        }

        /// =================================================
        /// <summary>
        /// Sets the values using some 1D array
        /// </summary>
        ///
        /// <param name="flatArray"></param>
        public void Set(float[] flatArray)
        {
            if (flatArray.Length != this.values.Length)
            {
                throw new InvalidOperationException(
                    $"The Matrix and the 1D array have diferent linear lengths. {this.LinearSize} != {flatArray.Length}"
                );
            }

            for (int i = 0; i < this.values.Length; i++)
            {
                this.values[i] = flatArray[i];
            }
        }

        /// =================================================
        /// <summary>
        /// Adds some Matrix to this value per value
        /// </summary>
        ///
        /// <param name="some"></param>
        public void Add(Matrix some)
        {
            int r = this.Rows;
            int c = this.Cols;

            if (r != some.Rows || c != some.Cols)
            {
                throw new InvalidOperationException(
                    $"Cannot add matrices of diferent size. {r} {c} != {some.Rows} {some.Cols}"
                );
            }

            for (int i = 0; i < this.values.Length; i++)
            {
                this.values[i] += some.Values[i];
            }
        }

        /// =================================================
        /// <summary>
        /// Applies some function to the values
        /// </summary>
        ///
        /// <param name="runner"></param>
        public void Map(Func<float, float> runner)
        {
            int r = this.Rows;
            int c = this.Cols;
            for (int j = 0; j < r; j++)
            {
                for (int i = 0; i < c; i++)
                {
                    this.values[j * this.Cols + i] = runner(this.values[j * this.Cols + i]);
                }
            }
        }

        // =================================================
        /// <summary>
        /// Prints the Matrix to the std out
        /// </summary>
        public void Print()
        {
            int r = this.Rows;
            int c = this.Cols;

            for (int j = 0; j < r; j++)
            {
                for (int i = 0; i < c; i++)
                {
                    Console.Write(this.values[j * this.Cols + i] + " ");
                }
                Console.WriteLine();
            }
        }

        // =================================================
        // Statics
        // =================================================

        /// =================================================
        /// <summary>
        /// Creates a Column Matrix with some 1D array
        /// </summary>
        ///
        /// <param name="values"></param>
        /// <returns></returns>
        public static Matrix From1DColum(float[] values)
        {
            float[] data = new float[values.Length];

            Array.Copy(values, data, values.Length);

            return new Matrix(data, data.Length, 1);
        }

        /// =================================================
        /// <summary>
        /// Performs a Matrix multiplication and returns the result
        /// </summary>
        ///
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Matrix Product(Matrix a, Matrix b)
        {
            if (a.Cols != b.Rows)
            {
                throw new InvalidOperationException(
                    $"A columns should be equal to B rows. {a.Rows} {a.Cols} != {b.Rows} {b.Cols}"
                );
            }

            float[] newValues = new float[a.Rows * b.Cols];

            for (int j = 0; j < a.Rows; j++)
            {
                for (int i = 0; i < b.Cols; i++)
                {
                    float sum = 0;
                    for (int k = 0; k < a.Cols; k++)
                    {
                        sum += a.Values[j * a.Cols + k] * b.Values[k * b.Cols + i];
                    }

                    newValues[j * b.Cols + i] = sum;
                }
            }


            return new Matrix(newValues, a.Rows, b.Cols);
        }
    }
}
