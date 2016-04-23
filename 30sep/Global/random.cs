using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _30sep.Models;

namespace _30sep.Global
{
    public class random
    {
        //Retrieve a random Book:
public Book RandomBook(List<Book> list)
{
    Random randNum = new Random();
    return  list[randNum.Next(list.Count)];
}



        


    }
}