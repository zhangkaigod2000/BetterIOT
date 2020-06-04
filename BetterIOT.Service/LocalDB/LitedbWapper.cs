using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterIOT.Service.LocalDB
{
    public class LitedbWapper
    {
        string DbPath = "";
        public LitedbWapper(string DBPath)
        {
            DbPath = DBPath;
        }

        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        public void Insert<T>(IEnumerable<T> datas)
        {
            using (var db = new LiteDatabase(DbPath))
            {
                try
                {
                    // Get a collection (or create, if doesn't exist)
                    var col = db.GetCollection<T>(typeof(T).ToString());
                    foreach(T data in datas)
                    {
                        col.Insert(data);
                    }
                }
                catch(Exception ex)
                {

                }
            }
        }

        public IEnumerable<T> SeachAll<T>()
        {
            IEnumerable<T> TDatas = null; ;
            using (var db = new LiteDatabase(DbPath))
            {
                try
                {
                    // Get a collection (or create, if doesn't exist)
                    var col = db.GetCollection<T>(typeof(T).ToString());
                    TDatas = col.FindAll();
                }
                catch (Exception ex)
                {

                }
            }
            return TDatas;
        }

        public T FindById<T>(int docId)where T : new()
        {
            // Open data file (or create if not exits)
            using (var db = new LiteDatabase(DbPath))
            {
                // Get a collection (or create, if not exits)
                var col = db.GetCollection(typeof(T).ToString());
                var doc = col.FindById(1);
                return BsonToObject.ConvertTo<T>(doc);
            }
        }
        public IList<BsonDocument> FindAll<T>()
        {
            // Open data file (or create if not exits)
            using (var db = new LiteDatabase(DbPath))
            {
                // Get a collection (or create, if not exits)
                var col = db.GetCollection(typeof(T).ToString());
                var doc = col.FindAll().ToList();
                return doc;
            }
        }
        public bool Delete(int docId,string TypeName)
        {
            using (var db = new LiteDatabase(DbPath))
            {
                var col = db.GetCollection(TypeName);
                var success = col.Delete(docId);
                return success;
            }
        }


        public BsonDocument FindById(int docId, string TypeName)
        {
            using (var db = new LiteDatabase(DbPath))
            {
                var col = db.GetCollection(TypeName);
                var doc = col.FindById(1);
                return doc;
            }
        }

        public (ILiteCollection<BsonDocument>, LiteDatabase) GetLiteCollection<T>()
        {
            var db = new LiteDatabase(DbPath);
            var col = db.GetCollection(typeof(T).ToString());
            return (col,db);
        }

        public void Clear<T>()
        {
            using (var db = new LiteDatabase(DbPath))
            {
                try
                {
                    var col = db.GetCollection<T>(typeof(T).ToString());
                    col.DeleteAll();
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
