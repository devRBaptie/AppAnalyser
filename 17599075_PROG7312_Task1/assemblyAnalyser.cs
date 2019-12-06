using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _17599075_PROG7312_Task1
{
    public partial class assemblyAnalyser : Form 
    {

        public string filepath, folderPath, folderName, AssemblyName, last, an;
        
        List<MethodsinClasses> numMethods = new List<MethodsinClasses>();
        List<string> interfaceList = new List<string>();
        List<string> usedInterfaceList = new List<string>();

        //FilePath filePath = new FilePath();

        public assemblyAnalyser()
        {
            InitializeComponent();
        }

       
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(550, 150);
        }

       
        // ---------------------BROWSE BUTTON---------------------
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
                textBox1.Text = last;

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

                this.Size = new Size(1250, 800);
                this.CenterToScreen();

                textBox2.Text = "";
                clearAll();
                loadObject();
                DisplayAssembly();
                if (filepath != null)
                {
                    ListBox_Output2.Items.Clear();
                    NumberOfMethodsInClasses();
                }

            }

            /*
             //FOLDER BROWSE
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                Description = "Folder Search",
                RootFolder = Environment.SpecialFolder.Desktop      
            };

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderBrowserDialog1.SelectedPath;

                // SPLIT THE SELECTED PATH INTO PARTS
                string[] filename = folderPath.Split('\\');
                last = filename[filename.Length - 1];
                last.Trim();
                AssemblyName = last.Substring(0, last.Length - 4);
                textBox1.Text = last;
            }
            */

        }

        // ---------------------CLEARS ALL INPUT AND OUTPUT FIELDS + ALL DATA IN LISTS---------------------
        public void clearAll()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            listBox_Interface_Output_Acceptable.Items.Clear();
            listBox_Interface_Output_Unacceptable.Items.Clear();
            ListBox_Output.Items.Clear();
            ListBox_Output2.Items.Clear();
            textBox2.Clear();
            regexInput.Clear();
            numMethods.Clear();
            interfaceList.Clear();
            usedInterfaceList.Clear();

        }

        //POPULAT OBJECT
        public void loadObject()
        {
            try
            {               
                //----------RETRIEVES ALL INTERFACES NOT USED-------------
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes());

                foreach (var item in types)
                {
                    if (item.IsInterface)
                    {
                        interfaceList.Add(item.Name);

                    }
                }  
            }
            catch (Exception e)
            {

                //throw;
            }


            // -------------ASSEMBLY-------------
            try
            {
                Assembly assembly = Assembly.LoadFrom(filepath); 
                Type[] types = assembly.GetTypes();
                numMethods.Clear();


                foreach (Type t in types)
                {
                    MethodInfo[] methodInfo = t.GetMethods();

                    // NUM OF METHODS IN CLASS          
                    int c = methodInfo.Length;

                    int classLines = CountNumberOfLinesInCSFile(folderPath + "\\" + t.Name + ".cs");

                    // ADD CLASS AND NUM OF METHODS IN SAID CLASS TO A LIST
                    numMethods.Add(new MethodsinClasses { className = t.Name, methodNum = c, numClassLines = classLines, avLines= classLines/c });


                    

                    // -------------INTERFACE-------------
                    // GETS IMPLEMENTED INTERFACES + ADDS TO A LIST
                    Type[] types1 = t.GetInterfaces();
                    foreach (Type item in types1)
                    {
                        usedInterfaceList.Add(item.Name);
                    }
                }
            }
            catch (Exception e)
            {
                /*MessageBox.Show(e.ToString());
                MessageBox.Show("Fatal Error!! \n Please choose another folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;*/
            }
            //-------SORT------
            numMethods.Sort();
            usedInterfaceList.Sort();
            interfaceList.Sort();

            
        }

        

        // ---------------------DISPLAY EXTERNAL REFRENCE NAME + VERSION NUMBER---------------------
        public void DisplayAssembly()
        {
            try
            {
                ListBox_Output.Items.Clear();
                listBox_Interface_Output_Acceptable.Items.Clear();
                // GET REFRENCES
                Assembly assembly = Assembly.LoadFrom(filepath);

                AssemblyName[] Name = assembly.GetReferencedAssemblies();

                List<RefList> l = new List<RefList>();
                // ASSEMBLY NAME + VERSION NUMBER
                foreach (var item in Name)
                {
                    l.Add(new RefList { refName = item.Name, version = item.Version.ToString() });

                }
                l.OrderBy(x => x.refName);
                // CLASSES IN ASSEMBLY
                foreach (var item in l)
                {
                    ListBox_Output.Items.Add(item.ToString());
                }
                // INTERFACES IN ASSEMBLY
                foreach (var item in interfaceList)
                {
                    listBox_Interface_Output_Unacceptable.Items.Add(item);
                }
                // INTERFACES IN ASSEMBLY IN USE
                foreach (var item in usedInterfaceList)
                {
                    listBox_Interface_Output_Acceptable.Items.Add(item);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Oops something wong\nTry again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }



        }

        // ---------------------DISPLAYS NUMBER OF METHODS IN EACH CLASS---------------------
        public void NumberOfMethodsInClasses()
        {

            foreach (var item in numMethods)
            {
                ListBox_Output2.Items.Add(item.ToString()); ;

            }

        }

        // ---------------------Acceptable max av num of lines---------------------
        private void Button5_Click(object sender, EventArgs e)
        {
            try
            {
                // CHANGE WINDOW SIZE
                //this.Size = new Size(1115, 650);
                this.CenterToScreen();

                int i = int.Parse(textBox2.Text);
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                foreach (var item in numMethods)
                {
                    if (item.avLines <= i)
                    {
                        listBox1.Items.Add(item.className + " has an average of " + item.avLines + " lines.");
                    }
                    else
                    {
                        listBox2.Items.Add(item.className + " has an average of " + item.avLines + " lines.");
                    }
                    
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Oops something wong\nTry again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }    

        // ---------------------SORT BY AVERAGE NUMBER OF LINES PER METHOD---------------------
        private void Button6_Click(object sender, EventArgs e)
        {
            ListBox_Output2.Items.Clear();
            numMethods.Sort();
            foreach (var item in numMethods)
            {
                ListBox_Output2.Items.Add(item.ToString()); ;

            }
        }

        // ---------------------SORT BY NUMBER OF METHODS IN CLASS---------------------
        private void Button4_Click(object sender, EventArgs e)
        {
            ListBox_Output2.Items.Clear();
            numMethods.Sort((x, y) => y.methodNum.CompareTo(x.methodNum));
            foreach (var item in numMethods)
            {
                ListBox_Output2.Items.Add(item.ToString()); ;

            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Inheritance inheritance = new Inheritance();
            inheritance.Show();
        }




        //------------------------SELECTED COMPLIANT INTERFACE---------------------
        //SHOWS WHICH CLASSES IMPLEMENT THE INTERFACE
        private void ListBox_Interface_Output_Acceptable_SelectedIndexChanged(object sender, EventArgs e)
        {

            listBox3.Items.Clear();
            var selectedItem = listBox_Interface_Output_Acceptable.SelectedItem;

            Assembly assembly = Assembly.LoadFrom(filepath);
            Type[] types = assembly.GetTypes();

            foreach (Type t in types)
            {
                List<Type> interfaces = new List<Type>();

                interfaces = t.GetInterfaces().AsEnumerable().ToList();
                foreach (Type x in interfaces)
                {
                    if (selectedItem == x.Name)
                    {                        
                        listBox3.Items.Add(t.Name);
                    }
                }
            }
        }

        //---------------------REGEX PATTERN---------------------
        private void Button2_Click(object sender, EventArgs e)
        {
            listBox5.Items.Clear();
            listBox4.Items.Clear();

            // Create a pattern -- [I]\w+ -- input returns all interfaces starting with 'I'
            string pattern = @"\b(" + regexInput.Text + @")\b";
            // Create a Regex  
            Regex rg = new Regex(pattern, RegexOptions.IgnoreCase);

            foreach (var item in usedInterfaceList)
            {
                if (Regex.Match(item, pattern).Success)
                {
                    listBox4.Items.Add(item);
                }
                else
                {
                    listBox5.Items.Add(item);
                }
            }
            foreach (var item in interfaceList)
            {
                if (Regex.Match(item, pattern).Success)
                {
                    listBox4.Items.Add(item);
                }
                else
                {
                    listBox5.Items.Add(item);
                }
            }
        }


        //------------------COUNT LINES-----------------
        private int CountNumberOfLinesInCSFile(string dirPath)
        {
            int totalNumberOfLines = 0;

            FileSystemInfo file = new FileInfo(dirPath.Trim());

            Interlocked.Add(ref totalNumberOfLines, CountNumberOfLine(file));
            

            return totalNumberOfLines;
        }

       /* private int CountNumberOfLinesInCSFilesOfDirectory(string dirPath)
        {
            FileInfo[] csFiles = new DirectoryInfo(dirPath.Trim())
                                        .GetFiles("*.cs", SearchOption.AllDirectories);

            int totalNumberOfLines = 0;
            Parallel.ForEach(csFiles, fo =>
            {
                Interlocked.Add(ref totalNumberOfLines, CountNumberOfLine(fo));
            });
            return totalNumberOfLines;
        }*/

        private int CountNumberOfLine(Object tc)
        {
            try
            {
                FileInfo fo = (FileInfo)tc;
                int count = 0;
                int inComment = 0;

                using (StreamReader sr = fo.OpenText())
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (IsRealCode(line.Trim(), ref inComment))
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
            catch (FileNotFoundException e)
            {
                //MessageBox.Show("File not found!! \n Please choose another class", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            return 0;
        }        

        private bool IsRealCode(string trimmed, ref int inComment)
        {

            /*if (trimmed.Contains("void")| trimmed.Contains("return"))
            {
                return true;
            }*/
            if (trimmed.StartsWith("/*") && trimmed.EndsWith("*/"))
                return false;
            else if (trimmed.StartsWith("/*"))
            {
                inComment++;
                return false;
            }
            else if (trimmed.EndsWith("*/"))
            {
                inComment--;
                return false;
            }

            return
                   inComment == 0
                && !trimmed.StartsWith("//")
                && (trimmed.StartsWith("if")
                    || trimmed.StartsWith("else if")
                    || trimmed.StartsWith("using (")
                    || trimmed.StartsWith("else  if")
                    || trimmed.Contains(";")
                    || trimmed.StartsWith("public") //method signature
                    || trimmed.StartsWith("private") //method signature
                    || trimmed.StartsWith("protected") //method signature
                    );
        }

    }
}
