using System;
using System.Linq;
using System.Data.OleDb;
using System.Data;
using System.Data.Common;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace DataCenterOperation.Util
{
    public class Excel_Utils
    {
        public static List<string[]> GetSheetValuesFromExcel(string path, int index = 0)
        {
            List<string[]> returnList = new List<string[]>();
            var str = new ArrayList();
            using (FileStream fs = File.OpenRead(path))
            {
                HSSFWorkbook wk = new HSSFWorkbook(fs);   //把xls文件中的数据写入wk中
                ISheet sheet = wk.GetSheetAt(index);
                bool isNullRow = true;
                for (int i = 0; i <=sheet.LastRowNum; i++) 
                {
                    isNullRow = true;
                    IRow row = sheet.GetRow(i);                    
                    for (int k = 0; k <= row.LastCellNum; k++)  //LastCellNum 是当前行的总列数
                    {                        
                        ICell cell = row.GetCell(k);  //当前表格
                        if (cell != null)
                        {
                            str.Add(cell.ToString());   //获取表格中的数据并转换为字符串类型                            
                        }
                        else
                        {
                            str.Add("");
                        }                        
                    }

                    foreach (string item in str) //全是空格，就是空行。
                    {
                        if (item != "") { isNullRow = false; }
                    }

                    if ( isNullRow )
                        { break; } //碰到空行退出     
                    else
                        { returnList.Add((string[])str.ToArray(typeof(string))); }                          
                    str.Clear();
                    
                }                
            }
            return (returnList);
        }        
    }
}