using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestAdvanced : MonoBehaviour
{
    //delegate int NumberChanger(int n);

    //static int num = 10;

    //public static int AddNum(int p)
    //{
    //    num += p;
    //    return num;
    //}

    //public static int MultNum(int q)
    //{
    //    num *= q;
    //    return num;
    //}

    //public static int getNum()
    //{
    //    return num;
    //}

    //static FileStream fs;
    //static StreamWriter sw;

    //private delegate void printString(string s);
    //private void WriteToScreen(string str)
    //{
    //    Debug.Log(str);
    //}

    //private void sendString(printString ps)
    //{
    //    ps("Hello World");
    //}

    private void Start()
    {
        //NumberChanger nc1 = AddNum;
        //NumberChanger nc2 = MultNum;

        //nc1(25);
        //Debug.Log($"Value of Num: {getNum()}");
        //nc2(5);
        //Debug.Log($"Value of Num: {getNum()}");

        //printString ps1 = new printString(WriteToScreen);
        //sendString(ps1);
    }
}