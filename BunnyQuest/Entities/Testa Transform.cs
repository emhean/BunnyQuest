using BunnyQuest.ECS;
using BunnyQuest.ECS.Components;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


class MatrixFunktioner
{
    public int[][] Matrix_transformer(int[][] matrix)
    {
        int height = matrix.Length;
        int width = matrix[0].Length;
        int[][] new_matrix = new int[height][];

        for (int w = 0; w < new_matrix.Length; w++)
          //  for (int h = 0; h < height + 1; h++)
                new_matrix[w] = new int[height];


        if (height == width)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    new_matrix[height - 1 - y][x] = matrix[y][x];
                }
            }
            return new_matrix;
        }
        else throw new System.Exception("Your 2D array is not square! So we crash.");

    }
}

