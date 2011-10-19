using System;
using System.Drawing;
using System.Windows.Forms;

namespace console2
{
    public partial class Form1 : Form
    {
        private readonly MyConsole myConsole;
        public Form1(MyConsole myConsole)
        {
            this.myConsole = myConsole;
            InitializeComponent();
        }

        public RichTextBox getRichTextBox1()
        {
            return richTextBox1;
        }
        public ListBox getMyListBox()
        {
            return listBox1;
        }
        public TextBox getTextBox()
        {
            return textBox1;
        }

        private void form1Load(object sender, EventArgs e)
        {
            richTextBox1.AcceptsTab = true;
            setVisiblityAndDroppingDownAutoCompletionBox(false);
            timer1.Interval = 200;
            
        }

        private void setVisiblityAndDroppingDownAutoCompletionBox(bool flag)
        {
            autoCompletionBox.Visible = flag;
            autoCompletionBox.DroppedDown = flag;
        }
        private void richTextBox1KeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.KeyCode.ToString().Equals("Up") || e.KeyCode.ToString().Equals("Down") || e.KeyCode.ToString().Equals("Tab")))
            {
                setVisiblityAndDroppingDownAutoCompletionBox(false);
                timer1.Interval = 200;
                timer1.Start();
            }
            richBoxEscape(e);
            richboxTab(e);
            richboxDown(e);
            richboxUp(e);
            richboxEnter(e);
        }
        private void richTextBox1PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
       {
           if(e.KeyCode.ToString().Equals("Return"))
           {
               myConsole.interPrete(richTextBox1.Text);
               richTextBox1.Text = "";
               setVisiblityAndDroppingDownAutoCompletionBox(false);
               richTextBox1.SelectionStart = 0;
           }
           Console.Out.WriteLine(e.KeyCode.ToString());
            

       }

        #region keyPresses
        private void richBoxEscape(KeyEventArgs e)
        {
            if (e.KeyCode.ToString().Equals("Escape"))
            {
                timer1.Stop();
                setVisiblityAndDroppingDownAutoCompletionBox(false);
            }
        }

        private void richboxEnter(KeyEventArgs e)
        {
          /*  if (!e.KeyCode.ToString().Equals("Enter")) return;
            textBox1.Text += @"enter" + richTextBox1.SelectionStart;
            e.SuppressKeyPress = true;
            myConsole.interPrete(richTextBox1.Text);
            richTextBox1.Text = "";*/
        }

        private void richboxTab(KeyEventArgs e)
        {
            if (e.KeyCode.ToString().Equals("Tab"))
            {
                e.SuppressKeyPress = true;
                if (!autoCompletionBox.Visible)
                    timer1Tick(null, null);
                if (hasProperAutoCompletionWord())
                {
                    int newPosition;
                    richTextBox1.Text = myConsole.myAutoCompleter.autoCompleteText(richTextBox1.Text, autoCompletionBox.SelectedItem.ToString(),
                                                                                        richTextBox1.SelectionStart, out newPosition);
                    richTextBox1.SelectionStart = newPosition;
                }
                setVisiblityAndDroppingDownAutoCompletionBox(false);
            }
        }

        private void richboxUp(KeyEventArgs e)
        {
            if (e.KeyCode.ToString().Equals("Up"))
            {
                if (autoCompletionBox.Visible && (autoCompletionBox.Items.Count > 0))
                {
                    autoCompletionBox.SelectedItem = autoCompletionBox.Items[autoCompletionBox.Items.Count - 1];
                    e.SuppressKeyPress = true;
                    autoCompletionBox.Focus();
                }
            }
        }

        private void richboxDown(KeyEventArgs e)
        {
            if (e.KeyCode.ToString().Equals("Down"))
            {
                if (autoCompletionBox.Visible && (autoCompletionBox.Items.Count > 0))
                {
                    autoCompletionBox.SelectedItem = autoCompletionBox.Items[0];
                    if (autoCompletionBox.Items.Count > 1)
                        autoCompletionBox.SelectedItem = autoCompletionBox.Items[1];
                    e.SuppressKeyPress = true;
                    autoCompletionBox.Focus();
                }


             
            }
        }
        #endregion


        
        private bool hasProperAutoCompletionWord()
        {
            return autoCompletionBox != null && autoCompletionBox.Items.Count > 0;
        }

        private static Point extraDistanceForBox = new Point(7, 12);
        private void timer1Tick(object sender, EventArgs e)
        {
            var relativePosition = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart);
            autoCompletionBox.Location = new Point(extraDistanceForBox.X + relativePosition.X + richTextBox1.Location.X,
                                            extraDistanceForBox.Y + relativePosition.Y + richTextBox1.Location.Y);
            var listedItems = myConsole.myAutoCompleter.getCompletionItemsOfText(richTextBox1.Text,
                                                                                       richTextBox1.SelectionStart);
            autoCompletionBox.DataSource = listedItems;
            if (hasProperAutoCompletionWord())
            {
                setVisiblityAndDroppingDownAutoCompletionBox(true);
            }
            timer1.Stop();
        }


        private void autoCompletionBoxPreviewKeyDown(object sender,PreviewKeyDownEventArgs e)
        {
             if (e.KeyCode.ToString().Equals("Tab"))
            {
                if (hasProperAutoCompletionWord())
                {
                    int newPosition;
                    richTextBox1.Text = myConsole.myAutoCompleter.autoCompleteText(richTextBox1.Text, autoCompletionBox.SelectedItem.ToString(),
                                                                                        richTextBox1.SelectionStart, out newPosition);
                    richTextBox1.SelectionStart = newPosition;
                }
                setVisiblityAndDroppingDownAutoCompletionBox(false);
            }
            




        }
        private void autoCompletionBoxKeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode.ToString().Equals("Up"))
            {
                if (autoCompletionBox.SelectedIndex > 0)
                {
                    autoCompletionBox.SelectedIndex--;
                }
                else
                {
                    autoCompletionBox.SelectedIndex = autoCompletionBox.Items.Count - 1;
                }
                e.SuppressKeyPress = true;
                return;

            }
            if (e.KeyCode.ToString().Equals("Down"))
            {

                if (autoCompletionBox.SelectedIndex < autoCompletionBox.Items.Count - 1)
                {
                    autoCompletionBox.SelectedIndex++;
                }
                else
                {
                    autoCompletionBox.SelectedIndex = 0;
                }
                e.SuppressKeyPress = true;
                return;
            }
           if (e.KeyCode.ToString().Equals("Tab"))
            {
                
                richboxTab(e);
                return;
            }
            if (e.KeyCode.ToString().Equals("Escape"))
            {
                setVisiblityAndDroppingDownAutoCompletionBox(false);
                richTextBox1.Focus();
                return;
            }

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
