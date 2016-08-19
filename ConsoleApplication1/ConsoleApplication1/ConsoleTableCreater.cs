﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public class ConsoleTableCreater
    {
        /// <summary>
        /// This will hold the header of the table.
        /// </summary>

        private string[] header;

        /// <summary>
        /// This will hold the rows (lines) in the table, not including the
        /// header. I'm using a List of lists because it's easier to deal with...
        /// </summary>

        private List<List<string>> rows;

        /// <summary>
        /// This is the default element (character/string) that will be put
        /// in the table when user adds invalid data, example:
        ///     ConsoleTable ct = new ConsoleTable();
        ///     ct.AddRow(new List<string> { null, "bla", "bla" });
        /// That null will be replaced with "DefaultElement", also, empty
        /// strings will be replaced with this value.
        /// </summary>

        private const string DefaultElement = "X";

        public enum AlignText
        {
            ALIGN_RIGHT,
            ALIGN_LEFT,
        }

        public ConsoleTableCreater()
        {
            header = null;
            rows = new List<List<string>>();
            TextAlignment = AlignText.ALIGN_LEFT;
        }

        /// <summary>
        /// Set text alignment in table cells, either RIGHT or LEFT.
        /// </summary>
        public AlignText TextAlignment { get; set; }

        public void SetHeaders(string[] h)
        {
            header = h;
        }

        public void AddRow(List<string> row)
        {
            rows.Add(row);
        }

        private void AppendLine(StringBuilder hsb, int length)
        {

            hsb.Append(" ");
            hsb.Append(new string('-', length - 4));
            hsb.Append("\r\n");
        }

        /// <summary>
        /// This function returns the maximum possible length of an
        /// individual row (line). Of course that if we use table header,
        /// the maximum length of an individual row should equal the
        /// length of the header.
        /// </summary>
        private int GetMaxRowLength()
        {

            if (header != null)
                return header.Length;
            else
            {
                var maxlen = rows[0].Count;

                for (var i = 1; i < rows.Count; i++)
                {

                    if (rows[i].Count > maxlen) maxlen = rows[i].Count;
                }

                return maxlen;
            }

        }
        
        private void PutDefaultElementAndRemoveExtra()
        {
            var maxlen = GetMaxRowLength();

            foreach (List<string> t in rows)
            {
                // If we find a line that is smaller than the biggest line,
                // we'll add DefaultElement at the end of that line. In the end
                // the line will be as big as the biggest line.

                if (t.Count < maxlen)
                {

                    var loops = maxlen - t.Count;
                    for (var k = 0; k < loops; k++) t.Add(DefaultElement);

                }
                else if (t.Count > maxlen)
                {

                    // This will apply only when header != null, and we try to
                    // add a line bigger than the header line. Remove the elements
                    // of the line, from right to left, until the line is equal
                    // with the header line.

                    t.RemoveRange(maxlen, t.Count - maxlen);

                }

                // Find bad data, loop through all table elements.

                for (var j = 0; j < t.Count; j++)

                {

                    if (t[j] == null)
                        t[j] = DefaultElement;
                    else if (t[j] == "")
                        t[j] = DefaultElement;

                }
            }
        }


        /// <summary>
        /// This function will return an array of integers, an element at
        /// position 'i' will return the maximum length from column 'i'
        /// of the table (if we look at the table as a matrix).
        /// </summary>
        private int[] GetWidths()
        {

            int[] widths = null;

            if (header != null)
            {
                // Initially we assume that the maximum length from column 'i'
                // is exactly the length of the header from column 'i'.

                widths = new int[header.Length];

                for (int i = 0; i < header.Length; i++)
                    widths[i] = header[i].ToString().Length;

            }
            else
            {

                var count = GetMaxRowLength();

                widths = new int[count];

                for (var i = 0; i < count; i++)
                    widths[i] = -1;
            }

            foreach (List<string> s in rows)
            {
                for (int i = 0; i < s.Count; i++)
                {
                    s[i] = s[i].Trim();

                    if (s[i].Length > widths[i]) widths[i] = s[i].Length;
                }
            }

            return widths;
        }



        /// <summary>
        /// Returns a valid format that is to be passed to AppendFormat
        /// member function of StringBuilder.
        /// </summary>
        /// <param name="widths">The array of widths presented above.</param>
        private string BuildRowFormat(int[] widths)
        {
            var rowFormat = String.Empty;

            for (int i = 0; i < widths.Length; i++)
            {

                if (TextAlignment == AlignText.ALIGN_LEFT)
                    rowFormat += "|{" + i.ToString() + ",-" + (widths[i] + 2) + "}";

                else
                    rowFormat += "|{" + i.ToString() + "," + (widths[i] + 2) + "}";
                //{0,7}

            }

            rowFormat = rowFormat.Insert(rowFormat.Length, "|\r\n");
            return rowFormat;
        }

        /// <summary>
        /// Prints the table, main function.
        /// </summary>
        public void PrintTable()
        {

            if (rows.Count == 0)
            {
                Console.WriteLine("Can't create a table without any rows.");
                return;
            }


            PutDefaultElementAndRemoveExtra();

            var widths = GetWidths();

            var rowFormat = BuildRowFormat(widths);


            // I'm using a temporary string builder to find the total width

            // of the table, and increase BufferWidth of Console if necessary.

            StringBuilder toFindLen = new StringBuilder();

            toFindLen.AppendFormat(rowFormat, (header == null ? rows[0].ToArray() : header));

            int length = toFindLen.Length;

            if (Console.BufferWidth < length)
                Console.BufferWidth = length;

            // Print the first row, or header (if it exist), you can see that AppendLine

            // is called before/after every AppendFormat.

            StringBuilder hsb = new StringBuilder();

            AppendLine(hsb, length);

            hsb.AppendFormat(rowFormat, (header == null ? rows[0].ToArray() : header));

            AppendLine(hsb, length);

            // If header does't exist, we start from 1 because the first row

            // was already printed above.

            var idx = 0;

            if (header == null)
                idx = 1;

            for (int i = idx; i < rows.Count; i++)
            {

                hsb.AppendFormat(rowFormat, rows[i].ToArray());
                AppendLine(hsb, length);
            }

            Console.WriteLine(hsb.ToString());
        }
    }
}

