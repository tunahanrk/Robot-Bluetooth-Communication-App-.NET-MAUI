using ArslanMobileApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArslanMobileApp.Data_Transactions
{
    public class DBTrans
    {
        public string dbPath;
        private SQLiteConnection conn;

        public  DBTrans(string _dbPath)
        {
            this.dbPath = _dbPath;
        }

        public void InitHotels()
        {
            conn = new SQLiteConnection(this.dbPath);
            conn.CreateTable<HotelsClass>();
        }

        public void InitRestaurants()
        {
            conn = new SQLiteConnection(this.dbPath);
            conn.CreateTable<RestaurantClass>();
        }

        public void InitMalls()
        {
            conn = new SQLiteConnection(this.dbPath);
            conn.CreateTable<ShoppingMallsClass>();
        }

        public List<HotelsClass> GetHotelsClasses()
        {
            InitHotels();
            return conn.Table<HotelsClass>().ToList();
        }

        public List<RestaurantClass> GetRestaurantClasses()
        {
            InitRestaurants();
            return conn.Table<RestaurantClass>().ToList();
        }

        public List<ShoppingMallsClass> GetMallClasses()
        {
            InitMalls();
            return conn.Table<ShoppingMallsClass>().ToList();
        }

        public void AddHotels(HotelsClass hotel)
        {
            conn = new SQLiteConnection(this.dbPath);
            conn.Insert(hotel);
        }

        public void AddRestaurants(RestaurantClass restaurant)
        {
            conn = new SQLiteConnection(this.dbPath);
            conn.Insert(restaurant);
        }

        public void AddMalls(ShoppingMallsClass malls)
        {
            conn = new SQLiteConnection(this.dbPath);
            conn.Insert(malls);
        }

        public void Delete(int hotel_ID)
        {
            conn = new SQLiteConnection(this.dbPath);
            conn.Delete(new HotelsClass { ID = hotel_ID});
        }

        public void ClearHotels()
        {
            conn = new SQLiteConnection(this.dbPath);
            conn.DeleteAll<HotelsClass>();
        }

        public void ClearRetaurants()
        {
            conn = new SQLiteConnection(this.dbPath);
            conn.DeleteAll<RestaurantClass>();
        }

        public void ClearMalls()
        {
            conn = new SQLiteConnection(this.dbPath);
            conn.DeleteAll<ShoppingMallsClass>();
        }

        public void SaveHotelsToLocalDatabase(List<HotelsClass> hotels, string dbPath)
        {
            var dbTrans = new DBTrans(dbPath);

            foreach (var hotel in hotels)
            {
                dbTrans.AddHotels(hotel);
            }
        }

        public void SaveRestaurantsToLocalDatabase(List<RestaurantClass> restaurants, string dbPath)
        {
            var dbTrans = new DBTrans(dbPath);

            foreach (var restaurant in restaurants)
            {
                dbTrans.AddRestaurants(restaurant);
            }
        }

        public void SaveMallsToLocalDatabase(List<ShoppingMallsClass> malls, string dbPath)
        {
            var dbTrans = new DBTrans(dbPath);

            foreach (var mall in malls)
            {
                dbTrans.AddMalls(mall);
            }
        }

    }
}
