using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace _17599075_PROG7312_Task1
{
    public partial class Inheritance : Form
    {

        public string filepath, folderPath, folderName, AssemblyName, last, an;

        List<MethodsinClasses> numMethods = new List<MethodsinClasses>();
        List<string> inheritanceList = new List<string>();
        List<string> usedInterfaceList = new List<string>();

        public static List<Name> list = new List<Name>();   

       

        public Inheritance()
        {
            InitializeComponent();
        }


        // WRITE TO TEXT FILE
        private void Button2_Click(object sender, EventArgs e)
        {
            List<string> line = new List<string>();
            
            foreach (var item in inheritanceList)
            {
                line.Add(item);
            }
            System.IO.File.WriteAllLines("Inheritance.txt", line);

        }

        private void Inheritance_Load(object sender, EventArgs e)
        {
            this.Size = new Size(1000, 500);

            //loadClasses()
        }

        
        // BROWSE BUTTON
        private void Button1_Click(object sender, EventArgs e)
        {
            //FILE BROWSE
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse Assemblies",

                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = ".exe",
                Filter = "exe|*.exe",
                FilterIndex = 2,
                RestoreDirectory = true,
                ReadOnlyChecked = true,
                ShowReadOnly = true
            };



            DialogResult result = openFileDialog1.ShowDialog(); // Show the browse dialog.
            if (result == DialogResult.OK) // Test result.
            {
                filepath = openFileDialog1.FileName;

                string[] filename = filepath.Split('\\');
                last = filename[filename.Length - 1];
                last.Trim();
                AssemblyName = last.Substring(0, last.Length - 4);
                textBoxInherBrowse.Text = last;

                // SPLIT THE SELECTED PATH INTO PARTS TO NAV TO CLASSES WHICH SHOULD BE IN THE NAMESPACE
                // WHICH IS OUTSIDE OF THE BIN & DEBUG FOLDERS
                an = filename[filename.Length - 4];// string an = *\an[.Length() -4]  \bin[-3]\debug[-2]\an.exe[-1]
                StringBuilder sb = new StringBuilder();

                folderPath = "";

                foreach (var i in filename)
                {
                    sb.Append(i);
                    if (i.Equals(an))
                    {
                        // PATH TO CLASSES IN NAMESPACE
                        folderPath = sb.ToString();
                    }
                    sb.Append("\\");
                }


                // SCREEN SIZE 
                //this.Size = new Size(1000, 800);
                this.CenterToScreen();


                //textBoxInherBrowse.Text = "";
                //clearAll();
                loadObject();
                DisplayClasses();
                if (filepath != null)
                {
                    //ListBox_Output2.Items.Clear();
                    //NumberOfMethodsInClasses();
                }



            }
        }



        // SHOW INHERITANCE OF SELECTED CLASS
        private void SelectedItem(object sender, EventArgs e)
        {
/*
            Assembly assembly2 = Assembly.LoadFrom(filepath);
            Type[] types2 = assembly2.GetTypes();
            numMethods.Clear();


            foreach (Type t in types2)
            {
                MethodInfo[] methodInfo = t.GetMethods();

                // ADD CLASS AND NUM OF METHODS IN SAID CLASS TO A LIST
                numMethods.Add(new MethodsinClasses { className = t.FullName });
            }
*/


            // SHOW INTERFACES
            //listBox3.Items.Clear();

            


            /* Assembly assembly = Assembly.LoadFrom(filepath);
             Type[] types = assembly.GetTypes();

             foreach (Type t in types)
             {
                 List<Type> inheritance = new List<Type>();

                 inheritance = t.GetInterfaces().AsEnumerable().ToList();
                 foreach (Type x in inheritance)
                 {
                     if (selectedItem == x.Name)
                     {
                         listBox2.Items.Add(t.Name);
                     }
                 }
             }*/


            /******************************************************/
           /* var selectedItem = listBoxClasses_Inher.SelectedItem;
            selectedItem.ToString();


            string s = selectedItem.ToString();
            Assembly as = typeof(s).Assembly;

            foreach (Type type in asm.GetType(selectedItem))
            {
                string className = type.FullName;

                Type type2 = Type.GetType(selectedItem);

                while (type != null)
                {

                    type = type.BaseType;


                    inheritanceList.Add(type.ToString());

                }
            }*/







            /* Type type = Type.GetType(selectedItem); //target type
             object o = Activator.CreateInstance(type); // an instance of target type
             Type your = (Type)o;


             TypeConverter typeConverter = TypeDescriptor.GetConverter(propType);
             object propValue = typeConverter.ConvertFromString(selectedItem);
 */


            /* var propType = src.GetType().GetProperty(selectedItem).PropertyType;
             var converter = TypeDescriptor.GetConverter(propType);
             var convertedObject = converter.ConvertFromString(src);*/



            // object propvalue = Convert.ChangeType(selectedItem, Type);


            // INHERITANCE


        }

        // SHOW CLASSES
        private void DisplayClasses()
        {
            foreach (var item in numMethods)
            {
                listBoxClasses_Inher.Items.Add(item.className);
            }
        }

        private void loadObject()
        {            
            try
            {
                Assembly assembly = Assembly.LoadFrom(filepath);
                Type[] types = assembly.GetTypes();
                numMethods.Clear();

                foreach (Type t in types)
                {
                    MethodInfo[] methodInfo = t.GetMethods();

                    // ADD CLASS NAME TO A LIST
                    numMethods.Add(new MethodsinClasses { className = t.FullName });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                MessageBox.Show("Fatal Error!! \n Please choose another folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            //-------SORT------

           /* numMethods.Sort();
            usedInterfaceList.Sort();
            interfaceList.Sort();*/
        }


        // POPULATE TREE
        public static void PopulateTree()
        {
            foreach (var item in list)
            {

                Tree<Name> tree = new Tree<Name>();
                if (tree.Root == null)
                {
                    tree.Root = new TreeNode<Name>() // TREE ROOT ..... "Zoo"
                    {
                        Data = new Name(item.name),
                        Parent = null

                    };
                }
                else if (tree.Root.Childern == null)
                {
                    tree.Root.Childern[0].Childern = new List<TreeNode<Name>>() // CHILDERN OF tree.Root.Childern[0] ..... "Mammals"/"Reptiles"
                    {
                        new TreeNode<Name>() // tree.Root.Childern[0].CHILDREN[0] ..... "Mammals"
                        {
                            Data = new Name(item.name),
                            Parent = tree.Root
                        },
                        new TreeNode<Name>() // tree.Root.Childern[0].CHILDREN[1] ..... "Reptiles"
                        {
                            Data = new Name(item.name),
                            Parent = tree.Root
                        }
                    };

                }
                else if (tree.Root.Childern[1].Childern == null)
                {
                    tree.Root.Childern[1].Childern = new List<TreeNode<Name>>() // CHILDERN OF tree.Root.Childern[0] ..... "Reptiles"
                    {
                        new TreeNode<Name>() // tree.Root.Childern[1].CHILDREN[0] ..... "Lizzards"
                        {
                            Data = new Name(item.name),
                            Parent = tree.Root
                        }

                    };
                }


            }
        }

    }
}
