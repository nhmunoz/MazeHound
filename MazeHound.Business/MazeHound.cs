using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeHound.Business
{
    public class MazeHound
    {
        private int columns = 0, rows = 0;
        private int[][] maze = null;// = new int[width][height]; // The maze
        private bool[][] wasHere;// = new boolean[width][height];
        private bool[][] correctPath;// = new boolean[width][height]; // The solution to the maze
        private string path = "";
        private string pathWithOffset = "";
        private int startX = 0, startY = 0; // Starting X and Y values of maze
        private int endX = 0, endY = 0;     // Ending X and Y values of maze

        public string Path {
            get { return path; }
        }

        public string PathWithOffset
        {
            get { return pathWithOffset; }
        }

        public int[][] LoadMaze(string tsv, int offsetRows, int offsetCols)
        {
            string[] lines;
            string[] realLines;
            int[][] loadedMaze = null;
            int count = 0;
            
            tsv = tsv.Replace("\tF", "0");
            tsv = tsv.Replace("\t", "1");
            lines = tsv.Split("\n".ToCharArray());
            columns = 0;
            rows = 0;
            realLines = new string[lines.Length - offsetRows];
            foreach(string line in lines)
            {
                if (line.Length > columns)
                {
                    columns = line.Length;
                }
                if (count >= offsetRows && line.Length > 0)
                {
                    realLines[rows] = line;
                    rows++;
                }
                count++;
            }

            columns -= offsetCols - 1;
           
            loadedMaze = new int[rows][];

            for(int i = 0; i < rows; i++)
            {
                loadedMaze[i] = new int[columns];
                for (int j = 0; j < columns; j++)
                {
                    // Create Maze (1 = path, 2 = wall)
                    if (realLines[i].Length > 0)
                        loadedMaze[i][j] = Convert.ToInt32(realLines[i][j + offsetCols - 1]) == 49 ? 2 : 1;
                    else
                        loadedMaze[i][j] = 2;
                }
            }
            return loadedMaze;
        }

        public bool SolveMaze(string tsv, int x1, int y1, int x2, int y2, int offsetx, int offsety)
        {
            maze = LoadMaze(tsv, offsetx, offsety); 
            startX = y1;
            startY = x1;
            endX = y2;
            endY = x2;
            wasHere = new bool[rows][];
            correctPath = new bool[rows][];
            for (int row = 0; row < maze.Length; row++)
            {
                wasHere[row] = new bool[columns];
                correctPath[row] = new bool[columns];
                for (int col = 0; col < maze[row].Length; col++)
                {
                    wasHere[row][col] = false;
                    correctPath[row][col] = false;
                }
            }
            path += "[[" + x2.ToString() + ", " + y2.ToString() + "],";
            pathWithOffset += "[[" + (x2 + offsetx).ToString() + ", " + (y2 + offsety).ToString() + "],";
            bool b = RecursiveSolve(startX, startY, offsetx, offsety);

            if(b) {
                if (path.EndsWith(","))
                    path = path.Substring(0, path.Length - 1);
                path += "]";
                if (pathWithOffset.EndsWith(","))
                    pathWithOffset = pathWithOffset.Substring(0, pathWithOffset.Length - 1);
                pathWithOffset += "]";
            }
            return b;
        }

        private bool RecursiveSolve(int x, int y, int offsetx, int offsety)
        {
            if (x == endX && y == endY) return true; // If you reached the end
            if (maze[x][y] == 2 || wasHere[x][y]) return false;
            // If you are on a wall or already were here
            wasHere[x][y] = true;
            if (x != 0) // Checks if not on left edge
                if (RecursiveSolve(x - 1, y, offsetx, offsety))
                { // Recalls method one to the left
                    correctPath[x][y] = true; // Sets that path value to true;
                    path += "[" + y.ToString() + ", " + x.ToString() + "],";
                    pathWithOffset += "[" + (y + offsety).ToString() + ", " + (x + offsetx).ToString() + "],";
                    return true;
                }
            if (x != columns - 1) // Checks if not on right edge
                if (RecursiveSolve(x + 1, y, offsetx, offsety))
                { // Recalls method one to the right
                    correctPath[x][y] = true;
                    path += "[" + y.ToString() + ", " + x.ToString() + "],";
                    pathWithOffset += "[" + (y + offsety).ToString() + ", " + (x + offsetx).ToString() + "],";
                    return true;
                }
            if (y != 0)  // Checks if not on top edge
                if (RecursiveSolve(x, y - 1, offsetx, offsety))
                { // Recalls method one up
                    correctPath[x][y] = true;
                    path += "[" + y.ToString() + ", " + x.ToString() + "],";
                    pathWithOffset += "[" + (y + offsety).ToString() + ", " + (x + offsetx).ToString() + "],";
                    return true;
                }
            if (y != rows - 1) // Checks if not on bottom edge
                if (RecursiveSolve(x, y + 1, offsetx, offsety))
                { // Recalls method one down
                    correctPath[x][y] = true;
                    path += "[" + y.ToString() + ", " + x.ToString() + "],";
                    pathWithOffset += "[" + (y + offsety).ToString() + ", " + (x + offsetx).ToString() + "],";
                    return true;
                }
            return false;
        }
    }
}
