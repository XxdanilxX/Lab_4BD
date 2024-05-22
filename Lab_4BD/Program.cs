using MySql.Data.MySqlClient;
using System;
using System.Data;


namespace Lab_4BD
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Отримання підключення ...");
            MySqlConnection conn = DBUtils.GetDBConnection();

            try
            {
                Console.WriteLine("Відкриття підключення ...");

                conn.Open();

                Console.WriteLine("Підключення успішне!");

                // Викликаємо методи для виконання збережених процедур
                AddFarm(conn, "Нове Господарство", "Київська", "Оболонський", "04212", "+380123456789", "Іван Іванович");
                AddFertilizer(conn, "Нове Добриво", "Виробник1", 10.5m, 5000.0m, 12);
                AddOrder(conn, new DateTime(2024, 05, 22), 1, "Нове Добриво", 1, "Категорія1", 50.0m, new DateTime(2024, 06, 22));
                GetOrdersByDate(conn, new DateTime(2024, 05, 22));
            }
            catch (Exception e)
            {
                Console.WriteLine("Помилка: " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            Console.Read();
        }

        private static void AddFarm(MySqlConnection conn, string назваГосподарства, string область, string район, string індекс, string телефон, string головаГосподарства)
        {
            MySqlCommand cmd = new MySqlCommand("AddFarm", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_назва_господарства", назваГосподарства);
            cmd.Parameters.AddWithValue("@p_область", область);
            cmd.Parameters.AddWithValue("@p_район", район);
            cmd.Parameters.AddWithValue("@p_індекс", індекс);
            cmd.Parameters.AddWithValue("@p_телефон", телефон);
            cmd.Parameters.AddWithValue("@p_голова_господарства", головаГосподарства);

            cmd.ExecuteNonQuery();
        }

        private static void AddFertilizer(MySqlConnection conn, string назваДобрива, string виробник, decimal нормаВикористання, decimal вартістьТонни, int термінЗберігання)
        {
            MySqlCommand cmd = new MySqlCommand("AddFertilizer", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_назва_добрива", назваДобрива);
            cmd.Parameters.AddWithValue("@p_виробник", виробник);
            cmd.Parameters.AddWithValue("@p_норма_використання", нормаВикористання);
            cmd.Parameters.AddWithValue("@p_вартість_тонни", вартістьТонни);
            cmd.Parameters.AddWithValue("@p_термін_зберігання", термінЗберігання);

            cmd.ExecuteNonQuery();
        }

        private static void AddOrder(MySqlConnection conn, DateTime датаЗаповнення, int кодЗамовника, string назваДобрива, int кодДобрива, string категоріяПільг, decimal площаДляОбробки, DateTime датаПостачання)
        {
            MySqlCommand cmd = new MySqlCommand("AddOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_дата_заповнення", датаЗаповнення);
            cmd.Parameters.AddWithValue("@p_код_замовника", кодЗамовника);
            cmd.Parameters.AddWithValue("@p_назва_добрива", назваДобрива);
            cmd.Parameters.AddWithValue("@p_код_добрива", кодДобрива);
            cmd.Parameters.AddWithValue("@p_категорія_пільг", категоріяПільг);
            cmd.Parameters.AddWithValue("@p_площа_для_обробки", площаДляОбробки);
            cmd.Parameters.AddWithValue("@p_дата_постачання", датаПостачання);

            cmd.ExecuteNonQuery();
        }

        private static void GetOrdersByDate(MySqlConnection conn, DateTime date)
        {
            MySqlCommand cmd = new MySqlCommand("GetOrdersByDate", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_date", date);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Код замовлення: {reader["код_замовлення"]}, Дата заповнення: {reader["дата_заповнення"]}, Код замовника: {reader["код_замовника"]}, Назва добрива: {reader["назва_добрива"]}, Площа для обробки: {reader["площа_для_обробки"]}, Дата постачання: {reader["дата_постачання"]}");
                }
            }
        }
    }
}


