using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Reflection.Metadata;

namespace NezarkaBookstore
{
    //Format of input
    //GET _CustId_ http://www.nezarka.net/Books

    //Example of input
    //GET 1 http://www.nezarka.net/Books


    class Controller
    {
        TextReader Requests;
        ModelStore modelStore;
        Viewer viewerStore = new Viewer();
        public Controller(TextReader Requests, ModelStore modelStore) 
        {
            this.modelStore = modelStore;
            this.Requests = Requests;
        }

        public Book getBookFromModel(int id)
        {
            return modelStore.GetBook(id);
        }
        public void ParseRequests()
        {
            string? request = Requests.ReadLine();

            while (request != null) 
            {
                ParseOneRequest(request);
                request = Requests.ReadLine();
            }
        }
        void ParseBooksRequest(Customer customer, string[] link) 
        {
            string name = customer.FirstName;
            int cartItemsCount = customer.ShoppingCart.Items.Count;
            List<Book> books = (List<Book>)modelStore.GetBooks();
            switch (link.Length)
            {
                case 3:
                    viewerStore.ShowBooksPage(name, cartItemsCount, books);
                    break;
                case 5:
                    if (link[3] != "Detail") throw new Exception();
                    int BookId = -1;
                    try
                    {
                        BookId = int.Parse(link[4]);
                    } catch (Exception) { throw new Exception(); }

                    Book book = modelStore.GetBook(BookId);
                    if (book == null) throw new Exception();

                    viewerStore.ShowBookDetailsPage(name, cartItemsCount, book);
                    break;
                default: 
                   throw new Exception();

            }
        }
        void ParseShoppingCartRequest(Customer customer, string[] link) 
        {
            string name = customer.FirstName;
            int cartItemsCount = customer.ShoppingCart.Items.Count;
            ShoppingCart shoppingCart = customer.ShoppingCart;
            

            if (link.Length < 3 ) throw new Exception();
            switch (link.Length) 
            {
                case 3:
                    viewerStore.ShowShoppingCartPage(name, cartItemsCount, shoppingCart);

                    break;
                case 5:
                    if (link[3] == "Add")
                    {

                    }
                    else if (link[3] == "Remove")
                    {

                    }
                    else throw new Exception();
                    break;
                default : throw new Exception();
            }
        }
        void ParseLink(Customer customer, string link)
        {
            string[] tokens = link.Split('/', StringSplitOptions.RemoveEmptyEntries);
            //for (int i = 0; i < tokens.Length; i++) { Console.WriteLine(tokens[i]); }
            int tokensLenght = tokens.Length;
            if (tokensLenght < 3) throw new Exception();
            if (tokens[0] != "http:" || tokens[1] != "www.nezarka.net") throw new Exception();

            switch(tokens[2]) 
            {
                case "Books":
                    ParseBooksRequest(customer, tokens);
                    break;
                case "ShoppingCart":
                    ParseShoppingCartRequest(customer, tokens);
                    break;
                default: throw new Exception();
            }
        
        }
        void ParseOneRequest(string request) 
        {
            string[] tokens = request.Split(' ');
            //for (int i = 0; i < tokens.Length; i++) { Console.WriteLine(tokens[i]); }
            
            if (tokens.Length  != 3||tokens[0] != "GET")
            {
                viewerStore.ShowInvalidRequest();
                return;
            }

            int CustId;
            try
            {
                CustId = int.Parse(tokens[1]);
                Customer customer = modelStore.GetCustomer(CustId);
                if (customer == null)
                    throw new Exception();
                ParseLink(customer, tokens[2]);

            }
            catch (Exception)
            {
                viewerStore.ShowInvalidRequest();
            }
        }
    }
}