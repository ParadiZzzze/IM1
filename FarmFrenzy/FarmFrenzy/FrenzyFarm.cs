using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FarmFrenzy
{
    public partial class Form1 : Form
    {
        Dictionary<CheckBox, Cell> field = new Dictionary<CheckBox, Cell>();
        private int day = 0;
        private int inWallet = 100;

        public Form1()
        {
            InitializeComponent();

            foreach (CheckBox cb in tableLayoutPanel1.Controls)
                field[cb] = new Cell();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Checked) Plant(cb);
            else Harvest(cb);
        }


        private void Plant(CheckBox cb)
        {
            CellState state = field[cb].state;
            if (inWallet > 1)
            {
                inWallet -= 2;
                field[cb].Plant();
                UpdateBox(cb);
            }
        }
        private void Harvest(CheckBox cb)
        {
            CellState state = field[cb].state;
            if (state == CellState.Rotten)
            {
                if (inWallet == 0)
                {
                    return;
                }
                else inWallet -= 1;
            }
            if (state == CellState.Planted)
                inWallet += 1;
            if (state == CellState.Green)
                inWallet += 2;
            if (state == CellState.Immature)
                inWallet += 3;
            if (state == CellState.Mature)
                inWallet += 5;           
            field[cb].Harvest();
            UpdateBox(cb);
            
        }

       
        private void NextStep(CheckBox cb)
        {
            field[cb].NextStep();
            UpdateBox(cb);
        }
        private void UpdateBox(CheckBox cb)
        {
            Color c = Color.White;
            switch (field[cb].state)
            {
                case CellState.Planted: c = Color.Black;
                    break;
                case CellState.Green: c = Color.Green;
                    break;
                case CellState.Immature: c = Color.Yellow;
                    break;
                case CellState.Mature: c = Color.Red;
                    break;
                case CellState.Rotten: c = Color.Brown;
                    break;
            }
            cb.BackColor = c;
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            foreach (CheckBox cb in tableLayoutPanel1.Controls)
                NextStep(cb);
            day++;
            labDay.Text = "Day: " + day;
            SpeedLabel.Text = "SpeedNow: " + Convert.ToString(timer1.Interval);
            Wallet.Text = inWallet + " rub.";
        }

     
        private void DownSpeed_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (timer1.Interval <= 900)
                {
                    timer1.Interval += 100;
                }
            timer1.Enabled = true;
        }

        private void UpSpeed_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (timer1.Interval >= 101)
            {
                timer1.Interval -= 100;
            }
            timer1.Enabled = true;
        }
    }
    enum CellState
    {
        Empty,
        Planted,
        Green,
        Immature,
        Mature,
        Rotten
    }

    class Cell
    {
        public CellState state = CellState.Empty;
        private int progress = 0;

        const int prPlanted = 20;
        const int prGreen = 100;
        const int prImmature = 120;
        const int prMature = 140;

        public void Plant()
        {            
                state = CellState.Planted;                        
        }

        public void Harvest()
        {
            
            state = CellState.Empty;
            progress = 0;            
        }

        public void NextStep()
        {
            if ((state != CellState.Empty) && (state != CellState.Rotten))
            {
                progress++;
                if ((progress == prPlanted) || (progress == prGreen) || (progress == prImmature) || (progress == prMature)) 
            state++;
            }
        }
    }
}