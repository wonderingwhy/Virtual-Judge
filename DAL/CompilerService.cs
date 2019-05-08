using Helpers;
using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL
{
    public static class CompilerService
    {
        public static List<Compiler> SelectByOJ(uint OJID)
        {
            string sql = @"select * from compiler where OJID = @OJID";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@OJID", OJID));
            List<Compiler> compilers = new List<Compiler>();
            foreach (DataRow dr in dt.Rows)
            {
                Compiler compiler = new Compiler();
                compiler.CompilerID = (uint)dr["CompilerID"];
                compiler.OJID = (uint)dr["OJID"];
                compiler.Name = (string)dr["Name"];
                compilers.Add(compiler);
            }
            return compilers;
        }
    }
}
