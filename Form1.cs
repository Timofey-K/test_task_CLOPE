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


namespace Test_CLOPE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // создание таблицы для хранения транзакций и их принадлежности к кластерам
            dataSet.Tables.Add(tableWithTransaction);

            DataColumn idColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            idColumn.Unique = true; // столбец будет иметь уникальное значение
            idColumn.AllowDBNull = false; // не может принимать null
            idColumn.AutoIncrement = true; // будет автоинкрементироваться
            idColumn.AutoIncrementSeed = 1; // начальное значение
            idColumn.AutoIncrementStep = 1; // приращении при добавлении новой строки

            DataColumn transactionColumn = new DataColumn("Transaction", Type.GetType("System.String"));
            DataColumn clasterNumberColumn = new DataColumn("ClasterNumber", Type.GetType("System.Double"));

            tableWithTransaction.Columns.Add(idColumn);
            tableWithTransaction.Columns.Add(transactionColumn);
            tableWithTransaction.Columns.Add(clasterNumberColumn);

            tableWithTransaction.PrimaryKey = new DataColumn[] { tableWithTransaction.Columns["Id"] };
        }

        //создание таблицы
        DataSet dataSet = new DataSet("DataSet");
        DataTable tableWithTransaction = new DataTable("DataTable");

        //создание глобального списка кластеров
        List<Claster> clasters = new List<Claster>();
        //создание глобальной переменной r
        double r = 1.1;

        /// <summary>
        /// Открытие файла с транзакциями
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_open_Click(object sender, EventArgs e)
        {
            //сбрасываем состояние программы к начальному состоянию
            tableWithTransaction.Clear();
            rTB_journal.Clear();
            clasters.Clear();
            clope1exec(false);

            string filePath;             //путь к файлу            
            string line;                 //считываемая строка

            //открываем диалог поиска файла
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //устанавливаем начальные настройки, диск C, все файлы
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "All files (*.*)|*.*";

                //если файл выбран, то считываем его строки в таблицу
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();
                    StreamReader sr = new StreamReader(fileStream);
                    line = sr.ReadLine();

                    //пока файл не закончился добавляем строки в таблицу
                    while (line != null)
                    {
                        tableWithTransaction.Rows.Add(new object[] { null, line, null });
                        line = sr.ReadLine();
                    }

                    //выводим все содержимое таблицы в журнал
                    foreach(DataRow row in tableWithTransaction.Rows)
                    {
                        rTB_journal.AppendText(row[1] + "\n");
                    }                   
                }
            }
        }

        /// <summary>
        /// Выполнение кластеризации методом CLOPE, 1 проход, инициализация
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ex_CLOPE_1_Click(object sender, EventArgs e)
        {            
            //попытка считать значение r из textbox
            if (!double.TryParse(tb_withR.Text, out r))
            {
                MessageBox.Show("указано неверное значение r");
                return;
            }

            //используемые переменные
            double maxDelta, delta;     //дельты для поиска максимума
            int numberOfBetterClaster = 0;    //номер кластера c наибольшим профитом

            //проход по всем строкам таблицы
            foreach (DataRow transaction in tableWithTransaction.Rows)
            {
                //сброс максимумов
                maxDelta = 0;
                delta = 0;

                //поиск кластера с максимальным профитом для транзакции
                for (int i = 0; i < clasters.Count; i++)
                {
                    //нахождение профита для текущего кластера
                    delta = deltaAdd(clasters[i], transaction[1], r);

                    //если профита больше максимума, то запоминаем текущий кластер
                    if (maxDelta < delta)
                    {
                        maxDelta = delta;
                        numberOfBetterClaster = i;
                    }
                }

                //проверяем профит от добавления трензакции в новый кластер
                Claster newClaster = new Claster();
                delta = deltaAdd(newClaster, transaction[1], r);

                //если профита от создания нового кластера больше, то создаем новый кластер с текущей транзакцией и записываем в таблицу номер кластера
                if (maxDelta < delta)
                {
                    clasters.Add(new Claster(transaction[1]));
                    transaction[2] = clasters.Count - 1;
                }
                //если меньше, то добавляем транзакцию к подходящему кластеру и записываем номер кластера в таблицу
                else
                {
                    clasters[numberOfBetterClaster].addNewTransaction(transaction[1]);
                    transaction[2] = numberOfBetterClaster;
                }
            }

            //отображаем результат выполнения инициализации в журнале и меняем состояние программы
            showResalt();
            clope1exec(true);
        }

        /// <summary>
        /// Выполнение последующих этапов метода CLOPE, итрации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ex_CLOPE_2_Click(object sender, EventArgs e)
        {
            //подготавливаем необходимые переменные
            bool moved = true;                  
            double maxDelta, delta, removeCost;
            int numberOfBetterClaster, numberCurretClaster;

            //цикл выполнения итераций пока происходят перестановки в кластерах
            while (moved)
            {
                moved = false;

                //проходим по всем строкам таблицы
                foreach (DataRow transaction in tableWithTransaction.Rows)
                {
                    //сброс максимумов
                    maxDelta = 0;
                    delta = 0;



                    //получаем номер текущего кластера и стоимость удаления транзакции из кластера
                    numberCurretClaster = int.Parse(transaction[2].ToString());
                    removeCost = deltaRemove(clasters[numberCurretClaster], transaction[1], r);
                    numberOfBetterClaster = numberCurretClaster;

                    //поиск кластера с максимальным профитом для транзакции при перемещении
                    for (int i = 0; i < clasters.Count; i++)
                    {
                        //просматриваем все кластеры кроме того в котором транзакция лежит
                        if (numberCurretClaster != i)
                        {
                            //нахождение профит для текущего кластера если добавить транзакцию
                            delta = deltaAdd(clasters[i], transaction[1], r);
                        }

                        //если профита больше, то запоминаем  максимальный профит и номер текущего кластера
                        if (maxDelta < (delta+removeCost))
                        {
                            maxDelta = delta+removeCost;
                            numberOfBetterClaster = i;
                        }
                    }

                    //проверяем профит от добавления транзакции в новый кластер
                    Claster newClaster = new Claster();
                    delta = deltaAdd(newClaster, transaction[1], r);

                    //если профита от нового кластера больше, то добавляем транзакцию в него, и удаляем её из старого кластера
                    if (maxDelta < (delta + removeCost))
                    {
                        clasters.Add(new Claster(transaction[1]));
                        clasters[numberCurretClaster].RemoveTransaction(transaction[1]);
                        transaction[2] = clasters.Count - 1;
                        moved = true;
                    }

                    //если меньше, проверяем является ли лучший кластер текущим для транзакции, если нет, то перемещаем транзакцию
                    else if (int.Parse(transaction[2].ToString()) != numberOfBetterClaster)
                    {
                        clasters[numberOfBetterClaster].addNewTransaction(transaction[1]);
                        clasters[numberCurretClaster].RemoveTransaction(transaction[1]);
                        transaction[2] = numberOfBetterClaster;
                        moved = true;
                    }
                }
            }       
            //выводим результаты итераций в жернал
            showResalt();
        }

        /// <summary>
        /// отчищает журнал и выводит результат кластеризации
        /// </summary>
        void showResalt()
        {
            rTB_journal.Clear();
            for (int i = 0; i < clasters.Count; i++)
            {
                rTB_journal.AppendText("Claster №" + i + " contains: " + clasters[i].countTransaction + " transaction" + "\n");
            }                   
        }

        /// <summary>
        /// изменение состояния кнопок при изменении этапа выполнения метода
        /// </summary>
        /// <param name="exec"></param>
        void clope1exec(bool exec)
        {
            if (exec)
            {
                tb_withR.Enabled = false;
                btn_ex_CLOPE_1.Enabled = false;
                btn_ex_CLOPE_2.Enabled = true;
                btn_test_mushrooms.Enabled = true;
            }
            else
            {
                tb_withR.Enabled = true;
                btn_ex_CLOPE_1.Enabled = true;
                btn_ex_CLOPE_2.Enabled = false;
                btn_test_mushrooms.Enabled = false;
            }
        }

        /// <summary>
        /// проверка профита при добавлении транзакции к кластеру
        /// </summary>
        /// <param name="OriginalClaster"></param>
        /// <param name="transaction"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        double deltaAdd(Claster OriginalClaster, object transaction, double r)
        {
            //копируем исходный кластер для расчетов
            Claster testClaster = new Claster(OriginalClaster);

            //разбиваем транзакцию на элементы
            string[] transactionElements = transaction.ToString().Split(',');

            //получаем количество транзакций при добавлении новой транзакции
            testClaster.countTransaction += 1;

            //полчаем новую площадь при добавлении транзакции
            testClaster.square += transactionElements.Length;

            //получаем количество уникальных элементов в кластере и дополняем словарь
            for (int i = 0; i < transactionElements.Length; i++)
            {
                //если элемент для словаря новый, то добавляем его к словарю
                if (!testClaster.DictOcc.ContainsKey(transactionElements[i]))
                {
                    testClaster.DictOcc.Add(transactionElements[i], 1);
                }
                //если элемент уже есть в словаре, то увеличивем его значение
                else
                {
                    testClaster.DictOcc[transactionElements[i]]++;
                }
            }
            testClaster.uniqueItem = testClaster.DictOcc.Count;

            //если создается новый кластер
            if (testClaster.countTransaction == 1)
            {
                return testClaster.square / Math.Pow(testClaster.uniqueItem, r);
            }

            return (testClaster.square * testClaster.countTransaction / Math.Pow(testClaster.uniqueItem, r)) - (OriginalClaster.square * OriginalClaster.countTransaction / Math.Pow(OriginalClaster.uniqueItem, r));
        }


        /// <summary>
        /// проверка профита при удалении транзакции из кластера
        /// </summary>
        /// <param name="OriginalClaster"></param>
        /// <param name="transaction"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        double deltaRemove(Claster OriginalClaster, object transaction, double r)
        {
            //копируем исходный кластер для расчетов
            Claster testClaster = new Claster(OriginalClaster);

            //разбиваем транзакцию на элементы
            string[] transactionElements = transaction.ToString().Split(',');

            //получаем количество транзакций при удалении транзакции
            testClaster.countTransaction -= 1;

            //полчаем новую площадь при удалении транзакции
            testClaster.square -= transactionElements.Length;

            //получаем количество уникальных элементов при удалении элементов транзакции из словаря
            for (int i = 0; i < transactionElements.Length; i++)
            {
                testClaster.DictOcc[transactionElements[i]]--;
                if (testClaster.DictOcc[transactionElements[i]] == 0)
                {
                    testClaster.DictOcc.Remove(transactionElements[i]);
                }
            }
            testClaster.uniqueItem = testClaster.DictOcc.Count;

            return (testClaster.square * testClaster.countTransaction / Math.Pow(testClaster.uniqueItem, r)) - (OriginalClaster.square * OriginalClaster.countTransaction / Math.Pow(OriginalClaster.uniqueItem, r));
        }

        /// <summary>
        /// проверка чистоты кластеризации на примере грибов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_test_mushrooms_Click(object sender, EventArgs e)
        {
            //создаем список кластеров для разиения грибов в них на съедобные и ядовитые
            List<ForCheckOnMushroms> forCheckMushrooms = new List<ForCheckOnMushroms>(clasters.Count);
            for (int i = 0; i < clasters.Count; i++)
            {
                forCheckMushrooms.Add(new ForCheckOnMushroms());
            }

            //проходим по всем транзакциям заполняя список грибов.
            foreach (DataRow transaction in tableWithTransaction.Rows)
            {
                string[] elemOfTransaction = transaction[1].ToString().Split(',');
                if(elemOfTransaction[0] == "e")
                {
                    forCheckMushrooms[int.Parse(transaction[2].ToString())].numberOfEdibleMushrooms++;
                }
                else if (elemOfTransaction[0] == "p")
                {
                    forCheckMushrooms[int.Parse(transaction[2].ToString())].numberOfPoisonMushrooms++;
                }
            }

            //отчищаем журнал и выводим результат проверки
            rTB_journal.Clear();
            for (int i = 0; i < forCheckMushrooms.Count; i++)
            {
                rTB_journal.AppendText("in claster №" + i +
                    " count edible mushrooms = " + forCheckMushrooms[i].numberOfEdibleMushrooms +
                    ", and count poison mushrooms = " + forCheckMushrooms[i].numberOfPoisonMushrooms + "\n");
            }          
        }

       
    }

}
        

