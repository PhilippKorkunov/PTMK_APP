using MyApp.Model;
using MyAppDbContext.Entities;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyApp
{
    public static class Service
    {

        //modelViws - записи представления, котороые нужно отобразить. requiredColumns - перечень столбцов, которые нужно отобразить
        public static void ToConsoleViews<T>(this IEnumerable<T> modelViews, IEnumerable<string>? requiredColumns = null) where T : IViewModel
        {
            PropertyInfo[] propertyInfo;
            Type myType = typeof(T);
            propertyInfo = myType.GetProperties(); //свойства представления (колонки таблицы)
            string[] propertyNames = myType.GetProperties().Select(x => x.Name).ToArray(); //имена колонок

            if (requiredColumns is not null)
            {
                var exception = requiredColumns.Except(propertyNames);

                if (exception.Any())
                {
                    throw new Exception($"Неправильно указаны имена столбцов: {String.Join(", ", exception)}"); // Проверка перечня столбцов, которые нужно отобразить
                }
            }
            else
            {
                requiredColumns = propertyNames; //нужно отобразить все стобцы, эквивалентно в SQL - '*')
            }

            Dictionary<string, List<string>> columns = new ();// словарь столбцов, где ключ - имя столбца, список - данные по этому столбцу
            var listColumns = requiredColumns.ToList();
            List<T> listViews = modelViews.ToList();
            var listMaxes = new List<int>(); // список максимальной длины отображаемых данных для столбцов 

            for (int i = 0; i < listColumns.Count; i++)
            {
                columns[listColumns[i]] = new List<string>() { listColumns[i] };
            }

            for (int i = 0; i < listViews.Count; i++) //Полцчение значений для каждго modelView по каждому необходимому свойству (столбцу)
            {
                foreach (var prop in propertyInfo)
                {
                    if (prop is not null)
                    {
                        string? value = prop.GetValue(listViews[i]) is null ? string.Empty : 
                            prop.GetValue(listViews[i]) is null ? string.Empty : prop.GetValue(listViews[i]).ToString(); //Значение свойста по имени свойства
                        columns[prop.Name].Add(value is null ? string.Empty : value);
                    }
                    else
                    {
                        throw new Exception($"Property {prop} is null");
                    }
                }
            }


            for (int i = 0; i < listColumns.Count; i++)
            {
                listMaxes.Add(columns[propertyNames[i]].MaxBy(x =>x.Length) is null ? 0 : columns[propertyNames[i]].MaxBy(x => x.Length).Length);
            }

            int length = listMaxes.Sum() + listMaxes.Count * 2 + listColumns.Count + 1; // длина всей таблицы, где listMaxes.Sum() + listMaxes.Count * 2 - длина всех столбцов
                                                                                        // (listMaxes.Count * 2) - все отступы (слева и справа по одному от границ)
                                                                                        //  listColumns.Count + 1 - количество границ
            Console.WriteLine(new StringBuilder().Insert(0, "-",length)); //Верхняя граница таблицы
            for(int i = 0; i <= listViews.Count; i++)
            {
                string output = "|";
                for (int j=0; j<listColumns.Count; j++)
                {
                    output += $" {columns[propertyNames[j]][i] + new StringBuilder().Insert(0, " ", listMaxes[j] - columns[propertyNames[j]][i].Length)} |";
                }
                Console.WriteLine(output);
                Console.WriteLine(new StringBuilder().Insert(0, "-", length));//Разделительная черта между записями (строками)
            }
        }

        public static IEnumerable<UserModelView> ToUserModelViews(this IEnumerable<User> users) => from user in users
                                                                                                    select new UserModelView(user);
    }
}
