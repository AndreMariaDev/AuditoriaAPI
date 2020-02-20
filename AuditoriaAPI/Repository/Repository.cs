using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuditoriaAPI.Model;
using Dapper;

namespace AuditoriaAPI.Repository
{
    public static class Repository<T> where T : class
    {
        static Util.FileCreator fileCreator = new Util.FileCreator();

        public static IEnumerable<T> getList(String query, enumConnectionString connectionStrings)
        {
            String ConnectionStrings = GetConnectionStrings(connectionStrings);
            try
            {
                using (IDbConnection db = new SqlConnection(ConnectionStrings))
                {
                    StringBuilder strGet = new StringBuilder();
                    fileCreator.WriteTxtLog(String.Format("[{0}] - {1}", DateTime.Now.ToString("dd/MM/yyyy 'às' HH:mm"), strGet.ToString()));

                    return db.Query<T>(query).AsEnumerable();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("[{0}] - {1}", DateTime.Now.ToString("dd/MM/yyyy 'às' HH:mm"), ex.ToString()));
                fileCreator.WriteTxtLog(String.Format("[{0}] - {1}", DateTime.Now.ToString("dd/MM/yyyy 'às' HH:mm"), ex.ToString()));
                return null;
            }
        }

        public static int getCount(String query, enumConnectionString connectionStrings)
        {
            String ConnectionStrings = GetConnectionStrings(connectionStrings);
            try
            {
                using (IDbConnection db = new SqlConnection(ConnectionStrings))
                {
                    fileCreator.WriteTxtLog(String.Format("[{0}] - {1}", DateTime.Now.ToString("dd/MM/yyyy 'às' HH:mm"), query));

                    return db.Query<int>(query).Single();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("[{0}] - {1}", DateTime.Now.ToString("dd/MM/yyyy 'às' HH:mm"), ex.ToString()));
                fileCreator.WriteTxtLog(String.Format("[{0}] - {1}", DateTime.Now.ToString("dd/MM/yyyy 'às' HH:mm"), ex.ToString()));
                return 0;
            }
        }

        public static Auditoria GetAuditoria(String query, enumConnectionString connectionStrings)
        {
            try
            {
                String ConnectionStrings = GetConnectionStrings(enumConnectionString.DapperAuditoria);
                using (IDbConnection db = new SqlConnection(ConnectionStrings))
                {
                    db.Open();
                    var AuditoriaDictionary = new Dictionary<int, Auditoria>();
                    var result = db.Query<Auditoria, AuditoriaItem, Auditoria>(
                            query,
                                (A, AI) =>
                                {
                                    Auditoria AuditoriaEntry;
                                    if (!AuditoriaDictionary.TryGetValue(A.id, out AuditoriaEntry))
                                    {
                                        AuditoriaEntry = A;
                                        AuditoriaEntry.AuditoriaItem = new List<AuditoriaItem>();
                                        AuditoriaDictionary.Add(A.id, AuditoriaEntry);
                                    }
                                    AuditoriaEntry.AuditoriaItem.Add(AI);
                                    return AuditoriaEntry;
                                }).Distinct().ToList();

                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    

        public static void Insert(String query, enumConnectionString connectionStrings)
        {
            String ConnectionStrings = GetConnectionStrings(connectionStrings);
            try
            {
                using (IDbConnection db = new SqlConnection(ConnectionStrings))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        try
                        {
                            db.Execute(query, transaction: transaction);
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("[{0}] - {1}", DateTime.Now.ToString("dd/MM/yyyy 'às' HH:mm"), ex.ToString()));
                fileCreator.WriteTxtLog(String.Format("[{0}] - {1}", DateTime.Now.ToString("dd/MM/yyyy 'às' HH:mm"), ex.ToString()));
            }
        }

        public static void Update(String query, enumConnectionString connectionStrings)
        {
            String ConnectionStrings = GetConnectionStrings(connectionStrings);
            try
            {
                using (IDbConnection db = new SqlConnection(ConnectionStrings))
                {
                    db.Execute(query);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("[{0}] - {1}", DateTime.Now.ToString("dd/MM/yyyy 'às' HH:mm"), ex.ToString()));
                fileCreator.WriteTxtLog(String.Format("[{0}] - {1}", DateTime.Now.ToString("dd/MM/yyyy 'às' HH:mm"), ex.ToString()));
            }
        }

        public static String GetConnectionStrings(enumConnectionString connectionStrings)
        {
            String ConnectionStrings = string.Empty;
            if (connectionStrings == enumConnectionString.DapperAuditoria)
                ConnectionStrings = ConfigurationManager.ConnectionStrings["DapperAuditoria"].ConnectionString;
            else if (connectionStrings == enumConnectionString.DapperVortice2)
                ConnectionStrings = ConfigurationManager.ConnectionStrings["DapperVortice2"].ConnectionString;

            return ConnectionStrings;
        }
    }
}