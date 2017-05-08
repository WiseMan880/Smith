using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace MergeSort
{
    public partial class SortThread
    {
        public List<int> unsorted;
        public List<int> sorted;
        public AutoResetEvent reset;

        public void threadfun()
        {
            sorted = new List<int>(mergesort(unsorted));
            reset.Set();
        }

        private List<int> mergesort(List<int> m)
        {
            List<int> result = new List<int>();
            List<int> left = new List<int>();
            List<int> right = new List<int>();
            if (m.Count <= 1)
                return m;

            int middle = m.Count / 2;
            for (int i = 0; i < middle; i++)
                left.Add(m[i]);
            for (int i = middle; i < m.Count; i++)
                right.Add(m[i]);
            left = mergesort(left);
            right = mergesort(right);
            if (left[left.Count - 1] <= right[0])
                return append(left, right);
            result = merge(left, right);
            return result;
        }

        private List<int> append(List<int> a, List<int> b)
        {
            List<int> result = new List<int>(a);
            foreach (int x in b)
                result.Add(x);
            return result;
        }

        private List<int> merge(List<int> a, List<int> b)
        {
            List<int> s = new List<int>();
            while (a.Count > 0 && b.Count > 0)
            {

                if (a[0] < b[0])

                {
                    s.Add(a[0]);
                    a.RemoveAt(0);
                }
                else
                {
                    s.Add(b[0]);
                    b.RemoveAt(0);
                }
            }
            while (a.Count > 0)
            {
                s.Add(a[0]);
                a.RemoveAt(0);
            }

            while (b.Count > 0)
            {
                s.Add(b[0]);
                b.RemoveAt(0);
            }


            return s;
        }
    }

    public partial class Form1 : Form
    {
        AutoResetEvent evt = new AutoResetEvent(true);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[] test = Array.ConvertAll(textBox1.Text.Trim().Split(','), s => int.Parse(s));
            SortThread sort = new SortThread();
            sort.unsorted = test.ToList<int>();
            sort.reset = evt;

            Thread td = new Thread(new ThreadStart(sort.threadfun));
            td.Start();
            evt.WaitOne(1000);

            List<int> result = new List<int>(sort.sorted);
            showsort(result);
        }

        public void showsort(List<int> result)

        {
            foreach (int i in result)
            {
                if (textBox2.Text.Trim()==String.Empty)
                    textBox2.Text = i.ToString();
                else
                    textBox2.Text = textBox2.Text + "," + i.ToString();
            }

        }
    }
}