using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MIDTERM
{
    public partial class Form1 : Form
    {
        //4-11-21 SH NEW 1L: Create a List to store the panels
        List<Panel> listPanel = new List<Panel>();

        //4-11-21 SH NEW 1L: Create a Queue to store the products
        Queue<Product> productQueue = new Queue<Product>();

        //4-11-21 SH NEW 1L: Create a Queue to store the orders
        Queue<Order> orderQueue = new Queue<Order>();

        //4-12-21 SH NEW 2L: add the two files for the information
        public static string path = @"productlist.txt";
        public static string pathO = @"orderlist.txt";

        public static FileInfo fi;

        Stream streamP;
        Stream streamO;

        IFormatter formatter = new BinaryFormatter();

        public Form1()
        {
            InitializeComponent();
        }

        //4-12-21 SH NEW 33L: add panels + read product file + read order file
        private void Form1_Load(object sender, EventArgs e)
        {
            //4-11-21 SH NEW 2L:Open the file and give the program the ablility to read it
            fi = new FileInfo(path);
            streamP = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
            //4-11-21 SH NEW 4L: deserialize and read the info from the file
            if (new FileInfo(path).Length != 0)
            {
                productQueue = (Queue<Product>)formatter.Deserialize(streamP);
            }
            //4-11-21 SH NEW 1L: Clost the file and stop reading
            streamP.Close();


            //4-11-21 SH NEW 2L:Open the file and give the program the ablility to read it
            fi = new FileInfo(pathO);
            streamO = new FileStream(pathO, FileMode.OpenOrCreate, FileAccess.Read);
            //4-11-21 SH NEW 4L: deserialize and read the info from the file
            if (new FileInfo(pathO).Length != 0)
            {
                orderQueue = (Queue<Order>)formatter.Deserialize(streamO);
            }
            //4-11-21 SH NEW 1L: Clost the file and stop reading
            streamO.Close();


            //4-12-21 SH NEW 5L: add the panels
            listPanel.Add(panel1);
            listPanel.Add(panel2);
            listPanel.Add(panel3);
            listPanel.Add(panel4);
            listPanel[0].BringToFront();
        }

        //4-12-21 SH NEW 5L: when Add product is clicked the id is set + panel is brought to front
        private void buttonProduct_Click(object sender, EventArgs e)
        {
            idIn.Text = (productQueue.Count + 1).ToString();
            listPanel[1].BringToFront();
        }

        //4-12-21 SH NEW 35L: when submit is clicked the form is saved
        private void buttonSub1_Click(object sender, EventArgs e)
        {
            //4-12-21 SH NEW 22L: form only submits if all fields are filled
            if (nameIn.Text != "" && priceIn.Text != "" && desIn.Text != "")
            {
                //4-12-21 SH NEW 7L: input is assign to the object
                productQueue.Enqueue(new Product
                {
                    Id = Int32.Parse(idIn.Text),
                    Name = nameIn.Text.Trim(),
                    Price = Int32.Parse(priceIn.Text),
                    Description = desIn.Text.Trim()
                });


                //4-12-21 SH NEW 3L: save the file
                streamP = new FileStream(path, FileMode.Open, FileAccess.Write);
                formatter.Serialize(streamP, productQueue);
                streamP.Close();


                //4-12-21 SH NEW 1L: alert the user the information was submited
                Success<string>(nameIn.Text.Trim());
            }
            else
                Error<string>(); //4-12-21 SH NEW 1L: alert the user a field is empty


            //4-12-21 SH NEW 4L: clear fields and update id
            idIn.Text = (productQueue.Count + 1).ToString();
            nameIn.Text = "";
            priceIn.Text = "";
            desIn.Text = "";
        }

        //4-12-21 SH NEW 4L: go back to home page
        private void buttonMenu1_Click(object sender, EventArgs e)
        {
            listPanel[0].BringToFront();
        }

        //4-12-21 SH NEW 23L: when Add Order is clicked set id + bring panel to front + read productQueue
        private void buttonOrder_Click(object sender, EventArgs e)
        {
            //4-12-21 SH NEW 2L: bring panel to front + set id
            listPanel[2].BringToFront();
            idOut.Text = (orderQueue.Count + 1).ToString();


            //4-11-21 SH NEW 2L:Open the file and give the program the ablility to read it
            fi = new FileInfo(path);
            streamP = new FileStream(path, FileMode.Open, FileAccess.Read);
            //4-11-21 SH NEW 4L: deserialize and read the info from the file
            if (new FileInfo(path).Length != 0)
            {
                productQueue = (Queue<Product>)formatter.Deserialize(streamP);
            }
            //4-12-21 SH NEW 4L: add products to drop down
            foreach ( Product p in productQueue)
            {
                dropProduct.Items.Add(p.Name);
            }
            //4-11-21 SH NEW 1L: Clost the file and stop reading
            streamP.Close();
        }

        //4-12-21 SH NEW 33L: Only 1 save button because the program creates the file
        private void buttonSub2_Click(object sender, EventArgs e)
        {
            if (dropProduct.Text != "" && priceOut.Text != "" && custName.Text != "")
            {
                //4-12-21 SH NEW 7L: input is assigned to the object
                orderQueue.Enqueue(new Order
                {
                    OId = Int32.Parse(idOut.Text),
                    OName = custName.Text.Trim(),
                    OPrice = Int32.Parse(priceOut.Text),
                    OProduct = dropProduct.Text
                });


                //4-12-21 SH NEW 3L: save file
                streamO = new FileStream(pathO, FileMode.Open, FileAccess.Write);
                formatter.Serialize(streamO, orderQueue);
                streamO.Close();


                //4-12-21 SH NEW 1L: alert the user the information saved
                SuccessO<string>(custName.Text.Trim());
            }
            else
                Error<string>();//4-12-21 SH NEW 1L: alert the user a field is empty


            //4-12-21 SH NEW 4L: clear fields + update id
            idOut.Text = (orderQueue.Count + 1).ToString();
            custName.Text = "";
            priceOut.Text = "";
            dropProduct.Text = "";
        }

        //4-12-21 SH NEW 8L: set the price when product is changed
        private void dropProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(Product p in productQueue)
            {
                if(dropProduct.Text == p.Name)
                    priceOut.Text = p.Price.ToString();
            }
        }

        //4-12-21 SH NEW 4L: go back to menu
        private void buttonMenu3_Click(object sender, EventArgs e)
        {
            listPanel[0].BringToFront();
        }

        //4-12-21 SH NEW 24L: print the orders
        private void buttonTrans_Click(object sender, EventArgs e)
        {
            //4-12-21 SH NEW 1L: bring panel to front
            listPanel[3].BringToFront();


            //4-12-21 SH NEW 2L:Open the file and give the program the ablility to read it
            fi = new FileInfo(pathO);
            streamO = new FileStream(pathO, FileMode.Open, FileAccess.Read);
            //4-12-21 SH NEW 4L: deserialize and read the info from the file
            if (new FileInfo(pathO).Length != 0)
            {
                orderQueue = (Queue<Order>)formatter.Deserialize(streamO);
            }
            //4-12-21 SH NEW 1L: Clost the file and stop reading
            streamO.Close();


            //4-12-21 SH NEW 4L: print each object in orderQueue
            foreach (Order order in orderQueue)
            {
                labelOutput.Text += order.OId + " - " + order.OName + " bought a " + order.OProduct + " Pizza for $" + order.OPrice + "\n";
            }
        }

        //4-12-21 SH NEW 4L: go back to menu
        private void buttonMenu2_Click(object sender, EventArgs e)
        {
            listPanel[0].BringToFront();
        }

        //4-12-21 SH NEW 4L: product success alert
        private void Success<T>(T data)
        {
            MessageBox.Show($"{data} was saved!", "Success!", MessageBoxButtons.OK);
        }
        //4-12-21 SH NEW 4L: order success alert
        private void SuccessO<T>(T data)
        {
            MessageBox.Show($"{data}'s order was saved!", "Success!", MessageBoxButtons.OK);
        }
        //4-12-21 SH NEW 4L: error alert
        private void Error<T>()
        {
            MessageBox.Show($"Please Fill All Fields!", "Empty fields", MessageBoxButtons.OK);
        }
    }
}
