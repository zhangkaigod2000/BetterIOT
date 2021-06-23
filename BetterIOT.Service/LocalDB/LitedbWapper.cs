using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterIOT.Service.LocalDB
{
    public class LitedbWapper:IDisposable
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
                    var col = db.GetCollection<T>();
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
                    var col = db.GetCollection<T>();
                    TDatas = col.FindAll();
                }
                catch (Exception ex)
                {

                }
            }
            return TDatas;
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



        public void Clear<T>()
        {
            using (var db = new LiteDatabase(DbPath))
            {
                try
                {
                    var col = db.GetCollection<T>();
                    col.DeleteAll();
                }
                catch (Exception ex)
                {

                }
            }
        }

        public void Dispose()
        {
            
        }
    }
}
