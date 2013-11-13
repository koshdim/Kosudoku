using Kosudoku.Models;
using System;

namespace Kosudoku
{
    internal static class GameGenerator
    {
        public static Matrix Generate(int dimension)
        {
            var tableDimension= dimension * dimension;
            var blockRowNumber = dimension * dimension * dimension;

            var random = new Random();
            var matrix = new Matrix(dimension);
            matrix.Reset();

            for (var number = 1; number <= tableDimension; number++)
            {
                for (var i = 0; i < dimension; i++)
                {
                    for (var j = 0; j < dimension; j++)
                    {
                        var itemBlockIndex = random.Next(0, tableDimension - 1);
                        var freeCellFound = false;

                        for (int k = 0; k < tableDimension; k++)
                        {
                            var topLeftCellIndex = j*dimension + i*blockRowNumber;
                            var itemMatrixIndex = topLeftCellIndex + itemBlockIndex%dimension + (itemBlockIndex/dimension)*tableDimension;
                            var row = itemMatrixIndex/tableDimension;
                            var column = itemMatrixIndex%tableDimension;

                            if (CanAdd(matrix, number, row, column, dimension))
                            {
                                matrix.Numbers[itemMatrixIndex].Value = number;
                                freeCellFound = true;
                                break;
                            }

                            if (++itemBlockIndex >= tableDimension)
                                itemBlockIndex = 0;
                        }

                        if (!freeCellFound)
                        {
                            matrix.Reset();
                            number = 1;
                            i = -1;
                            break;
                        }
                    }
                }
            }

            return matrix;
        }

        private static bool CanAdd(Matrix matrix, int number, int row, int column, int dimension)
        {
            for (var i = 0; i < dimension * dimension && i != row; i++)
            {
                if (matrix.ItemAt(i, column).Value == number) return false;
            }
            for (var j = 0; j < dimension * dimension && j != column; j++)
            {
                if (matrix.ItemAt(row, j).Value == number) return false;
            }
            return matrix.ItemAt(row, column).Value == 0;
        }
    }
}
