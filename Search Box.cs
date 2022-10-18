using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Unitversal
{
    public partial class MainWindow : Form
    {
        /// <summary>
        /// Scroll search box contents when search box is overflowed with text.
        /// </summary>
        private void ScrollTextContents(bool Resized)
        {
            //Get width of string in pixels
            int StringWidth = TextRenderer.MeasureText(SearchBox.Text, SearchBox.Font).Width;
            if (StringWidth > SearchBox.Width)
            {
                AppState.EntryMaxed = true;
                //Scrolls content and preserve cursor position after search box becomes smaller from resize
                if (Resized)
                {
                    int CurrentPosition = SearchBox.SelectionStart;
                    SearchBox.SelectionStart = SearchBox.TextLength - 1; //Scroll
                    SearchBox.SelectionStart = CurrentPosition;
                }
            }
            else
            {
                //Scrolls search box contents to beginning when box is no longer overflowed
                if (AppState.EntryMaxed)
                {
                    int CurrentPosition = SearchBox.SelectionStart;
                    SearchBox.SelectionStart = 0; //Scroll
                    //SearchBox.ScrollToCaret();
                    SearchBox.SelectionStart = CurrentPosition;
                    AppState.EntryMaxed = false;
                }
            }
            SearchBox.Focus();
        }
        private static int LongestSubsequence(string a, string b)
        {
            int[,] Matrix = new int[a.Length + 1, b.Length + 1];
            for (int i = 0; i <= a.Length; i++)
            {
                Matrix[i, 0] = 0;
            }
            for (int i = 0; i <= b.Length; i++)
            {
                Matrix[0, i] = 0;
            }
            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    if (a[i - 1] == b[j - 1])
                    {
                        Matrix[i, j] = Matrix[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        Matrix[i, j] = Math.Max(Matrix[i - 1, j], Matrix[i, j - 1]);
                    }
                }
            }
            return Matrix[a.Length, b.Length];
        }
        private static int LongestSubstring(string a, string b)
        {
            int Longest = 0;
            int[,] Matrix = new int[a.Length, b.Length];
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    if (a[i] == b[j])
                    {
                        if (i == 0 || j == 0)
                        {
                            Matrix[i, j] = 1;
                        }
                        else
                        {
                            Matrix[i, j] = Matrix[i - 1, j - 1] + 1;
                        }
                        if (Matrix[i, j] > Longest)
                        {
                            Longest = Matrix[i, j];
                        }
                    }
                    else
                    {
                        Matrix[i, j] = 0;
                    }
                }
            }
            return Longest;
        }
        private static int BestCompare(Tuple<string, int, int> x, Tuple<string, int, int> y)
        {
            if (x.Item1.Length < y.Item1.Length) //Shortest strings rank first
            {
                return -1;
            }
            else if (x.Item1.Length == y.Item1.Length && x.Item3 > y.Item3) //Case sensitive preferred
            {
                return -1;
            }
            else if (x.Item1.Length == y.Item1.Length && x.Item3 == y.Item3) //Alphabetize if same
            {
                return x.Item1.CompareTo(y.Item1);
            }
            return 1;
        }
        /// <summary>
        /// Get best matches of a unit name from database.
        /// </summary>
        /// <returns>
        /// A <see langword="List"/> of the best matches of a unit name from the
        /// database and the number of matching characters as a <see cref="KeyValuePair"/>.
        /// </returns>
        private static List<string> BestMatches(string Unit)
        {
            List<Tuple<string, int, int>> SubsequenceMatches = new List<Tuple<string, int, int>>();
            List<Tuple<string, int, int>> SubstringMatches = new List<Tuple<string, int, int>>();
            int SubsequenceBest = 0;
            int SubstringBest = 0;
            int Casing = 0; //0 means case insensitive, 1 means case sensitive
            int SubsequenceScore;
            int SubstringScore;
            int Case1;
            int Case2;
            int NoCase1;
            int NoCase2;
            foreach (KeyValuePair<string, string> x in AppState.UnitList)
            {
                Case1 = LongestSubsequence(Unit, x.Key); //Case sensitive
                NoCase1 = LongestSubsequence(Unit.ToUpper(), x.Key.ToUpper()); //Case insensitive
                Case2 = LongestSubstring(Unit, x.Key);
                NoCase2 = LongestSubstring(Unit.ToUpper(), x.Key.ToUpper());
                //Determine if case insensitive match is better based on percentage
                if ((float)NoCase1 / x.Key.Length > (float)Case1 / x.Key.Length)
                {
                    SubsequenceScore = NoCase1;
                    Casing = 0;
                }
                else //Case sensitivity is preferred as it best captures meaning of inexact query
                {
                    SubsequenceScore = Case1;
                    Casing = 1;
                }
                if ((float)NoCase2 / x.Key.Length > (float)Case2 / x.Key.Length)
                {
                    SubstringScore = NoCase2;
                    Casing = 0;
                }
                else
                {
                    SubstringScore = Case2;
                    Casing = 1;
                }
                //Add to list only if equal or better than best
                if (SubsequenceScore > 0)
                {
                    if (SubsequenceScore >= SubsequenceBest)
                    {
                        SubsequenceMatches.Add(new Tuple<string, int, int>(x.Key, SubsequenceScore, Casing));
                        SubsequenceBest = SubsequenceScore;
                    }
                }
                if (SubstringScore > 0)
                {
                    if (SubstringScore >= SubstringBest)
                    {
                        SubstringMatches.Add(new Tuple<string, int, int>(x.Key, SubstringScore, Casing));
                        SubstringBest = SubstringScore;
                    }
                }
            }
            if (SubsequenceBest > SubstringBest + 2) //+2 adjustment because longest common substring is preferred
            {
                SubsequenceMatches.RemoveAll(x => x.Item2 != SubsequenceBest);
                SubsequenceMatches.Sort((x, y) => BestCompare(x, y));
                return SubsequenceMatches.ConvertAll(x => x.Item1);
            }
            else //Longest common substring is preferred as it best captures meaning of an inexact query
            {
                SubstringMatches.RemoveAll(x => x.Item2 != SubstringBest);
                SubstringMatches.Sort((x, y) => BestCompare(x, y));
                return SubstringMatches.ConvertAll(x => x.Item1);
            }
        }
        /// <summary>
        /// Get best plural form for a given unit from a given list of plurals.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents the best plural form.
        /// </returns>
        private static string BestPlural(string Unit, List<string> Plurals)
        {
            List<Tuple<string, int>> SubsequenceMatches = new List<Tuple<string, int>>();
            int SubsequenceBest = 0;
            int SubsequenceScore;
            foreach (string x in Plurals)
            {
                SubsequenceScore = LongestSubsequence(Unit, x);
                if (SubsequenceScore >= SubsequenceBest)
                {
                    SubsequenceMatches.Add(new Tuple<string, int>(x, SubsequenceScore));
                    SubsequenceBest = SubsequenceScore;
                }
            }
            SubsequenceMatches.RemoveAll(x => x.Item2 != SubsequenceBest);
            SubsequenceMatches.Sort((x, y) => x.Item1.Length.CompareTo(y.Item1.Length));
            return SubsequenceMatches[0].Item1;
        }
    }
}
