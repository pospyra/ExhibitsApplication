using System.Collections.Generic;
using ExhibitsApplication.Models;

namespace ExhibitsApplication.Services
{
    public class ExhibitsStorage
    {
        private static ExhibitsStorage instance;
        private List<ExhibitsModel> exhibitsList;

        // Приватный конструктор для предотвращения создания экземпляров класса вне самого класса
        private ExhibitsStorage()
        {
            exhibitsList = new List<ExhibitsModel>();
        }

        // Статический метод для получения единственного экземпляра хранилища
        public static ExhibitsStorage GetInstance()
        {
            if (instance == null)
            {
                instance = new ExhibitsStorage();
            }
            return instance;
        }

        // Метод для добавления выставки в хранилище
        public void AddExhibit(ExhibitsModel exhibit)
        {
            exhibitsList.Add(exhibit);
        }

        // Метод для получения списка всех выставок
        public List<ExhibitsModel> GetAllExhibits()
        {
            return exhibitsList;
        }

        // Метод для очистки хранилища
        public void Clear()
        {
            exhibitsList.Clear();
        }
    }
}
