﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17599075_PROG7312_Task1
{
    class TreeNode<T>
    {

        public T Data { get; set; }
        public TreeNode<T> Parent { get; set; }
        public List<TreeNode<T>> Childern { get; set; }
        public int GetHeight()
        {
            int height = 1;
            TreeNode<T> current = this;

            while (current.Parent != null)
            {
                height++;
                current = current.Parent;

            }
            return height;
        }
    }
}
