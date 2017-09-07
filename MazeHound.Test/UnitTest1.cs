using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace MazeHound.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestLoadMaze()
        {
            TextReader reader = new StreamReader("Hound Maze(tsv).txt");
            string tsv = reader.ReadToEnd();
            reader.Close();
            MazeHound.Business.MazeHound mh = new MazeHound.Business.MazeHound();
            Assert.IsNotNull(mh.LoadMaze(tsv, 5, 5));

        }
        [TestMethod]
        public void TestSolveMaze()
        {
            TextReader reader = new StreamReader("Hound Maze(tsv).txt");
            string tsv = reader.ReadToEnd();
            reader.Close();
            MazeHound.Business.MazeHound mh = new MazeHound.Business.MazeHound();
            Assert.IsTrue(mh.SolveMaze(tsv, 54, 77, 12, 20, 5, 5));

        }
    }
}
