using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace BDWeb
{
    public partial class Form1 : Form
    {
        private int track = 0;
        private string conString;
        public Form1()
        {
            InitializeComponent();
            //Create a connection to Oracle
            conString = "User Id=system; password=system;" +

                        //How to connect to an Oracle DB without SQL*Net configuration file
                        //also known as tnsnames.ora.
                        "Data Source=192.168.0.42:1521/XEPDB1; Pooling=false;";
            //chart1.ChartAreas["Defaul"]
        }

        void UpdatePower()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        cmd.CommandText = "begin pack1.update_sm('" + ListUpdatePr.SelectedItem + "'," + txtBUPDATE.Text + "," + comboMapid.SelectedItem + "); end;";
                        OracleDataReader reader = cmd.ExecuteReader();
                        reader.Dispose();
                        MessageBox.Show("Обновлено!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }
        void Proc2()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    con.Open();
                    cmd.BindByName = true;
                    OracleCommand cmd_count = con.CreateCommand();
                    cmd.CommandText = $@"select p.name,sm.prod_id,to_char(sm.dat,'MON') as mon,sm.Dat,sm.men_pers_code,sm.quantity,sm.sale_dat
from salemap sm,pricelist pl,products p
where sm.prod_id=pl.prod_id and p.prod_id=pl.prod_id and pl.dat=sm.dat and EXTRACT(YEAR from sm.dat)=EXTRACT(YEAR from sysdate)
order by EXTRACT(MONTH from sm.dat)";
                    cmd_count.CommandText = $@"select count(*) from (select p.name,sm.prod_id,to_char(sm.dat,'MON') as mon,sm.Dat,sm.men_pers_code,sm.quantity,sm.sale_dat
                    from salemap sm,pricelist pl, products p
                        where sm.prod_id = pl.prod_id and p.prod_id = pl.prod_id and pl.dat = sm.dat and EXTRACT(YEAR from sm.dat)= EXTRACT(YEAR from sysdate)
                    order by EXTRACT(MONTH from sm.dat))";
                    OracleDataReader reader = cmd.ExecuteReader();
                    OracleDataReader reader_count = cmd_count.ExecuteReader();
                    int count = 0;
                    while (reader_count.Read())
                    {
                        dgvProc2.RowCount = int.Parse(reader_count[0].ToString());
                    }

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                            dgvProc2[i, count].Value = reader[i].ToString();
                        count++;
                    }



                }
            }
        }
        private string str;
        void RefreshPL()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.BindByName = true;
                    OracleCommand cmd_count = con.CreateCommand();
                    cmd.CommandText = "SELECT * FROM pricelist order by prod_id";
                    cmd_count.CommandText = "select count(*) from pricelist";
                    OracleDataReader reader = cmd.ExecuteReader();
                    OracleDataReader reader_count = cmd_count.ExecuteReader();
                    int count = 0;
                    while (reader_count.Read())
                    {
                        dgv_pricelist.RowCount = int.Parse(reader_count[0].ToString());
                    }

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                            dgv_pricelist[i, count].Value = reader[i].ToString();
                        count++;
                    }
                }
            }
        }

        void FillMens()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select distinct Men_Pers_Code from salemap order by 1";
                        cmd_count.CommandText = "select count(distinct Men_Pers_Code) from salemap";
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        OracleDataReader reader = cmd.ExecuteReader();
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                comboMensId.Items.Add(reader[i]);
                            }
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }
        void FillSub()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select distinct Subs_id from salemap order by 1";
                        cmd_count.CommandText = "select count(distinct subs_id) from salemap";
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        OracleDataReader reader = cmd.ExecuteReader();
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                comboSubs.Items.Add(reader[i]);
                            }
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }
        void RefreshSmap()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.BindByName = true;
                    OracleCommand cmd_count = con.CreateCommand();
                    cmd.CommandText = "SELECT * FROM SaleMap order by map_id";
                    cmd_count.CommandText = "select count(*) from SaleMap";
                    OracleDataReader reader = cmd.ExecuteReader();
                    OracleDataReader reader_count = cmd_count.ExecuteReader();
                    int count = 0;
                    while (reader_count.Read())
                    {
                        dgv_Smap.RowCount = int.Parse(reader_count[0].ToString());
                    }

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                            dgv_Smap[i, count].Value = reader[i].ToString();
                        count++;
                    }
                }
            }
        }
        void RefreshArch()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select * from ARCHIVE";
                        cmd_count.CommandText = "select count(*) from ARCHIVE";

                        OracleDataReader reader = cmd.ExecuteReader();
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        while (reader_count.Read())
                        {
                            dgv_Archive.RowCount = Convert.ToInt32(reader_count[0].ToString());
                        }
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                dgv_Archive[i, count].Value = (object)reader[i].ToString();
                            }
                            count++;
                        }

                        reader.Dispose();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimeSaleDat.Format = DateTimePickerFormat.Custom;

            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
            dateTimePicker2.CustomFormat = "dd/MM/yyyy";
            dateTimeSaleDat.CustomFormat = "dd/MM/yyyy";

            dgv_Archive.RowHeadersVisible = false;
            dgv_pricelist.RowHeadersVisible = false;
            dgv_Smap.RowHeadersVisible = false;
            dgvProc1.RowHeadersVisible = false;
            dgvProc2.RowHeadersVisible = false;
            RefreshPL();
            RefreshArch();
            RefreshSmap();
        }

        void Fillprid()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select distinct prod_id from pricelist order by 1";
                        cmd_count.CommandText = "select count(distinct prod_id) from pricelist";
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        OracleDataReader reader = cmd.ExecuteReader();
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                comboProdid.Items.Add(reader[i]);
                            }
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == 0)
            {
                comboSubs.Items.Clear();
                FillSub();
                comboMensId.Items.Clear();
                FillMens();
                if (comboProdid.Items.Count == 0)
                    Fillprid();
                button2.Visible = true;

                label7.Visible = true;
                label8.Visible = true;
                label9.Visible = true;
                lbQuat.Visible = true;

                comboProdid.Visible = true;
                txtBx_Map_id.Visible = true;
                txtBx_Quantity.Visible = true;
                comboDat.Visible = true;

                lbMenid.Visible = true;
                lbSD.Visible = true;
                lbSub.Visible = true;
                dateTimeSaleDat.Visible = true;
                comboMensId.Visible = true;
                comboSubs.Visible = true;

                comboMapid.Visible = false;
                listWhere.Visible = false;
                btnUpdateP.Visible = false;
                labelUPDATE.Visible = false;
                labelUPDStates.Visible = false;

                txtBUPDATE.Visible = false;
                ListUpdatePr.Visible = false;

                btnDel.Visible = false;
                label11.Visible = false;
                comboBox5.Visible = false;

                dgvProc2.Visible = false;
                btnList.Visible = false;

                dateTimePicker1.Visible = false;
                dateTimePicker2.Visible = false;

                btnGetGoods.Visible = false;
                numericUpDown1.Visible = false;
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                dgvProc1.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label2.Visible = false;
            }
            if (listBox1.SelectedIndex == 1)
            {
                comboMapid.Items.Clear();
                Fillmapid();
                ListUpdatePr.Visible = true;
                txtBUPDATE.Visible = true;

                comboMapid.Visible = true;
                listWhere.Visible = true;
                btnUpdateP.Visible = true;
                labelUPDATE.Visible = true;
                labelUPDStates.Visible = true;

                button2.Visible = false;

                lbMenid.Visible = false;
                lbSD.Visible = false;
                lbSub.Visible = false;
                dateTimeSaleDat.Visible = false;
                comboMensId.Visible = false;
                comboSubs.Visible = false;

                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                lbQuat.Visible = false;

                comboProdid.Visible = false;
                txtBx_Map_id.Visible = false;
                txtBx_Quantity.Visible = false;
                comboDat.Visible = false;

                btnDel.Visible = false;
                label11.Visible = false;
                comboBox5.Visible = false;

                dgvProc2.Visible = false;
                btnList.Visible = false;

                dateTimePicker1.Visible = false;
                dateTimePicker2.Visible = false;

                btnGetGoods.Visible = false;
                numericUpDown1.Visible = false;
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                dgvProc1.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label2.Visible = false;
            }
            if (listBox1.SelectedIndex == 2) // del_from_smap
            {
                comboBox5.Items.Clear();
                FillDelete();
                btnDel.Visible = true;
                label11.Visible = true;
                comboBox5.Visible = true;
                lbMenid.Visible = false;
                lbSD.Visible = false;
                lbSub.Visible = false;
                dateTimeSaleDat.Visible = false;
                comboMensId.Visible = false;
                comboSubs.Visible = false;

                txtBUPDATE.Visible = false;
                ListUpdatePr.Visible = false;

                button2.Visible = false;

                comboMapid.Visible = false;
                listWhere.Visible = false;
                btnUpdateP.Visible = false;
                labelUPDATE.Visible = false;
                labelUPDStates.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                lbQuat.Visible = false;

                comboProdid.Visible = false;
                txtBx_Map_id.Visible = false;
                txtBx_Quantity.Visible = false;
                comboDat.Visible = false;

                dgvProc2.Visible = false;
                btnList.Visible = false;

                dateTimePicker1.Visible = false;
                dateTimePicker2.Visible = false;

                btnGetGoods.Visible = false;
                numericUpDown1.Visible = false;
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                dgvProc1.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label2.Visible = false;
            }
            if (listBox1.SelectedIndex == 3)
            {

                txtBUPDATE.Visible = false;
                ListUpdatePr.Visible = false;
                lbMenid.Visible = false;
                lbSD.Visible = false;
                lbSub.Visible = false;
                dateTimeSaleDat.Visible = false;
                comboMensId.Visible = false;
                comboSubs.Visible = false;
                btnDel.Visible = false;
                label11.Visible = false;
                comboBox5.Visible = false;

                btnList.Visible = false;

                comboMapid.Visible = false;
                listWhere.Visible = false;
                btnUpdateP.Visible = false;
                labelUPDATE.Visible = false;
                labelUPDStates.Visible = false;

                button2.Visible = false;
                dgvProc2.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                lbQuat.Visible = false;

                comboProdid.Visible = false;
                txtBx_Map_id.Visible = false;
                txtBx_Quantity.Visible = false;
                comboDat.Visible = false;

                dgvProc1.Visible = true;
                dateTimePicker1.Visible = true;
                dateTimePicker2.Visible = true;

                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label2.Visible = true;

                btnGetGoods.Visible = true;
                numericUpDown1.Visible = true;
                radioButton1.Visible = true;
                radioButton2.Visible = true;
            }
            if (listBox1.SelectedIndex == 4)
            {
                txtBUPDATE.Visible = false;
                ListUpdatePr.Visible = false;
                lbMenid.Visible = false;
                lbSD.Visible = false;
                lbSub.Visible = false;
                dateTimeSaleDat.Visible = false;
                comboMensId.Visible = false;
                comboSubs.Visible = false;
                dgvProc2.Visible = true;
                btnList.Visible = true;

                comboMapid.Visible = false;
                listWhere.Visible = false;
                btnUpdateP.Visible = false;
                labelUPDATE.Visible = false;
                labelUPDStates.Visible = false;

                btnDel.Visible = false;
                label11.Visible = false;
                comboBox5.Visible = false;

                button2.Visible = false;
                dgvProc1.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                lbQuat.Visible = false;

                comboProdid.Visible = false;
                txtBx_Map_id.Visible = false;
                txtBx_Quantity.Visible = false;
                comboDat.Visible = false;

                dateTimePicker1.Visible = false;
                dateTimePicker2.Visible = false;

                btnGetGoods.Visible = false;
                numericUpDown1.Visible = false;
                radioButton1.Visible = false;
                radioButton2.Visible = false;

                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label2.Visible = false;
            }
        }


        private void btnGetGoods_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value.Date < dateTimePicker2.Value.Date)
            {
                if (radioButton2.Checked)
                {
                    using (OracleConnection con = new OracleConnection(conString))
                    {
                        using (OracleCommand cmd = con.CreateCommand())
                        {
                            try
                            {
                                con.Open();
                                cmd.BindByName = true;
                                OracleCommand cmd_count = con.CreateCommand();
                                cmd.CommandText = "Select * from (Select p.name as Name, to_char(sm.sale_dat, 'mm.dd.yyyy') as Data, p.PG_id as PGroup, pl.price as PRICE from salemap sm, pricelist pl, products p where sm.prod_id = pl.prod_id and p.prod_id = pl.prod_id and pl.dat = sm.dat and sm.sale_dat BETWEEN '" + dateTimePicker1.Text + "' and ' " + dateTimePicker2.Text + "' and p.PG_ID = " + numericUpDown1.Text + " group by p.name, to_char(sm.sale_dat, 'mm.dd.yyyy'), p.PG_id, pl.price order by PRICE desc)";

                                cmd_count.CommandText = "Select Count(*) from (Select p.name as Name, to_char(sm.sale_dat, 'mm.dd.yyyy') as Data, p.PG_id as PGroup, pl.price as PRICE from salemap sm, pricelist pl, products p where sm.prod_id = pl.prod_id and p.prod_id = pl.prod_id and pl.dat = sm.dat and sm.sale_dat BETWEEN '" + dateTimePicker1.Text + "' and ' " + dateTimePicker2.Text + "' and p.PG_ID = " + numericUpDown1.Text + " group by p.name, to_char(sm.sale_dat, 'mm.dd.yyyy'), p.PG_id, pl.price order by PRICE desc)";

                                OracleDataReader reader = cmd.ExecuteReader();
                                OracleDataReader reader_count = cmd_count.ExecuteReader();
                                while (reader_count.Read())
                                {
                                    dgvProc1.RowCount = Convert.ToInt32(reader_count[0].ToString());
                                }
                                int count = 0;
                                while (reader.Read())
                                {
                                    string s = String.Empty;
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        dgvProc1[i, count].Value = (object)reader[i].ToString();
                                    }
                                    count++;
                                }
                                reader.Dispose();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
                if (radioButton1.Checked)
                {
                    using (OracleConnection con = new OracleConnection(conString))
                    {
                        using (OracleCommand cmd = con.CreateCommand())
                        {
                            try
                            {
                                con.Open();
                                cmd.BindByName = true;
                                OracleCommand cmd_count = con.CreateCommand();
                                cmd.CommandText = "Select * from (Select p.name as Name, to_char(sm.sale_dat, 'mm.dd.yyyy') as Data, p.PG_id as PGroup, pl.price as PRICE from salemap sm, pricelist pl, products p where sm.prod_id = pl.prod_id and p.prod_id = pl.prod_id and pl.dat = sm.dat and sm.sale_dat BETWEEN '" + dateTimePicker1.Text + "' and ' " + dateTimePicker2.Text + "' and p.PG_ID = " + numericUpDown1.Text + " group by p.name, to_char(sm.sale_dat, 'mm.dd.yyyy'), p.PG_id, pl.price order by PRICE )";

                                cmd_count.CommandText = "Select Count(*) from (Select p.name as Name, to_char(sm.sale_dat, 'mm.dd.yyyy') as Data, p.PG_id as PGroup, pl.price as PRICE from salemap sm, pricelist pl, products p where sm.prod_id = pl.prod_id and p.prod_id = pl.prod_id and pl.dat = sm.dat and sm.sale_dat BETWEEN '" + dateTimePicker1.Text + "' and ' " + dateTimePicker2.Text + "' and p.PG_ID = " + numericUpDown1.Text + " group by p.name, to_char(sm.sale_dat, 'mm.dd.yyyy'), p.PG_id, pl.price order by PRICE )";

                                OracleDataReader reader = cmd.ExecuteReader();
                                OracleDataReader reader_count = cmd_count.ExecuteReader();
                                while (reader_count.Read())
                                {
                                    dgvProc1.RowCount = Convert.ToInt32(reader_count[0].ToString());
                                }
                                int count = 0;
                                while (reader.Read())
                                {
                                    string s = String.Empty;
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        dgvProc1[i, count].Value = (object)reader[i].ToString();
                                    }
                                    count++;
                                }
                                reader.Dispose();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
            }
            else
                MessageBox.Show("Первая дата должна быть меньше конечной");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Создана информационная система, позволяющая добавлять, изменять, удалять и просматривать информацию о поступлениях товаров.\n" +
                            "Во вкладке процедуры, пользователь может добавить, удалить или изменить данные.\n Помимо этого пользователь может:\n 1)Посмотреть список поступлений товаров, группированный по месяцм текущего года, с указанием для каждого из них полной информации о поступившем товаре.\n 2)Список всех товаров определенной группы, поступивших в определенный временной период с указанием убывания/возрастания стоимости.\n" +
                            "Также стоит запрет на удаление и изменение для товаров, поступивших в текущем месяце или если менее, чем 3 рабочих дня от текущей даты."
                            );

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboProdid.SelectedItem == null || txtBx_Map_id.Text == "" || txtBx_Quantity.Text == "" || comboMensId.SelectedItem == null || comboSubs.SelectedItem == null)
                MessageBox.Show("Не все поля заполнены.\nПосмотрите в карту продаж для просмотра данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            string end = "";

            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        if (comboDat.SelectedItem == null)
                            MessageBox.Show("Выберите дату");
                        else
                        {
                            string s = comboDat.SelectedItem.ToString();

                            for (int i = 0; i < s.Length - 8; i++)
                            {
                                end += s[i];
                            }
                        }
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "INSERT INTO SALEMAP (MAP_id,PROD_id,DAT,SUBS_id,MEN_PERS_CODE,QUANTITY,SALE_DAT) values (" + txtBx_Map_id.Text + "," + comboProdid.SelectedItem + ",'" + end + "'," + comboSubs.SelectedItem + "," + comboMensId.SelectedItem + "," + txtBx_Quantity.Text + ",'" + dateTimeSaleDat.Text + "')";
                        OracleDataReader reader = cmd.ExecuteReader();
                        reader.Dispose();
                        MessageBox.Show("Добавлено!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
            dgv_Smap.Rows.Clear();
            RefreshSmap();
            RefreshArch();
        }
        private void btnList_Click(object sender, EventArgs e)
        {
            dgvProc2.Rows.Clear();
            Proc2();
        }

        void Fillmapid()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select map_id from salemap";
                        cmd_count.CommandText = "select count(map_id) from salemap";
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        OracleDataReader reader = cmd.ExecuteReader();
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                comboMapid.Items.Add(reader[i]);
                            }
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }
        void FillDelete()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select map_id from salemap";
                        cmd_count.CommandText = "select count(map_id) from salemap";
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        OracleDataReader reader = cmd.ExecuteReader();
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                comboBox5.Items.Add(reader[i]);
                            }
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }
        private void btnDel_Click(object sender, EventArgs e)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "begin pack1.del_from_smap(" + comboBox5.SelectedItem + "); end;";
                        OracleDataReader reader = cmd.ExecuteReader();
                        reader.Dispose();
                        MessageBox.Show("Удалено!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }

            comboBox5.SelectedItem = null;
            comboBox5.Items.Clear();
            FillDelete();
            RefreshSmap();
            RefreshArch();
        }

        private void btnUpdateP_Click(object sender, EventArgs e)
        {
            try
            {
                if (ListUpdatePr.SelectedItem == null || comboMapid.SelectedItem == null || txtBUPDATE.Text == "")
                {
                    MessageBox.Show("Не все критерии выбраны");
                }
                else if (Convert.ToInt32(txtBUPDATE.Text) <= 0)
                {
                    MessageBox.Show("Индексы начинаются с 1");
                }
                else if (ListUpdatePr.SelectedIndex == 0 && (Convert.ToInt32(txtBUPDATE.Text) > 4 || Convert.ToInt32(txtBUPDATE.Text) < 1))
                {
                    MessageBox.Show("Subs_id варьируется от 1 до 4");
                }
                else if (ListUpdatePr.SelectedIndex == 1 && (Convert.ToInt32(txtBUPDATE.Text) > 999 || Convert.ToInt32(txtBUPDATE.Text) < 995))
                {
                    MessageBox.Show("Men_Pers_code варьируется от 995 до 999");
                }
                else if (ListUpdatePr.SelectedIndex == 0 && txtBUPDATE.Text == dgv_Smap[3, Convert.ToInt32(comboMapid.SelectedIndex) ].Value.ToString())
                {
                    MessageBox.Show("Нельзя обновить, введены существующие данные!");
                }
                else if (ListUpdatePr.SelectedIndex == 1 && txtBUPDATE.Text == dgv_Smap[4, Convert.ToInt32(comboMapid.SelectedIndex) ].Value.ToString())
                {
                    MessageBox.Show("Нельзя обновить, введены существующие данные!");
                }
                else if (ListUpdatePr.SelectedIndex == 2 && txtBUPDATE.Text == dgv_Smap[5, Convert.ToInt32(comboMapid.SelectedIndex) ].Value.ToString())
                {
                    MessageBox.Show("Нельзя обновить, введены существующие данные!");
                }
                else
                {

                    UpdatePower();
                    dgv_Archive.Rows.Clear();
                    dgv_Smap.Rows.Clear();
                    RefreshArch();
                    RefreshSmap();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ListUpdatePr_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void FillCB4Subsid()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select distinct Subs_id from salemap order by 1";
                        cmd_count.CommandText = "select count(distinct subs_id) from salemap";
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        OracleDataReader reader = cmd.ExecuteReader();
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                comboBox4.Items.Add(reader[i]);
                            }
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }
        void FillCB4MenPersCode()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select distinct Men_Pers_Code from salemap order by 1";
                        cmd_count.CommandText = "select count(distinct Men_Pers_Code) from salemap";
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        OracleDataReader reader = cmd.ExecuteReader();
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                comboBox4.Items.Add(reader[i]);
                            }
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }
        void FillCB4PRID()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select distinct prod_id from salemap order by 1";
                        cmd_count.CommandText = "select count(distinct prod_id) from salemap order by 1";
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        OracleDataReader reader = cmd.ExecuteReader();
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                comboBox4.Items.Add(reader[i]);
                            }
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }

        void SortSMProdid()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select * from salemap where prod_id=" + comboBox4.SelectedItem + "";
                        cmd_count.CommandText = "select count(*) from salemap where prod_id=" + comboBox4.SelectedItem + "";
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader_count.Read())
                        {
                            dgv_Smap.RowCount = Convert.ToInt32(reader_count[0].ToString());
                        }
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                dgv_Smap[i, count].Value = (object)reader[i].ToString();
                            }
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }

        void SortSMSubsId()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select * from salemap where Subs_id=" + comboBox4.SelectedItem + "";
                        cmd_count.CommandText = "select count(*) from salemap where Subs_id=" + comboBox4.SelectedItem + "";
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader_count.Read())
                        {
                            dgv_Smap.RowCount = Convert.ToInt32(reader_count[0].ToString());
                        }
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                dgv_Smap[i, count].Value = (object)reader[i].ToString();
                            }
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }

        void SortSMMenPersCode()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select * from salemap where Men_pers_code=" + comboBox4.SelectedItem + "";
                        cmd_count.CommandText = "select count(*) from salemap where Men_pers_code=" + comboBox4.SelectedItem + "";
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader_count.Read())
                        {
                            dgv_Smap.RowCount = Convert.ToInt32(reader_count[0].ToString());
                        }
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                dgv_Smap[i, count].Value = (object)reader[i].ToString();
                            }
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }
        void FillCB3()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select distinct prod_id from pricelist ";
                        cmd_count.CommandText = "select count(distinct prod_id) from pricelist ";
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        OracleDataReader reader = cmd.ExecuteReader();
                        //while (reader_count.Read())
                        //{
                        //    dgv_pricelist.RowCount = Convert.ToInt32(reader_count[0].ToString());
                        //}
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                comboBox3.Items.Add(reader[i]);
                            }
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }

        void fillDat()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select distinct dat from salemap where prod_id=" + comboProdid.SelectedItem + "";
                        cmd_count.CommandText = "select count(distinct dat) from salemap where prod_id=" + comboProdid.SelectedItem + "";
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        OracleDataReader reader = cmd.ExecuteReader();
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                comboDat.Items.Add(reader[i]);
                            }
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }
        void FillCB3PR()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select distinct price from pricelist order by 1";
                        cmd_count.CommandText = "select count(distinct price) from pricelist";
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        OracleDataReader reader = cmd.ExecuteReader();
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                comboBox3.Items.Add(reader[i]);
                            }
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }

        void SortProdid()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select * from pricelist where prod_id=" + comboBox3.SelectedItem + "";
                        cmd_count.CommandText = "select count(*) from pricelist where prod_id=" + comboBox3.SelectedItem + "";
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader_count.Read())
                        {
                            dgv_pricelist.RowCount = Convert.ToInt32(reader_count[0].ToString());
                        }
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                dgv_pricelist[i, count].Value = (object)reader[i].ToString();
                            }
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }

        void SortPrice()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;
                        OracleCommand cmd_count = con.CreateCommand();
                        cmd.CommandText = "select * from pricelist where price=" + comboBox3.SelectedItem + "";
                        cmd_count.CommandText = "select count(*) from pricelist where price=" + comboBox3.SelectedItem + "";
                        OracleDataReader reader_count = cmd_count.ExecuteReader();
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader_count.Read())
                        {
                            dgv_pricelist.RowCount = Convert.ToInt32(reader_count[0].ToString());
                        }
                        int count = 0;
                        while (reader.Read())
                        {
                            string s = String.Empty;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                dgv_pricelist[i, count].Value = (object)reader[i].ToString();
                            }
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Выберите столбец");
            }
            else
            {
                dgv_pricelist.Rows.Clear();
                if (comboBox2.SelectedItem == "Prod_id")
                {

                    if (comboBox3.SelectedItem != null)
                        SortProdid();
                    else
                        MessageBox.Show("Выберите значение");

                }


                if (comboBox2.SelectedItem == "Price")
                {
                    if (comboBox3.SelectedItem != null)
                        SortPrice();
                    else
                        MessageBox.Show("Выберите значение");
                }

            }

            comboBox3.SelectedItem = null;
        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            RefreshPL();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите столбец");
            }
            else
            {
                if (comboBox4.SelectedItem == null)
                {
                    MessageBox.Show("Выберите значение");
                }
                else
                {
                    dgv_Smap.Rows.Clear();
                    if (comboBox1.SelectedItem == "Prod_id")
                    {
                        SortSMProdid();
                    }
                    if (comboBox1.SelectedItem == "Men_Pers_Code")
                    {
                        SortSMMenPersCode();
                    }
                    if (comboBox1.SelectedItem == "Subs_id")
                    {
                        SortSMSubsId();
                    }

                }
            }

            comboBox4.SelectedItem = null;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            if (comboBox2.SelectedItem == "Prod_id")
                if (comboBox3.Items.Count == 0)
                    FillCB3();
            if (comboBox2.SelectedItem == "Price")
                if (comboBox3.Items.Count == 0)
                    FillCB3PR();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            if (comboBox1.SelectedItem == "Prod_id")
                if (comboBox4.Items.Count == 0)
                    FillCB4PRID();
            if (comboBox1.SelectedItem == "Men_Pers_Code")
                if (comboBox3.Items.Count == 0)
                    FillCB4MenPersCode();
            if (comboBox1.SelectedItem == "Subs_id")
                if (comboBox3.Items.Count == 0)
                    FillCB4Subsid();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            RefreshSmap();
        }

        private void comboProdid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboProdid.SelectedItem == null)
                comboDat.Items.Clear();
            else
            {
                comboDat.SelectedItem = null;
                comboDat.Items.Clear();
                fillDat();
            }

        }

        private void listWhere_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
