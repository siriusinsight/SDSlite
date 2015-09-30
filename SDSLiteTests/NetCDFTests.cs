﻿using System;
using NUnit.Framework;

using Microsoft.Research.Science.Data;
using System.Linq;

namespace SDSLiteTests
{
    public class NetCDFTests
    {
        public void Empty_AddVariable<T>(T[] data)
        {
            var fn = System.IO.Path.GetTempFileName();
            try
            {
                var dsuri = new Microsoft.Research.Science.Data.NetCDF4.NetCDFUri();
                dsuri.FileName = fn;
                dsuri.OpenMode = ResourceOpenMode.Create;
                var vname = "a";
                using (var ds = DataSet.Open(dsuri))
                {
                    ds.AddVariable(typeof(T), vname, data, "a");
                    ds.Commit();
                }
                dsuri.OpenMode = ResourceOpenMode.ReadOnly;
                using (var ds = DataSet.Open(dsuri))
                {
                    Assert.IsTrue(ds.Variables.Contains(vname));
                    var v = ds[vname];
                    Assert.AreEqual(1, v.Rank);
                    Assert.AreEqual(typeof(T), v.TypeOfData);
                    var d = v.GetData() as T[];
                    Assert.IsNotNull(d);
                    Assert.AreEqual(data.Length, d.Length);
                    for (int i = 0; i < data.Length; i++)
                        Assert.AreEqual(data[i], d[i], String.Format("index={0}", i));
                }

            }
            finally
            {
                System.IO.File.Delete(fn);
            }
        }
        [Test]
        public void Empty_AddVariable_double()
        {
            Empty_AddVariable<double>(new double[]{ -1.0, 0.0, 3.0, Math.PI, Double.Epsilon, Double.PositiveInfinity });
        }
        [Test]
        public void Empty_AddVariable_float()
        {
            Empty_AddVariable<float>(new float[] { -1.0f, 0.0f, 3.0f, 4.0f, Single.Epsilon, Single.NegativeInfinity });
        }
        [Test]
        public void Empty_AddVariable_int()
        {
            Empty_AddVariable<int>(new int[] { 1, 0, Int32.MinValue, Int32.MaxValue });
        }
        [Test]
        public void Empty_AddVariable_string()
        {
            Empty_AddVariable(new string[] { "string1", "", null, new String(Enumerable.Repeat('x',4096).ToArray()),"line1\r\nline2", "English Русский" });
        }
        //[Test]
        //public void Empty_AddVariable_string()
        //{
        //    Empty_AddVariable<string>(new string[] { "a", "", "b"});
        //}
        public void Empty_AddAttribute<T>(T data)
        {
            var fn = System.IO.Path.GetTempFileName();
            try
            {
                var dsuri = new Microsoft.Research.Science.Data.NetCDF4.NetCDFUri();
                dsuri.FileName = fn;
                dsuri.OpenMode = ResourceOpenMode.Create;
                var vname = "a";
                using (var ds = DataSet.Open(dsuri))
                {
                    ds.Metadata[vname]=data;
                }
                dsuri.OpenMode = ResourceOpenMode.ReadOnly;
                using (var ds = DataSet.Open(dsuri))
                {
                    Assert.IsTrue(ds.Metadata.ContainsKey(vname));
                    var v = ds.Metadata[vname];
                    Assert.IsNotNull(v);
                    Assert.AreEqual(typeof(T), v.GetType());
                    if (typeof(T).IsArray) {
                        var darr = data as Array;
                        var varr = v as Array;
                        for (int i = 0; i < darr.Length; i++)
                            Assert.AreEqual(darr.GetValue(i), varr.GetValue(i), String.Format("index={0}", i));
                    }
                    else
                        Assert.AreEqual(data, v);
                }

            }
            finally
            {
                System.IO.File.Delete(fn);
            }
        }
        [Test]
        public void Empty_AddAttribute_double()
        {
            Empty_AddAttribute(Math.PI);
        }
        [Test]
        public void Empty_AddAttribute_double0()
        {
            Empty_AddAttribute(Math.PI);
        }
        [Test]
        public void Empty_AddAttribute_double1()
        {
            Empty_AddAttribute(new double[]{Math.PI});
        }
        [Test]
        public void Empty_AddAttribute_double2()
        {
            Empty_AddAttribute(new double[]{0.0,1.0});
        }
        [Test]
        public void Empty_AddAttribute_float()
        {
            Empty_AddAttribute(0.0f);
        }
        [Test]
        public void Empty_AddAttribute_int()
        {
            Empty_AddAttribute(Int32.MinValue);
        }
        [Test]
        public void Empty_AddAttribute_uint()
        {
            Empty_AddAttribute(UInt32.MaxValue);
        }
        [Test]
        public void Empty_AddAttribute_string()
        {
            Empty_AddAttribute("string");
        }
        [Test]
        public void Empty_AddAttribute_string_empty()
        {
            Empty_AddAttribute("");
        }
        [Test]
        public void Empty_AddAttribute_string0()
        {
            Empty_AddAttribute(new string[]{});
        }
        [Test]
        public void Empty_AddAttribute_string1()
        {
            Empty_AddAttribute(new string[] { "string" });
        }
        [Test]
        public void Empty_AddAttribute_string2()
        {
            Empty_AddAttribute(new string[] { "", "string", "English, Русский" });
        }
        [Test]
        public void Empty_AddAttribute_UtfString()
        {
            Empty_AddAttribute("Русский");
        }

    }
}
