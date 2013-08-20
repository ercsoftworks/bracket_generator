/**
 * Stats.cs
 * Basic Statistics Library.
 * 
 * Author: Eric Carestia
 * Copyright: EC Computers, LLC.
 * Creation Date: April 21, 2012
 * Version:  1.0.2;
 * 
 * Statistics class implements basic
 * statistical functions (mean, median, quartiles,etc.)
 * 
 * This libary will implement further statistical functions
 * as needed
 * 
 * TO DO: Add skewness fucntion as per Wolfram Research
 * 
 **/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brackets2012
{
    public class Statistics
    {
        private double[] list;

        public Statistics(params double[] list)
        {
            this.list = list;
        }

        public void update(params double[] list)
        {
            this.list = list;
        }

        public double mode()
        {
            try
            {
                double[] i = new double[list.Length];
                list.CopyTo(i, 0);
                sort(i);
                double val_mode = i[0], help_val_mode = i[0];
                int old_counter = 0, new_counter = 0;
                int j = 0;
                for (; j <= i.Length - 1; j++)
                    if (i[j] == help_val_mode)
                        new_counter++;
                    else if (new_counter > old_counter)
                    {
                        old_counter = new_counter;
                        new_counter = 1;
                        help_val_mode = i[j];
                        val_mode = i[j - 1];
                    }
                    else if (new_counter == old_counter)
                    {
                        val_mode = double.NaN;
                        help_val_mode = i[j];
                        new_counter = 1;
                    }
                    else
                    {
                        help_val_mode = i[j];
                        new_counter = 1;
                    }
                if (new_counter > old_counter)
                    val_mode = i[j - 1];
                else if (new_counter == old_counter)
                    val_mode = double.NaN;
                return val_mode;
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

        public int length()
        {
            return list.Length;
        }

        public double min()
        {
            double minimum = double.PositiveInfinity;
            for (int i = 0; i <= list.Length - 1; i++)
                if (list[i] < minimum)
                    minimum = list[i];
            return minimum;
        }

        public double max()
        {
            double maximum = double.NegativeInfinity;
            for (int i = 0; i <= list.Length - 1; i++)
                if (list[i] > maximum)
                    maximum = list[i];
            return maximum;
        }

        public double Q1()
        {
            return Qi(0.25);
        }

        public double Q2()
        {
            return Qi(0.5);
        }

        public double Q3()
        {
            return Qi(0.75);
        }

        public double mean()
        {
            try
            {
                double sum = 0;
                for (int i = 0; i <= list.Length - 1; i++)
                    sum += list[i];
                return sum / list.Length;
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

        public double range()
        {
            double minimum = min();
            double maximum = max();
            return (maximum - minimum);
        }

        public double IQ()
        {
            return Q3() - Q1();
        }

        public double middle_of_range()
        {
            double minimum = min();
            double maximum = max();
            return (minimum + maximum) / 2;
        }

        /// <summary>
        /// Variance()
        /// </summary>
        /// <returns></returns>
        public double variance()
        {
            try
            {
                double s = 0;
                for (int i = 0; i <= list.Length - 1; i++)
                    s += Math.Pow(list[i], 2);
                return (s - list.Length * Math.Pow(mean(), 2)) / (list.Length - 1);
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// standard_dev()
        /// </summary>
        /// <returns></returns>
        public double standard_dev()
        {
            return Math.Sqrt(variance());
        }

        /// <summary>
        /// YULE()
        /// </summary>
        /// <returns></returns>
        public double YULE()
        {
            try
            {
                return ((Q3() - Q2()) - (Q2() - Q1())) / (Q3() - Q1());
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// index_Standard()
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public double index_Standard(double member)
        {
            try
            {
                if (exist(member))
                    return (member - mean()) / standard_dev();
                else
                    return double.NaN;
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// Covariance()
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public double Covariance(Statistics s)
        {
            try
            {
                if (this.length() != s.length())
                    return double.NaN;
                int len = this.length();
                double sum_mul = 0;
                for (int i = 0; i <= len - 1; i++)
                    sum_mul += (this.list[i] * s.list[i]);
                return (sum_mul - len * this.mean() * s.mean()) / (len - 1);
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// Covariance()- Overload 1
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static double Covariance(Statistics s1, Statistics s2)
        {
            try
            {
                if (s1.length() != s2.length())
                    return double.NaN;
                int len = s1.length();
                double sum_mul = 0;
                for (int i = 0; i <= len - 1; i++)
                    sum_mul += (s1.list[i] * s2.list[i]);
                return (sum_mul - len * s1.mean() * s2.mean()) / (len - 1);
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// correlation_coeff()
        /// </summary>
        /// <param name="design"></param>
        /// <returns></returns>
        public double correlation_coeff(Statistics design)
        {
            try
            {
                return this.Covariance(design) / (this.standard_dev() * design.standard_dev());
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// correlation_coeff- Overload 1
        /// </summary>
        /// <param name="design1"></param>
        /// <param name="design2"></param>
        /// <returns></returns>
        public static double correlation_coeff(Statistics design1, Statistics design2)
        {
            try
            {
                return Covariance(design1, design2) / (design1.standard_dev() * design2.standard_dev());
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// a()
        /// "a" factor of linear function of design
        /// </summary>
        /// <param name="design"></param>
        /// <returns></returns>
        public double a(Statistics design)
        {
            try
            {
                return this.Covariance(design) / (Math.Pow(design.standard_dev(), 2));
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// a() Overload 1
        /// </summary>
        /// <param name="design1"></param>
        /// <param name="design2"></param>
        /// <returns></returns>
        public static double a(Statistics design1, Statistics design2)
        {
            try
            {
                return Covariance(design1, design2) / (Math.Pow(design2.standard_dev(), 2));
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// b()
        /// "b" factor of linear design of function
        /// </summary>
        /// <param name="design"></param>
        /// <returns></returns>
        public double b(Statistics design)
        {
            return this.mean() - this.a(design) * design.mean();
        }

        /// <summary>
        /// b() - Overload 1
        /// </summary>
        /// <param name="design1"></param>
        /// <param name="design2"></param>
        /// <returns></returns>
        public static double b(Statistics design1, Statistics design2)
        {
            return design1.mean() - a(design1, design2) * design2.mean();
        }

        private double Qi(double i)
        {
            try
            {
                double[] j = new double[list.Length];
                list.CopyTo(j, 0);
                sort(j);
                if (Math.Ceiling(list.Length * i) == list.Length * i)
                    return (j[(int)(list.Length * i - 1)] + j[(int)(list.Length * i)]) / 2;
                else
                    return j[((int)(Math.Ceiling(list.Length * i))) - 1];
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// Accessor Method Sort();
        /// </summary>
        /// <param name="i"></param>
        public void sort(double[] i)
        {
            double[] temp = new double[i.Length];
            merge_sort(i, temp, 0, i.Length - 1);
        }

        /// <summary>
        /// merge_Sort() implementation
        /// </summary>
        /// <param name="source"></param>
        /// <param name="temp"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private void merge_sort(double[] source,
            double[] temp, int left, int right)
        {
            int mid;
            if (left < right)
            {
                mid = (left + right) / 2;
                merge_sort(source, temp, left, mid);
                merge_sort(source, temp, mid + 1, right);
                merge(source, temp, left, mid + 1, right);
            }
        }

        private void merge(double[] source, double[] temp,
            int left, int mid, int right)
        {
            int i, left_end, num_elements, tmp_pos;
            left_end = mid - 1;
            tmp_pos = left;
            num_elements = right - left + 1;
            while ((left <= left_end) && (mid <= right))
            {
                if (source[left] <= source[mid])
                {
                    temp[tmp_pos] = source[left];
                    tmp_pos++;
                    left++;
                }
                else
                {
                    temp[tmp_pos] = source[mid];
                    tmp_pos++;
                    mid++;
                }
            }
            while (left <= left_end)
            {
                temp[tmp_pos] = source[left];
                left++;
                tmp_pos++;
            }
            while (mid <= right)
            {
                temp[tmp_pos] = source[mid];
                mid++;
                tmp_pos++;
            }
            for (i = 1; i <= num_elements; i++)
            {
                source[right] = temp[right];
                right--;
            }
        }

        /// <summary>
        /// exist()
        /// checks a list for a certain member
        /// returns false if the list doesn't contain
        /// the parameter member.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private bool exist(double member)
        {
            bool is_exist = false;
            int i = 0;
            while (i <= list.Length - 1 && !is_exist)
            {
                is_exist = (list[i] == member);
                i++;
            }
            return is_exist;
        }
    }
}
