using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_CLOPE
{
    public class Claster
    {
        // количество транзакций в кластере - N
        public int countTransaction = 0;
        //Ширина кластера, то сколько в нем уникадьных элементов - W
        public int uniqueItem = 0;
        //словарь количества элементов каждлго вида - Occ
        public Dictionary<string, int> DictOcc = new Dictionary<string, int>();
        //площадь кластера, то сколько в нем элементов всего - S
        public int square = 0;

        //конструктор с заданием пустого кластера
        public Claster() { }

       //конструктор для создания копии кластера
        public Claster(Claster C)
        {
            countTransaction = C.countTransaction;
            uniqueItem = C.uniqueItem;
            DictOcc = new Dictionary<string,int>(C.DictOcc);
            square = C.square;
        }

       
        //конструктор кластера по транзакции для создания нового не пустого кластера
        public Claster(object transaction)
        {
            //устанавливаем колличество транзакций в кластере
            countTransaction = 1;

            //разбиваем транзакцию на элементы
            string[] transactionElements = transaction.ToString().Split(',');

            //устанавливаем колличество элементов в кластере
            square = transactionElements.Length;

            //заполняем словарь элементами транзакции и их колличеством
            for (int i = 0; i < transactionElements.Length; i++)
            {
                if (!DictOcc.ContainsKey(transactionElements[i]))
                    DictOcc.Add(transactionElements[i], 1);
                else{
                    DictOcc[transactionElements[i]]++;
                }
            }

            //устанавливаем колличество уникальных элементов в кластере
            uniqueItem = DictOcc.Count;           
        }

        //метод для добавления транзакции к существующему кластеру
        public void addNewTransaction(object transaction)
        {
            //увеличиваем количество транзакций в кластере
            countTransaction++;

            //разбиваем транзакцию на элементы
            string[] transactionElements = transaction.ToString().Split(',');

            //добавляем колличество новых элементов к кластеру
            square += transactionElements.Length;

            //дополняем словарь
            for (int i = 0; i < transactionElements.Length; i++)
            {
                if (!DictOcc.ContainsKey(transactionElements[i]))
                    DictOcc.Add(transactionElements[i], 1);
                else
                {
                    DictOcc[transactionElements[i]]++;
                }
            }

            //устанавливаем колличество уникальных элементов в кластере
            uniqueItem = DictOcc.Count;
        }

        //метод для удаления транзакции из кластера
        public void RemoveTransaction(object transaction)
        {
            //уменьшаем количество транзакций в кластере
            countTransaction--;

            //разбиваем транзакцию на элементы
            string[] transactionElements = transaction.ToString().Split(',');

            //уменьшаем колличество элементов в кластере
            square -= transactionElements.Length;

            //корректируем словарь словарь
            for (int i = 0; i < transactionElements.Length; i++)
            {
                DictOcc[transactionElements[i]]--;
                if (DictOcc[transactionElements[i]] == 0)
                {
                    DictOcc.Remove(transactionElements[i]);
                }
            }

            //устанавливаем колличество уникальных элементов в кластере
            uniqueItem = DictOcc.Count;
        }
    }
}
