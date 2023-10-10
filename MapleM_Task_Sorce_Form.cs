using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using System.Xml.Linq;

// ver1.12
// リファクタリングはしてません
namespace MapleStoryM_Task
{    
    public partial class Form1 : Form
    {
        public static int form_Height;
        public static int NowDay;
        public bool weekchaged;
        public int eruda;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
            DateTime dateTime = DateTime.Now;
            NowDay = dateTime.Day;
            Console.WriteLine(NowDay);
            weekchaged = false;
        }
        //
        //初期化処理
        //
        public void Init()
        {
            Form form = new Form();

            string filePath = "data.csv";

            if (File.Exists(filePath))
            {
                
            } else
            {
                MessageBox.Show("ファイルが見つかりませんでした。", "警告", MessageBoxButtons.OK);
                return;
            }
            StreamReader sr = new StreamReader("data.csv");
            int lines = int.Parse(sr.ReadLine());
            string str = sr.ReadLine();
           
            string[] whdata = str.Split(',');
            int[] n = new int[5];
            string[] jw = new string[9];
            n[0] = int.Parse(whdata[0]);
            n[1] = int.Parse(whdata[1]);
            n[2] = int.Parse(whdata[2]);
            n[3] = int.Parse(whdata[3]);
            n[4] = int.Parse(whdata[13]); //エルダ
            jw[0] = whdata[4].ToString();
            jw[1] = whdata[5].ToString();
            jw[2] = whdata[6].ToString();
            jw[3] = whdata[7].ToString();
            jw[4] = whdata[8].ToString();
            jw[5] = whdata[9].ToString();
            jw[6] = whdata[10].ToString();
            jw[7] = whdata[11].ToString();
            jw[8] = whdata[12].ToString();

            this.Location = new Point(n[0],n[1]);
            this.Width = n[2];
            this.Height = n[3];

            //エルダの表示
            eruda = n[4];
            erudaShow();            

            this.Column_Red.Visible = Convert.ToBoolean(jw[0]);
            this.Column_Blue.Visible = Convert.ToBoolean(jw[1]);
            this.Column_Green.Visible = Convert.ToBoolean(jw[2]);
            this.Column_Yellow.Visible = Convert.ToBoolean(jw[3]);
            this.Column_Purple.Visible = Convert.ToBoolean(jw[4]);
            this.Column_jacm.Visible = Convert.ToBoolean(jw[5]);
            this.Column_honetail.Visible = Convert.ToBoolean(jw[6]);
            this.Column_pink.Visible = Convert.ToBoolean(jw[7]);
            this.Column_signus.Visible = Convert.ToBoolean(jw[8]);

            form_Height = lines * 23;

            string line = "";
            string[] array = new string[17];
  
            //残りの全行読み込み
            for(int i = 0; i < lines; i++)
            {
                line =  sr.ReadLine();

                array = line.Split(',');

                int m = array[0].Length;
                if(m < 2)
                {
                    array[0] = "0" + array[0];
                }
                if (array[1] == null) { array[1] = ""; }
                    dataGridView1.Rows.Add(array[0].ToString(), array[1],
                        Convert.ToBoolean(array[2]), Convert.ToBoolean(array[3]),
                        Convert.ToBoolean(array[4]), Convert.ToBoolean(array[5]),
                        Convert.ToBoolean(array[6]), Convert.ToBoolean(array[7]),
                        Convert.ToBoolean(array[8]), Convert.ToBoolean(array[9]),
                        Convert.ToBoolean(array[10]), Convert.ToBoolean(array[11]),
                        array[12].ToString(), array[13].ToString(),
                        array[14].ToString(), array[15].ToString(), array[16].ToString());
            }
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        //
        //上へボタンの処理
        //
        private void 上へToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0) { return; }
            int selectrow = dataGridView1.CurrentRow.Index;
            if (selectrow == 0) { return; }

            //データの一時退避
            string[] temp = new string[17];
            bool[]    temp2 = new bool[17];
            temp[0] = dataGridView1.Rows[selectrow - 1].Cells[0].Value.ToString();
            temp[1] = dataGridView1.Rows[selectrow - 1].Cells[1].Value.ToString();

            for (int i = 2; i < 12; i++)
            {
                temp2[i] = Convert.ToBoolean(dataGridView1.Rows[selectrow - 1].Cells[i].Value);
            }

            for (int i = 12; i < 17; i++)
            {
                temp[i] = dataGridView1.Rows[selectrow - 1].Cells[i].Value.ToString();
            }
            //上の行に選択した行をコピー
            for (int i = 1; i < 17; i++)
            {
                dataGridView1.Rows[selectrow - 1].Cells[i].Value = dataGridView1.Rows[selectrow].Cells[i].Value;
            }
            //元の行に退避データを移動
            dataGridView1.Rows[selectrow].Cells[1].Value = temp[1];

            //bool型を移動
            for (int i = 2; i < 12; i++)
            {
                dataGridView1.Rows[selectrow].Cells[i].Value = temp2[i];
            }

            //string型を移動
            for (int i = 13; i < 17; i++)
            {
                dataGridView1.Rows[selectrow].Cells[i].Value = temp[i];
            }
            //選択行を上に移動
            dataGridView1.CurrentCell = dataGridView1.Rows[selectrow-1].Cells[0];
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        //
        //フォームをクリックされたときの処理
        //
        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            var selectrow = dataGridView1.CurrentRow.Index;
            this.dataGridView1.EndEdit();
        }
        //
        //下へボタンの処理
        //
        private void 下へDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0) { return; }
            //一番下なら処理しない
            if (dataGridView1.CurrentRow.Index + 1 >= dataGridView1.Rows.Count) { return; }

            int selectrow = dataGridView1.CurrentRow.Index;
            var zero = dataGridView1.Rows[selectrow].Cells[0].Value.ToString();

            //一時退避
            string[] temp = new string[17];            
            bool[] temp2 = new bool[17];

            temp[0] = dataGridView1.Rows[selectrow + 1].Cells[0].Value.ToString();
            temp[1] = dataGridView1.Rows[selectrow + 1].Cells[1].Value.ToString();

            //bool型を移動
            for (int i = 2; i < 12; i++)
            {
                temp2[i] = Convert.ToBoolean(dataGridView1.Rows[selectrow + 1].Cells[i].Value);
            }

            //string型を移動
            for (int i = 12; i < 17; i++)
            {
                temp[i] = dataGridView1.Rows[selectrow + 1].Cells[i].Value.ToString();
            }

            // 上の行に選択した行をコピー
            for (int i = 1; i < 17; i++)
                {
                    dataGridView1.Rows[selectrow+1].Cells[i].Value = dataGridView1.Rows[selectrow].Cells[i].Value;
                }

            // 選択行に退避したデータを格納
            dataGridView1.Rows[selectrow].Cells[0].Value = zero.ToString();
            dataGridView1.Rows[selectrow].Cells[1].Value = temp[1];

            for (int i = 2; i < 12; i++)
            {
                dataGridView1.Rows[selectrow].Cells[i].Value = temp2[i];
            }

            for (int i = 12; i < 17; i++)
            {
                dataGridView1.Rows[selectrow].Cells[i].Value = temp[i];
            }
            //選択行を下に移動
            dataGridView1.CurrentCell = dataGridView1.Rows[selectrow + 1].Cells[0];
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        //
        //削除ボタンの処理
        //
        private void 削除EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count == 0) { return; }
            int selectrow = dataGridView1.CurrentRow.Index;
            var id = dataGridView1.Rows[selectrow].Cells[0].Value;
            var name = dataGridView1.Rows[selectrow].Cells[1].Value;

            DialogResult dr = MessageBox.Show
                ("ID：「" + id + "」\n" + name + "を削除しますか？", "確認", MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
            if (dr == DialogResult.OK)
            {
                int n = selectrow;

                //行数の最後から指定位置までIDを修正する
                for(int i = dataGridView1.Rows.Count-1; i > n ; i--)
                {
                    dataGridView1.Rows[i].Cells[0].Value = dataGridView1.Rows[i-1].Cells[0].Value;
                }     
                //指定行を削除
                dataGridView1.Rows.RemoveAt(selectrow);

                if (this.Height > 18)
                {
                    this.Height -= 26;
                    if(this.Height < 18)
                    {
                        this.Height = 18;
                    }
                }
            }
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        //
        //保存ボタンの処理
        //
        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //即反映
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            int n = dataGridView1.Rows.Count;

            //ファイルの有無を確認して、有なら上書きを尋ねる
            StreamReader sr = new StreamReader("data.csv");

            if (sr != null)
            {
                DialogResult dr = MessageBox.Show("ファイルを上書きしますか？", "確認", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No) {
                    sr.Close();
                    return;
                }
            }
            //一旦閉じる
            sr.Close();
            
            //書き込み先指定
            StreamWriter file = new StreamWriter("data.csv", false, Encoding.UTF8);

            // ファイルに書き込む
            file.WriteLine(n);
            file.WriteLine(this.Location.X + "," + this.Location.Y + ","
                + this.Width.ToString() + "," + this.Height.ToString() + ","
                + this.Column_Red.Visible.ToString() + ","
                + this.Column_Blue.Visible.ToString() + ","
                + this.Column_Green.Visible.ToString() + ","
                + this.Column_Yellow.Visible.ToString() + ","
                + this.Column_Purple.Visible.ToString() + ","
                + this.Column_jacm.Visible.ToString() + ","
                + this.Column_honetail.Visible.ToString() + ","
                + this.Column_pink.Visible.ToString() + ","
                + this.Column_signus.Visible.ToString() + "," + eruda);

            for (int i = 0; i < n; i++)
            {
                for(int j = 0; j < 17; j++)
                {
                    file.Write(dataGridView1.Rows[i].Cells[j].Value);
                    if (j < 16)
                    {
                        file.Write(",");
                    }
                    else if(j == 16)
                    {
                        file.Write("\n");
                    }
                }
            }            
            // ファイルを閉じる
            file.Close();
        }
        //
        //データが更新されたとき
        //
        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {            
            var dataGridView1 = sender as DataGridView;
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //コミットされていない内容がある
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        //
        //追加ボタンの処理
        //
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dataGridView1.Sort(Column_id,
                     System.ComponentModel.ListSortDirection.Ascending);
            if (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height-300 > this.Height)
            {
                this.Height += 26;
            }
            int n = dataGridView1.Rows.Count;
            string g = "0";

            if (n < 9)
            {
                g = ("0" + (n+1)).ToString();
            }
            else
            {
                g = (n+1).ToString();
            }
                dataGridView1.Rows.Add(g, " ", false, false,
                                                false, false, false,
                                                    false, false, false, false, false,
                                                    "", "", "", "", "");
            dataGridView1.CurrentCell = dataGridView1.Rows[n].Cells[0];
        }
        //
        //開くボタンの処理
        //
        private void 開くkOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show
               ("ファイルを開きますか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                this.dataGridView1.Rows.Clear();
                Init();
            }
        }

        private void 日課の初期化ToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0) { return; }
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DialogResult dr = MessageBox.Show
               ("日課を初期化しますか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                int n = dataGridView1.Rows.Count;
                for (int i = 0; i < n; i++)
                {
                    dataGridView1.Rows[i].Cells[2].Value = 0;
                }
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void エルダの初期化ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0) { return; }
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DialogResult dr = MessageBox.Show
               ("エルダを初期化しますか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                int n = dataGridView1.Rows.Count;
                for (int i = 0; i < n; i++)
                {
                    dataGridView1.Rows[i].Cells[4].Value = 0;
                    dataGridView1.Rows[i].Cells[5].Value = 0;
                    dataGridView1.Rows[i].Cells[6].Value = 0;
                }
            }
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        private void 週間の初期化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0) { return; }
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DialogResult dr = MessageBox.Show
               ("週間を初期化しますか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                int n = dataGridView1.Rows.Count;
                for (int i = 0; i < n; i++)
                {
                    dataGridView1.Rows[i].Cells[3].Value = 0;
                }
            }
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            toolStripTimerLabel.Text = d.ToString();
            if (dataGridView1.Rows.Count == 0) { return; }
            if (NowDay != d.Day)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                int n = dataGridView1.Rows.Count;
                for (int i = 0; i < n; i++)
                {
                    dataGridView1.Rows[i].Cells[2].Value = false;
                }
                NowDay = d.Day;                
            }
            //週間のリセット
            if (d.DayOfWeek == DayOfWeek.Monday && weekchaged == false)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                int m = dataGridView1.Rows.Count;
                //週間欄のリセット
                for (int i = 0; i < m; i++)
                    {
                        dataGridView1.Rows[i].Cells[3].Value = false;
                    }                
                //エルダ欄のリセット
                for (int i = 0; i < m; i++)
                {
                    dataGridView1.Rows[i].Cells[4].Value = false;
                    dataGridView1.Rows[i].Cells[5].Value = false;
                    dataGridView1.Rows[i].Cells[6].Value = false;
                }
                weekchaged = true;
            }            
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void 赤ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.Column_Red.Visible == true) { this.Column_Red.Visible = false; }
            else
            {
                this.Column_Red.Visible = true;
            }
        }

        private void 青ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Column_Blue.Visible == true) { this.Column_Blue.Visible = false; }
            else
            {
                this.Column_Blue.Visible = true;
            }
        }

        private void 緑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Column_Green.Visible == true) { this.Column_Green.Visible = false; }
            else
            {
                this.Column_Green.Visible = true;
            }
        }

        private void 黄色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Column_Yellow.Visible == true) { this.Column_Yellow.Visible = false; }
            else
            {
                this.Column_Yellow.Visible = true;
            }
        }

        private void 紫ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Column_Purple.Visible == true) { this.Column_Purple.Visible = false; }
            else
            {
                this.Column_Purple.Visible = true;
            }
        }

        private void ジャクムToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Column_jacm.Visible == true) { this.Column_jacm.Visible = false; }
            else
            {
                this.Column_jacm.Visible = true;
            }
        }

        private void ホーンテイルToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.Column_honetail.Visible == true) { this.Column_honetail.Visible = false; }
            else
            {
                this.Column_honetail.Visible = true;
            }
        }

        private void ピンクビーンToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Column_pink.Visible == true) { this.Column_pink.Visible = false; }
            else
            {
                this.Column_pink.Visible = true;
            }
        }

        private void シグナスToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Column_signus.Visible == true) { this.Column_signus.Visible = false; }
            else
            {
                this.Column_signus.Visible = true;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if(eruda == 0) { eruda++; }
            else if(eruda == 1) { eruda++; }
            else if(eruda == 2) { eruda++; }
            else
            {
                eruda = 0;
            }
            erudaShow();
        }

        private void erudaShow()
        {
            switch (eruda)
            {
                case 0:
                    this.Column_Eluda1.Visible = false;
                    this.Column_Eluda2.Visible = false;
                    this.Column_Eluda3.Visible = false;
                    break;
                case 1:
                    this.Column_Eluda1.Visible = true;
                    this.Column_Eluda2.Visible = false;
                    this.Column_Eluda3.Visible = false;
                    break;
                case 2:
                    this.Column_Eluda1.Visible = true;
                    this.Column_Eluda2.Visible = true;
                    this.Column_Eluda3.Visible = false;
                    break;
                case 3:
                    this.Column_Eluda1.Visible = true;
                    this.Column_Eluda2.Visible = true;
                    this.Column_Eluda3.Visible = true;
                    break;
            }
        }
    }
}
