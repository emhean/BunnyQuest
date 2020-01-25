using BunnyQuest.ECS;
using BunnyQuest.ECS.Components;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


class MatrixFunktioner
{
    public int[][] Matrix_transformer(int[][] matrix)
    {
        var width = matrix.Length;
        int height = matrix[0].Length;
        int[][] new_matrix = new int[width][];

        for (int w = 0; w < new_matrix.Length; w++)
            for (int h = 0; h < height; h++)
                new_matrix[w] = new int[height];


        if (height == width)
        {
            for (int x = width; x >= 0; x--)
            {
                for (int y = height; y >= 0; y--)
                {

                    new_matrix[height - y][x] = matrix[x][y];
                }
            }
            return new_matrix;
        }
        else throw new System.Exception("Your 2D array is not square! So we crash.");

    }
}

