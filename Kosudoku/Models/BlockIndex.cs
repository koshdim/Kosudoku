namespace Kosudoku.Models
{
    internal class BlockIndex
    {
        public static int Next(int dimension, int blockNumber, int currentBlockIndex)
        {
            var blockRow = (int)blockNumber / dimension;
            var blockColumn = blockNumber % dimension;

            var blockTopLeft = dimension*(dimension*dimension*blockRow + blockColumn);

            var current = blockTopLeft + currentBlockIndex%dimension + (int) (currentBlockIndex/dimension)*dimension*dimension;

            if (current + 1 > blockTopLeft + dimension*dimension + dimension)
            {
                current = blockTopLeft;
            }
            if (current + 1 > (current % dimension) + dimension)
            {
                current += dimension*dimension;
            }
            return current;
        }
    }
}
